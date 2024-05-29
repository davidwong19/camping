<template>
  <div>
    <h1 class="text-center">Available Camping Spots</h1>
    <div class="card-container">
      <div v-for="spot in paginatedCampingSpots" :key="spot.id" class="card">
        <h2>{{ spot.title }}</h2>
        <div class="image-container">
          <img :src="`data:image/jpeg;base64,${spot.images[0]}`" alt="Camping Spot Image" class="spot-image">
        </div>
        <div class="details">
          <p>Country: {{ spot.country }}</p>
          <p>Location: {{ spot.location }}</p>
          <p>Average Rating: {{ spot.averageRating || 'No ratings yet' }}</p>
        </div>
      </div>
    </div>
    <div class="pagination">
      <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
      <button
        v-for="page in totalPages"
        :key="page"
        @click="goToPage(page)"
        :class="{ active: currentPage === page }"
      >{{ page }}</button>
      <button @click="nextPage" :disabled="currentPage === totalPages">Next</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'CampingSpots',
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
      try {
        const response = await axios.get('http://localhost:5000/api/campingspots');
        const spots = response.data;

        // Fetch average ratings
        const ratingPromises = spots.map(async spot => {
          try {
            const ratingResponse = await axios.get(`http://localhost:5000/api/campingspots/${spot.id}/average-rating`);
            spot.averageRating = ratingResponse.data;
          } catch (ratingError) {
            console.error(`Error fetching rating for spot ${spot.id}:`, ratingError);
            spot.averageRating = 'No rating';
          }
          return spot;
        });

        this.campingSpots = await Promise.all(ratingPromises);
      } catch (error) {
        console.error('Error fetching camping spots:', error);
      }
    },
    startBooking(spotId) {
      // Redirect to loginmessage page with spotId parameter
      this.$router.push({ name: 'loginmessage', params: { spotId } });
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

<style>
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

.image-container {
  width: 100%;
  height: 150px; 
  overflow: hidden;
  margin-bottom: 10px;
}

.spot-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.details {
  padding: 10px 0;
}

.book-button {
  background-color: #007bff;
  color: #fff;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.pagination button {
  background-color: green;
  color: white;
  border: none;
  padding: 10px 20px;
  margin: 0 5px;
  border-radius: 5px;
  cursor: pointer;
}

.pagination button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.pagination button.active {
  background-color:  #1383099a;
  font-weight: bold;
}
</style>
