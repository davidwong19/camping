<template>
  <div id="login-container">
    <div class="login-card">
      <h1>Book Camping Spot</h1>
      <form @submit.prevent="sendBookingRequest">
        <div class="form-inputs">
          <label for="name">Name:</label>
          <input type="text" id="name" v-model="bookingForm.name" required />
        </div>
        <div class="form-inputs">
          <label for="email">Email:</label>
          <input type="email" id="email" v-model="bookingForm.email" required />
        </div>
        <div class="form-inputs">
          <label for="message">Message:</label>
          <textarea id="message" v-model="bookingForm.message" required></textarea>
        </div>
        <div class="form-inputs">
          <label for="beginDate">Begin Date:</label>
          <input type="date" id="beginDate" v-model="bookingForm.beginDate" required />
        </div>
        <div class="form-inputs">
          <label for="endDate">End Date:</label>
          <input type="date" id="endDate" v-model="bookingForm.endDate" required />
        </div>
        <button type="submit" class="login-button">Send Booking Request</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'BookForum',
  data() {
    return {
      bookingForm: {
        name: '',
        email: '',
        message: '',
        beginDate: '',
        endDate: ''
      },
      spotId: null
    };
  },
  methods: {
    async sendBookingRequest() {
      try {
        if (!this.spotId) {
          console.error('No spot ID found');
          alert('Invalid camping spot ID.');
          return;
        }

        const user = JSON.parse(localStorage.getItem('user'));

        if (!user || !user.userId) {
          console.error('No valid user found in local storage');
          alert('You must be logged in to book a camping spot.');
          return;
        }

        const payload = {
          userId: user.userId,
          userName: this.bookingForm.name,
          email: this.bookingForm.email,
          message: this.bookingForm.message,
          beginDate: this.bookingForm.beginDate,
          endDate: this.bookingForm.endDate,
          status: 0,
          campingSpotId: this.spotId
        };

        const response = await axios.post(`http://localhost:5000/api/campingspots/${this.spotId}/book`, payload);
        console.log('Booking successful:', response.data);
        this.$router.push('/userover-viewbooking');
      } catch (error) {
        console.error('Error sending booking request:', error);
        this.$router.push('/userover-viewbooking');
      }
    }
  },
  created() {
    this.spotId = this.$route.params.spotId;
    if (!this.spotId) {
      console.error('No spot ID provided in route params');
      alert('Invalid camping spot ID.');
    }
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

#login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 60px;
  width: 100vw;
}

.login-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
}

.login-card h1 {
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

.form-inputs input, .form-inputs textarea {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.login-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.login-button:hover {
  background: darkgreen;
}
</style>
