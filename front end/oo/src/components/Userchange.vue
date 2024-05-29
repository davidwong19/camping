<template>
  <div id="update-container">
    <div class="update-card">
      <h2>Update User Information</h2>
      <form @submit.prevent="updateUser">
        <div class="form-inputs">
          <label for="username">Username:</label>
          <input v-model="user.username" id="username" type="text" required />
        </div>
        <div class="form-inputs">
          <label for="email">Email:</label>
          <input v-model="user.email" id="email" type="email" required />
        </div>
        <div class="form-inputs">
          <label for="password">Password:</label>
          <input v-model="user.password" id="password" type="password" required />
        </div>
        <button type="submit" class="update-button">Update</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      user: {
        id: null,
        username: '',
        email: '',
        password: '',
        userId: null
      }
    };
  },
  methods: {
    async fetchUser() {
      try {
        const user = JSON.parse(localStorage.getItem('user'));
        const Id = user ? user.userId : null;
        const response = await axios.get(`http://localhost:5000/api/Users/${Id}`);
        this.user = response.data;
      } catch (error) {
        console.error('Error fetching user:', error.message);
        alert('Error fetching user: ' + error.message);
      }
    },
    async updateUser() {
      try {
        const user = JSON.parse(localStorage.getItem('user'));
        const Id = user ? user.userId : null;
        await axios.put(`http://localhost:5000/api/Users/${Id}`, this.user);
        this.$router.push('/userchange-succes');
      } catch (error) {
        console.error('Error updating user:', error.message);
        alert('Error updating user: ' + error.message);
      }
    }
  }, 
  mounted() {
    this.fetchUser();
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

#update-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 60px;
  width: 100vw;
}

.update-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
}

.update-card h2 {
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

.form-inputs input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.update-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.update-button:hover {
  background: darkgreen;
}
</style>
