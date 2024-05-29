<template>
  <div id="app">
    <div id="nav">
      <nav class="stroke">
        <ul>
          <li><router-link to="/">Home</router-link></li>
          <li><router-link to="/about">About</router-link></li>
          <li v-if="authenticated && !isOwner"><router-link to="/userover-viewbooking">User Overview Booking</router-link></li>
          <li v-if="authenticated && !isOwner"><router-link to="/User-change">User Change</router-link></li>
          <li v-if="authenticated && !isOwner"><router-link to="/Camping-Spots">Camping Spots</router-link></li>
          <li v-if="authenticated && isOwner"><router-link to="/user-bookings">create camping spot</router-link></li>
          <li v-if="authenticated && isOwner"><router-link to="/Owner-Bookings">Owner Bookings</router-link></li>
          <li v-if="authenticated && isOwner"><router-link to="/booking-List">Your camping Spots</router-link></li>
          <li v-if="authenticated">
            <button class="logout-button" @click="logout">Logout</button>
          </li>
          <li v-if="!authenticated"><router-link to="/owner-auth">Owner</router-link></li>
          <li v-if="!authenticated"><router-link to="/user-auth">User</router-link></li>
        </ul>
      </nav>
    </div>
    <router-view @authenticated="setAuthenticated" @isOwner="setIsOwner" />
  </div>
</template>

<script>
export default {
  data() {
    return {
      authenticated: false,
      isOwner: false,
    };
  },
  methods: {
    setAuthenticated(status) {
      this.authenticated = status;
    },
    setIsOwner(status) {
      this.isOwner = status;
      localStorage.setItem('isOwner', status);
    },
    logout() {
      this.authenticated = false;
      this.isOwner = false;
      localStorage.removeItem('user');
      localStorage.removeItem('isOwner');
      this.$router.replace({ name: 'Slogout' });
    }
  },
  mounted() {
    const user = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.setAuthenticated(true);
      this.setIsOwner(user.role === 'Owner');
    } else {
      const isOwner = JSON.parse(localStorage.getItem('isOwner'));
      if (isOwner !== null) {
        this.setIsOwner(isOwner);
        this.setAuthenticated(true);
      }
    }
  }
};
</script>

<style>
@import url('https://fonts.googleapis.com/css?family=Roboto:500,900,100,300,700,400');

body {
  font-family: 'Roboto', sans-serif;
  padding: 0;
  margin: 0;
}

#app {
  text-align: center;
}

#nav {
  padding: 30px;
}

nav {
  width: 80%;
  margin: 0 auto;
  background: #ffffff;
  padding: 20px 0;
  box-shadow: 0px 5px 0px #dedede;
  background-image: url('https://naturparkschwarzwald.blog/wp-content/uploads/2019/02/Camp-Erdbeerloch.jpg');
  background-size: cover;
  background-position: center;
}

nav ul {
  list-style: none;
  text-align: center;
}

nav ul li {
  display: inline-block;
}

nav ul li a {
  display: block;
  padding: 15px;
  text-decoration: none;
  color: #f1f1f1; 
  text-transform: uppercase;
  margin: 0 10px;
}

nav ul li a,
nav ul li a:after,
nav ul li a:before {
  transition: all .5s;
}

nav ul li a:hover {
  color: #ddd; 
}


nav.stroke ul li a {
  position: relative;
}

nav.stroke ul li a:after {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  width: 0%;
  content: '.';
  color: transparent;
  background: #ddd;
  height: 1px;
}

nav.stroke ul li a:hover:after {
  width: 100%;
}

.logout-button {
  background-color: green;
  color: white;
  border: none;
  padding: 10px 20px;
  margin: 0 5px;
  border-radius: 5px;
  cursor: pointer;
}

.logout-button:hover {
  background-color: #1383099a;
}

.logout-button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
</style>
