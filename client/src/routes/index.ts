import type { RouteRecordRaw } from 'vue-router';
export const routes: RouteRecordRaw[] = [
	{
		path: '/',
		component: () => import('../views/Home.vue'),
		children: [
			{
				path: '/',
				name: 'Current employees',
				component: () => import('../views/CurrentEmployees.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/old',
				name: 'Old employees',
				component: () => import('../views/OldEmployees.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/positions',
				name: 'Positions',
				component: () => import('../views/Positions.vue'),
				meta: {
					nav: true
				}
			},
			{
				path: '/details/:id',
				name: 'Details',
				component: () => import('../views/EmployeeDetail.vue')
			},
			{
				path: '/details/edit/:id',
				name: 'Details edit',
				component: () => import('../views/EmployeeDetail.vue'),
				meta: {
					edit: true
				}
			}
		]
	}
];
