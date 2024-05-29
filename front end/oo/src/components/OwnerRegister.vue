<template>
  <div id="login-container">
    <div class="login-card">
      <h1>Owner Register</h1>
      <form @submit.prevent="register">
        <div class="form-inputs">
          <label for="username">Username</label>
          <input type="text" id="username" v-model="user.username" required>
        </div>
        <div class="form-inputs">
          <label for="email">Email</label>
          <input type="email" id="email" v-model="user.email" required>
        </div>
        <div class="form-inputs">
          <label for="password">Password</label>
          <input type="password" id="password" v-model="user.password" required>
        </div>
        <button type="submit" class="login-button">Register</button>
      </form>

      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
      <p v-if="successMessage" class="success-message">{{ successMessage }}</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      user: {
        username: '',
        email: '',
        password: ''
      },
      errorMessage: '',
      successMessage: ''
    };
  },
  methods: {
    async register() {
      try {
        const response = await axios.post('http://localhost:5000/api/Owners/register', this.user);

        if (response.status === 200) {
          this.successMessage = 'Owner created successfully';
          this.errorMessage = '';
          this.$router.push('/registration-success2');
        }
      } catch (error) {
        if (error.response && error.response.status === 400) {
          this.errorMessage = 'Email already exists';
          this.successMessage = '';
        } else {
          this.errorMessage = 'An error occurred. Please try again later.';
          this.successMessage = '';
        }
      }
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

.form-inputs input {
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

.error-message {
  color: red;
  margin-top: 10px;
}

.success-message {
  color: green;
  margin-top: 10px;
}
</style>
