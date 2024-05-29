<template>
  <div id="booking-container">
    <div class="booking-card">
      <h1>Create Camping Spot</h1>
      <form @submit.prevent="createCampingSpot" enctype="multipart/form-data">
        <div class="form-inputs">
          <label for="title">Title:</label>
          <input type="text" id="title" v-model="campingSpot.title" required>
        </div>
        <div class="form-inputs">
          <label for="country">Country:</label>
          <input type="text" id="country" v-model="campingSpot.country" required>
        </div>
        <div class="form-inputs">
          <label for="location">Location:</label>
          <input type="text" id="location" v-model="campingSpot.location" required>
        </div>
        <div class="form-inputs">
          <label for="theme">Theme:</label>
          <select id="theme" v-model="campingSpot.theme">
            <option value="By the sea">By the sea</option>
            <option value="Small campsites">Small campsites</option>
            <option value="Nature">Nature</option>
          </select>
        </div>
        <div class="form-inputs">
          <label for="howmanycampers">How many campers:</label>
          <input type="number" id="howmanycampers" v-model.number="campingSpot.howmanycampers" required>
        </div>
        <div class="form-inputs">
          <label for="onsiteaccommodation">On-site accommodation:</label>
          <select id="onsiteaccommodation" v-model="campingSpot.onsiteaccommodation">
            <option value="campervan">Campervan</option>
            <option value="tent pitch">Tent pitch</option>
            <option value="bring your own">Bring your own</option>
          </select>
        </div>
        <div class="form-inputs">
          <label for="beginDate">Begin Date:</label>
          <input type="datetime-local" id="beginDate" v-model="campingSpot.beginDate" required>
        </div>
        <div class="form-inputs">
          <label for="endDate">End Date:</label>
          <input type="datetime-local" id="endDate" v-model="campingSpot.endDate" required>
        </div>
        <div class="form-inputs">
          <label for="description">Description:</label>
          <textarea id="description" v-model="campingSpot.description" required></textarea>
        </div>
        <div class="form-inputs">
          <label for="isAvailable">Is Available:</label>
          <input type="checkbox" id="isAvailable" v-model="campingSpot.isAvailable">
        </div>
        <div class="form-inputs">
          <label for="images">Images:</label>
          <input type="file" id="images" multiple @change="handleFileUpload">
        </div>
        <button type="submit" class="booking-button">Create</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      campingSpot: {
        title: '',
        country: '',
        location: '',
        theme: '',
        howmanycampers: null,
        onsiteaccommodation: '',
        beginDate: '',
        endDate: '',
        description: '',
        isAvailable: true,
        ownerId: null,
      },
      files: []
    };
  },
  methods: {
    handleFileUpload(event) {
      this.files = Array.from(event.target.files);
    },
    async createCampingSpot() {
      const formData = new FormData();
      formData.append('title', this.campingSpot.title);
      formData.append('country', this.campingSpot.country);
      formData.append('location', this.campingSpot.location);
      formData.append('theme', this.campingSpot.theme);
      formData.append('howmanycampers', this.campingSpot.howmanycampers);
      formData.append('onsiteaccommodation', this.campingSpot.onsiteaccommodation);
      formData.append('beginDate', this.campingSpot.beginDate);
      formData.append('endDate', this.campingSpot.endDate);
      formData.append('description', this.campingSpot.description);
      formData.append('isAvailable', this.campingSpot.isAvailable);
      formData.append('ownerId', this.campingSpot.ownerId);

      this.files.forEach(file => {
        formData.append('images', file);
      });

      try {
        const response = await axios.post('http://localhost:5000/api/CampingSpots', formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        });
        console.log(response.data);
        this.$router.push('booking-List');
      } catch (error) {
        console.error(error);
      }
    }
  },
  created() {
    const user = JSON.parse(localStorage.getItem('user'));
    this.campingSpot.ownerId = user ? user.ownerId : null;
  }
};
</script>

<style scoped>
body {
  margin: 0;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  background: #f0f0f0;
}

#booking-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 60px;
  width: 100vw;
}

.booking-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
}

.booking-card h1 {
  margin-bottom: 20px;
  font-size: 24px;
}

.form-inputs {
  margin-bottom: 15px;
  text-align: left;
}

.form-inputs label {
  display: block;
  margin-bottom: 5px;
}

.form-inputs input, .form-inputs select, .form-inputs textarea {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.booking-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.booking-button:hover {
  background: darkgreen;
}
</style>
