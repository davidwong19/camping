<template>
  <div>
    <h1 class="text-center">Your Bookings</h1>
    <div class="card-container" v-if="paginatedBookings.length">
      <div v-for="booking in paginatedBookings" :key="booking.id" class="card">
        <h2>{{ booking.campingSpotTitle }}</h2>
        <div class="details">
          <p><strong>Status:</strong> {{ booking.statusText }}</p>
        </div>
        <div class="actions">
          <router-link :to="{ name: 'CampingSpotProfile', params: { id: booking.campingSpotId } }">
            <button class="action-button">Profile</button>
          </router-link>
          <button @click="deleteBooking(booking.id)" class="action-button">Delete Booking</button>
        </div>
      </div>
    </div>
    <div v-else>
      <p>No bookings found.</p>
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
  data() {
    return {
      bookings: [],
      currentPage: 1,
      itemsPerPage: 6
    };
  },
  computed: {
    paginatedBookings() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.bookingsWithStatusText.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.bookingsWithStatusText.length / this.itemsPerPage);
    },
    bookingsWithStatusText() {
      return this.bookings.map(booking => {
        let statusText;
        switch (booking.status) {
          case 0:
            statusText = 'Pending';
            break;
          case 1:
            statusText = 'Approved';
            break;
          case 2:
            statusText = 'Denied';
            break;
          default:
            statusText = 'Unknown';
        }
        return {
          ...booking,
          statusText
        };
      });
    }
  },
  methods: {
    async fetchBookings() {
      try {
        const user = JSON.parse(localStorage.getItem('user'));
        const userId = user ? user.userId : null;
        const response = await axios.get(`http://localhost:5000/api/Users/${userId}/bookings`);
        this.bookings = response.data;
      } catch (error) {
        console.error('Error fetching bookings:', error);
      }
    },
    async deleteBooking(bookingId) {
      try {
        await axios.delete(`http://localhost:5000/api/Bookings/${bookingId}`);
        this.bookings = this.bookings.filter(booking => booking.id !== bookingId);
      } catch (error) {
        console.error('Error deleting booking:', error);
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
    this.fetchBookings();
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

.details {
  padding: 10px 0;
}

.actions {
  display: flex;
  justify-content: space-between;
}

.action-button {
  background-color: green;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

.action-button:hover {
  background-color: #1383099a;
}

.text-center {
  text-align: center;
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
