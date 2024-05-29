<template>
  <div id="login-container">
    <div id="owner-login" class="login-card">
      <h1>Owner Login</h1>
      <div class="form-inputs">
        <label for="username">Username</label>
        <input type="text" id="username" name="username" v-model="input.username" placeholder="Username" />
      </div>
      <div class="form-inputs">
        <label for="password">Password</label>
        <input type="password" id="password" name="password" v-model="input.password" placeholder="Password" />
      </div>
      <button type="button" @click="login" class="login-button">Login</button>

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
      input: {
        username: '',
        password: ''
      },
      errorMessage: '',
      successMessage: ''
    };
  },
  methods: {
    async login() {
      this.errorMessage = '';
      this.successMessage = '';

      if (this.input.username && this.input.password) {
        try {
          const response = await axios.post(`http://localhost:5000/api/Owners/login`, {
            username: this.input.username,
            password: this.input.password
          });

          localStorage.setItem('user', JSON.stringify(response.data));
          localStorage.setItem('isOwner', true);
          this.$emit('authenticated', true);
          this.$emit('isOwner', true);

          this.successMessage = 'Login successful';
          this.$router.replace({ name: 'Sownerlogin' });

        } catch (error) {
          this.errorMessage = 'The username and/or password is wrong!';
        }
      } else {
        this.errorMessage = 'A username and password must be present';
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

#owner-login {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
}

#owner-login h1 {
  margin-bottom: 20px;
  font-size: 24px;
}

#owner-login .form-inputs {
  margin-bottom: 15px;
  text-align: left;
}

#owner-login .form-inputs label {
  display: block;
  margin-bottom: 5px;
}

#owner-login .form-inputs input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}

#owner-login .login-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

#owner-login .login-button:hover {
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
