import type { RouteRecordRaw } from 'vue-router';
export const routes: RouteRecordRaw[] = [
	{
		path: '/',
		component: () => import('@/views/Home.vue'),
		children: [
			{
				path: '/',
				name: 'Aktuálni zamestnanci',
				component: () => import('@/views/CurrentEmployees.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/old',
				name: 'Bývalí zamestnanci',
				component: () => import('@/views/OldEmployees.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/positions',
				name: 'Pozície',
				component: () => import('@/views/Positions.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/details/:id',
				name: 'Details',
				component: () => import('@/views/EmployeeDetail.vue')
			},
			{
				path: '/details/edit/:id',
				name: 'Details edit',
				component: () => import('@/views/EmployeeDetail.vue'),
				meta: {
					edit: true
				}
			},
			{
				path: '/details/new',
				name: 'New Employee',
				component: () => import('@/views/EmployeeDetail.vue'),
				meta: {
					new: true
				}
			}
		]
	}
];
