<template>
  <div id="login-container">
    <div class="login-card">
      <h1>Owner Authentication</h1>
      <router-link to="/owner/login" class="login-button">Owner Login</router-link>
      <router-link to="/owner/register" class="login-button">Owner Register</router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: 'OwnerAuth',
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

.login-button {
  width: 100%;
  padding: 10px;
  background: green;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  display: block;
  margin-top: 10px;
  text-align: center;
  text-decoration: none;
}

.login-button:hover {
  background: darkgreen;
}
</style>
