<template>
  <div>
    <h1 class="text-center">My Camping Spots</h1>
    <div class="card-container">
      <div v-for="spot in paginatedCampingSpots" :key="spot.id" class="card">
        <h2>{{ spot.title }}</h2>
        <div class="image-container">
          <img :src="`data:image/jpeg;base64,${spot.images[0]}`" alt="Camping Spot Image" class="spot-image">
        </div>
        <h2>{{ spot.description}}</h2>
        <div class="actions">
          <button class="action-button" @click="editSpot(spot.id)">Edit</button>
          <button class="action-button" @click="deleteSpot(spot.id)">Delete</button>
        </div>
      </div>
    </div>
    <div class="pagination">
      <button class="pagination-button" @click="prevPage" :disabled="currentPage === 1">Previous</button>
      <button
        v-for="page in totalPages"
        :key="page"
        @click="goToPage(page)"
        :class="['pagination-button', { active: currentPage === page }]"
      >{{ page }}</button>
      <button class="pagination-button" @click="nextPage" :disabled="currentPage === totalPages">Next</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'bookingList',
  data() {
    return {
      campingSpots: [],
      currentPage: 1,
      itemsPerPage: 6
    };
  },
  computed: {
    paginatedCampingSpots() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.campingSpots.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.campingSpots.length / this.itemsPerPage);
    }
  },
  methods: {
    async fetchCampingSpots() {
      const user = JSON.parse(localStorage.getItem('user'));
      const ownerId = user ? user.ownerId : null;

      if (!ownerId) {
        console.error('No owner ID found in local storage.');
        return;
      }

      try {
        const response = await axios.get(`http://localhost:5000/api/campingspots/owner/${ownerId}`);
        this.campingSpots = response.data;
      } catch (error) {
        console.error('Error fetching camping spots:', error);
      }
    },
    editSpot(id) {
      this.$router.push({ name: 'bookingListedit', params: { id } });
    },
    async deleteSpot(id) {
      try {
        await axios.delete(`http://localhost:5000/api/campingspots/${id}`);
        this.fetchCampingSpots();
      } catch (error) {
        console.error('Error deleting spot:', error.response ? error.response.data : error.message);
      }
    },
    prevPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
      }
    },
    goToPage(page) {
      this.currentPage = page;
    }
  },
  mounted() {
    this.fetchCampingSpots();
  }
};
</script>

<style scoped>
.text-center {
  text-align: center;
}

.card-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between; 
  max-width: 1200px; 
  margin: 0 auto; 
}

.card {
  width: calc(30% - 20px);
  margin-bottom: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-sizing: border-box;
}

.actions {
  display: flex;
  justify-content: space-between;
  margin-top: 10px;
}

.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.pagination-button, .action-button {
  background-color: green;
  color: white;
  border: none;
  padding: 10px 20px;
  margin: 0 5px;
  border-radius: 5px;
  cursor: pointer;
}

.pagination-button:disabled, .action-button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.pagination-button.active {
  background-color: #1383099a;
  font-weight: bold;
}
</style>
