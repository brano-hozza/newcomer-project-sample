import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './styles/index.scss';

import { createPinia } from 'pinia';
import { createApp } from 'vue';
import { createRouter, createWebHistory } from 'vue-router';

import App from './App.vue';
import { routes } from './routes';
createApp(App)
	.use(createPinia())
	.use(
		createRouter({
			routes: routes,
			history: createWebHistory()
		})
	)
	.mount('#app');
