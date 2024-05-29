<template>
  <div class="main-container">
    <aside class="sidebar">
      <div class="filter-card">
        <h2>Filters</h2>
        <div class="form-inputs">
          <label for="country">Country:</label>
          <input id="country" v-model="filters.country" @input="applyFilters" type="text" />
        </div>
        <div class="form-inputs">
          <label for="location">Location:</label>
          <input id="location" v-model="filters.location" @input="applyFilters" type="text" />
        </div>
        <div class="form-inputs">
          <label for="theme">Theme:</label>
          <select id="theme" v-model="filters.theme" @change="applyFilters">
            <option value="">All</option>
            <option value="By the sea">By the sea</option>
            <option value="Small campsites">Small campsites</option>
            <option value="Nature">Nature</option>
          </select>
        </div>
        <div class="form-inputs">
          <label for="onsiteaccommodation">On-site accommodation:</label>
          <select id="onsiteaccommodation" v-model="filters.onsiteaccommodation" @change="applyFilters">
            <option value="">All</option>
            <option value="campervan">Campervan</option>
            <option value="tent pitch">Tent pitch</option>
            <option value="bring your own">Bring your own</option>
          </select>
        </div>
        <div class="form-inputs">
          <label for="howmanycampers">How Many Campers:</label>
          <input id="howmanycampers" v-model.number="filters.howmanycampers" @input="applyFilters" type="number" min="0" />
        </div>
        <div class="form-inputs">
          <label for="beginDate">Begin Date:</label>
          <input id="beginDate" v-model="filters.beginDate" @input="applyFilters" type="date" />
        </div>
        <div class="form-inputs">
          <label for="endDate">End Date:</label>
          <input id="endDate" v-model="filters.endDate" @input="applyFilters" type="date" />
        </div>
        <div class="form-inputs">
          <label for="averageRating">Average Rating:</label>
          <select id="averageRating" v-model.number="filters.averageRating" @change="applyFilters">
            <option value="">All</option>
            <option value="0">0</option>
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
            <option value="5">5</option>
          </select>
        </div>
        <div class="form-inputs">
          <label for="title">Title:</label>
          <input id="title" v-model="filters.title" @input="applyFilters" type="text" />
        </div>
        <div class="form-inputs">
          <label for="isAvailable">Is Available:</label>
          <select id="isAvailable" v-model="filters.isAvailable" @change="applyFilters">
            <option value="">All</option>
            <option value="true">Yes</option>
            <option value="false">No</option>
          </select>
        </div>
      </div>
    </aside>
    <div class="content">
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
          <div class="button-group">
            <button @click="bookSpot(spot)" class="action-button">Book</button>
            <router-link :to="{ name: 'CampingSpotProfile', params: { id: spot.id } }">
              <button class="action-button">Profile</button>
            </router-link>
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
      itemsPerPage: 6,
      filters: {
        country: '',
        location: '',
        theme: '',
        onsiteaccommodation: '', // Added this line
        howmanycampers: null,
        beginDate: '',
        endDate: '',
        averageRating: null,
        title: '',
        isAvailable: '',
      },
    };
  },
  computed: {
    filteredCampingSpots() {
      return this.campingSpots.filter(spot => {
        return (
          (!this.filters.country || spot.country.toLowerCase().includes(this.filters.country.toLowerCase())) &&
          (!this.filters.location || spot.location.toLowerCase().includes(this.filters.location.toLowerCase())) &&
          (!this.filters.theme || spot.theme === this.filters.theme) &&
          (!this.filters.onsiteaccommodation || spot.onsiteaccommodation === this.filters.onsiteaccommodation) && // Added this line
          (!this.filters.howmanycampers || spot.howmanycampers == this.filters.howmanycampers) &&
          (!this.filters.beginDate || new Date(spot.beginDate) >= new Date(this.filters.beginDate)) &&
          (!this.filters.endDate || new Date(spot.endDate) <= new Date(this.filters.endDate)) &&
          (!this.filters.averageRating || spot.averageRating >= this.filters.averageRating) &&
          (!this.filters.title || spot.title.toLowerCase().includes(this.filters.title.toLowerCase())) &&
          (this.filters.isAvailable === '' || spot.isAvailable === (this.filters.isAvailable === 'true'))
        );
      });
    },
    paginatedCampingSpots() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredCampingSpots.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.filteredCampingSpots.length / this.itemsPerPage);
    }
  },
  methods: {
    async fetchCampingSpots() {
      try {
        const response = await axios.get('http://localhost:5000/api/campingspots');
        const spots = response.data;

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
    bookSpot(spot) {
      this.$router.push({ name: 'bookforum', params: { spotId: spot.id } });
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
    },
    applyFilters() {
      this.currentPage = 1;
    }
  },
  mounted() {
    this.fetchCampingSpots();
  }
};
</script>

<style scoped>
.main-container {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  padding: 20px;
  background: #f0f0f0;
}

.sidebar {
  width: 300px;
  margin-right: 20px;
}

.filter-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.filter-card h2 {
  margin-bottom: 20px;
  font-size: 24px;
  text-align: center;
}

.form-inputs {
  margin-bottom: 15px;
}

.form-inputs label {
  display: block;
  margin-bottom: 5px;
}

.form-inputs input,
.form-inputs select {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.content {
  flex-grow: 1;
  max-width: 1200px;
}

.card-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.card {
  width: calc(30% - 20px);
  margin-bottom: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-sizing: border-box;
  background: white;
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

.button-group {
  display: flex;
  justify-content: space-between;
}

.action-button {
  background-color: green;
  color: white;
  border: none;
  padding: 10px 20px;
  margin: 0 5px;
  border-radius: 5px;
  cursor: pointer;
  flex-grow: 1;
  margin: 5px;
}

.action-button:hover {
  background-color: darkgreen;
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
  background-color: #1383099a;
  font-weight: bold;
}
</style>
