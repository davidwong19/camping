<template>
  <div id="edit-container">
    <div class="edit-card">
      <h1>Edit Camping Spot</h1>
      <form @submit.prevent="updateSpot" enctype="multipart/form-data">
        <div class="form-inputs">
          <label for="title">Title</label>
          <input v-model="currentSpot.title" id="title" required />
        </div>
        <div class="form-inputs">
          <label for="country">Country</label>
          <input v-model="currentSpot.country" id="country" required />
        </div>
        <div class="form-inputs">
          <label for="location">Location</label>
          <input v-model="currentSpot.location" id="location" required />
        </div>
        <div class="form-inputs">
          <label for="theme">Theme</label>
          <input v-model="currentSpot.theme" id="theme" required />
        </div>
        <div class="form-inputs">
          <label for="howmanycampers">How Many Campers</label>
          <input v-model="currentSpot.howmanycampers" id="howmanycampers" type="number" required />
        </div>
        <div class="form-inputs">
          <label for="onsiteaccommodation">Onsite Accommodation</label>
          <input v-model="currentSpot.onsiteaccommodation" id="onsiteaccommodation" required />
        </div>
        <div class="form-inputs">
          <label for="beginDate">Begin Date</label>
          <input v-model="currentSpot.beginDate" type="date" id="beginDate" required />
        </div>
        <div class="form-inputs">
          <label for="endDate">End Date</label>
          <input v-model="currentSpot.endDate" type="date" id="endDate" required />
        </div>
        <div class="form-inputs">
          <label for="description">Description</label>
          <textarea v-model="currentSpot.description" id="description" required></textarea>
        </div>
        <div class="form-inputs">
          <label for="isAvailable">Is Available</label>
          <input v-model="currentSpot.isAvailable" id="isAvailable" type="checkbox" />
        </div>
        <div class="form-inputs">
          <label for="images">Images</label>
          <input type="file" id="images" multiple @change="onFileChange" />
        </div>
        <button type="submit" class="edit-button">Save</button>
        <button type="button" @click="cancelEdit" class="cancel-button">Cancel</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'bookingListedit',
  data() {
    return {
      currentSpot: {
        id: null,
        title: '',
        country: '',
        location: '',
        theme: '',
        howmanycampers: 0,
        onsiteaccommodation: '',
        beginDate: '',
        endDate: '',
        description: '',
        isAvailable: false,
        images: [],
        ownerId: null
      },
      newImages: []
    };
  },
  methods: {
    async updateSpot() {
      try {
        const formData = new FormData();
        Object.keys(this.currentSpot).forEach(key => {
          if (key !== 'images') {
            formData.append(key, this.currentSpot[key]);
          }
        });

        this.newImages.forEach(image => {
          formData.append('images', image);
        });

        await axios.put(`http://localhost:5000/api/campingspots/${this.currentSpot.id}`, formData);
        this.$router.push('/booking-List');
      } catch (error) {
        console.error('Error updating spot:', error.response ? error.response.data : error.message);
      }
    },
    onFileChange(event) {
      this.newImages = Array.from(event.target.files);
    },
    cancelEdit() {
      this.$router.push('/booking-List');
    },
    async fetchCampingSpot() {
      const id = this.$route.params.id;
      try {
        const response = await axios.get(`http://localhost:5000/api/campingspots/${id}`);
        this.currentSpot = response.data;
      } catch (error) {
        console.error('Error fetching camping spot:', error.response ? error.response.data : error.message);
      }
    }
  },
  mounted() {
    this.fetchCampingSpot();
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

#edit-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 60px;
  width: 100vw;
}

.edit-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
}

.edit-card h1 {
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

.form-inputs input,
.form-inputs select,
.form-inputs textarea {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.edit-button, .cancel-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  margin-top: 10px;
}

.edit-button:hover, .cancel-button:hover {
  background: darkgreen;
}

.cancel-button {
  background: green;
}

.cancel-button:hover {
  background: darkgreen;
}
</style>
