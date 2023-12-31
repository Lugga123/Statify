import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import ArtistView from '../views/ArtistView.vue'
import SongView from '../views/SongView.vue'
import MyAccountView from '../views/MyAccountView.vue'
import AboutView from '../views/AboutView.vue'
import OtherUsersView from '../views/OtherUsersView.vue'
import UserProfile from '../views/UserProfile.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      
    },

    {
      path: '/artist',
      name: 'artist',
      component: ArtistView,
    },

    {
      path: '/song',
      name: 'song',
      component: SongView,
    },
    {
      path: '/my-account',
      name: 'my-account',
      component: MyAccountView,
    },
    {
      path: '/about',
      name: 'about',
      component: AboutView,
    },
    {
      path: '/other-users',
      name: 'other-users',
      component: OtherUsersView,
    
    },
    {
      path: '/other-users-profil',
      name: '/other-users-profil',
      component: UserProfile,
    
    },
    

  ]
});



export default router;
