<template>
  <div>
    <h1 class="text-center">Manage Bookings</h1>
    <div class="card-container" v-if="paginatedOwnerBookings.length">
      <div v-for="booking in paginatedOwnerBookings" :key="booking.id" class="card">
        <h2>{{ booking.campingSpot.title }}</h2>
        <div class="details">
          <p><strong>Username:</strong> {{ booking.userName }}</p>
          <p><strong>Email:</strong> {{ booking.email }}</p>
          <p><strong>Begin Date:</strong> {{ booking.beginDate }}</p>
          <p><strong>End Date:</strong> {{ booking.endDate }}</p>
          <p><strong>Message:</strong> {{ booking.message }}</p>
          <p><strong>Status:</strong> {{ booking.statusText }}</p>
        </div>
        <div class="actions">
          <router-link :to="{ name: 'ownercamprof', params: { id: booking.campingSpotId } }">
            <button class="action-button">Profile</button>
          </router-link>
          <button @click="approveBooking(booking.id)" class="action-button">Approve</button>
          <button @click="denyBooking(booking.id)" class="action-button">Deny</button>
          <button @click="deleteBooking(booking.id)" class="action-button">Delete</button>
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
  name: 'OwnerBookings',
  data() {
    return {
      ownerBookings: [],
      currentPage: 1,
      itemsPerPage: 6
    };
  },
  computed: {
    paginatedOwnerBookings() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.ownerBookingsWithStatusText.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.ownerBookingsWithStatusText.length / this.itemsPerPage);
    },
    ownerBookingsWithStatusText() {
      return this.ownerBookings.map(booking => {
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
    async fetchOwnerBookings() {
      const user = JSON.parse(localStorage.getItem('user'));
      const ownerId = user ? user.ownerId : null;

      if (!ownerId) {
        console.error('No owner ID found in local storage.');
        return;
      }

      try {
        const response = await axios.get(`http://localhost:5000/api/ownerbookings/${ownerId}`);
        this.ownerBookings = response.data;
      } catch (error) {
        console.error('Error fetching owner bookings:', error);
      }
    },
    async approveBooking(id) {
      try {
        await axios.put(`http://localhost:5000/api/ownerbookings/${id}/approve`);
        this.fetchOwnerBookings();
      } catch (error) {
        console.error('Error approving booking:', error);
      }
    },
    async denyBooking(id) {
      try {
        await axios.put(`http://localhost:5000/api/ownerbookings/${id}/deny`);
        this.fetchOwnerBookings();
      } catch (error) {
        console.error('Error denying booking:', error);
      }
    },
    async deleteBooking(id) {
      try {
        await axios.delete(`http://localhost:5000/api/ownerbookings/${id}`);
        this.fetchOwnerBookings();
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
    this.fetchOwnerBookings();
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
