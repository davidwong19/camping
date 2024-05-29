import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from './components/Home.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    component: () => import('./components/About.vue')
  },
  {
    path: '/secure',
    name: 'Secure',
    component: () => import( './components/Userchange.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/user-bookings',
    name: 'UserBookings',
    component: () => import('./components/UserBookings.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/owner/login',
    name: 'OwnerLogin',
    component: () => import('./components/OwnerLogin.vue')
  },
  {
    path: '/owner/register',
    name: 'OwnerRegister',
    component: () => import('./components/OwnerRegister.vue')
  },
  {
    path: '/user/login',
    name: 'UserLogin',
    component: () => import('./components/UserLogin.vue')
  },
  {
    path: '/user/register',
    name: 'UserRegister',
    component: () => import('./components/UserRegister.vue')
  },
  {
    path: '/owner-auth',
    name: 'OwnerAuth',
    component: () => import('./components/OwnerAuth.vue')
  },
  {
    path: '/user-auth',
    name: 'UserAuth',
    component: () => import('./components/UserAuth.vue')
  },
  {
    path: '/booking-List',
    name: 'bookingList',
    component: () => import('./components/bookingList.vue')
  },
  {
    path: '/registration-success',
    name: 'RegistrationSuccess',
    component: () => import('./components/RegistrationSuccess.vue')
  },
  {
    path: '/registration-success2',
    name: 'RegistrationSuccess2',
    component: () => import('./components/RegistrationSuccess2.vue')
  },
  {
    path: '/Camping-Spots',
    name: 'CampingSpots',
    component: () => import('./components/CampingSpots.vue')
  },
  {
    path: '/Owner-Bookings',
    name: 'OwnerBookings',
    component: () => import('./components/OwnerBookings.vue')
  },
  {
    path: '/User-change',
    name: 'Userchange',
    component: () => import('./components/Userchange.vue')
  },
  {
    path: '/userover-viewbooking',
    name: 'useroverviewbooking',
    component: () => import('./components/useroverviewbooking.vue')
  },
  {
    path: '/CampingSpot-Profile',
    name: 'CampingSpotProfile',
    component: () => import('./components/CampingSpotProfile.vue')
  },

{
  path: '/book-forum',
  name: 'bookforum',
  component: () => import('./components/bookforum.vue')
},
{
  path: '/booking-Listedit',
  name: 'bookingListedit',
  component: () => import('./components/bookingListedit.vue')
},
{
  path: '/Suser-login',
  name: 'Suserlogin',
  component: () => import('./components/succes/Suserlogin.vue')
},
{
  path: '/Sowner-login',
  name: 'Sownerlogin',
  component: () => import('./components/succes/Sownerlogin.vue')
},
{
  path: '/S-logout',
  name: 'Slogout',
  component: () => import('./components/succes/Slogout.vue')
},
{
  path: '/userchange-succes',
  name: 'userchangesucces',
  component: () => import('./components/succes/userchangesucces.vue')
},
{
  path: '/owner-camprof',
  name: 'ownercamprof',
  component: () => import('./components/ownercamprof.vue')
}

];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
});

router.beforeEach((to, from, next) => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!user) {
      next({
        path: '/login',
        query: { redirect: to.fullPath }
      });
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
