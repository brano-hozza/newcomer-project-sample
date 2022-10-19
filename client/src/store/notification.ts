import { defineStore } from 'pinia';

import { INotification } from '@/types';


type TNotificationState = {
	notifications: INotification[];
	counter : number;
};

export const useNotificationStore = defineStore('notifications', {
	state: (): TNotificationState => ({
		notifications: [],
		counter: 0
	}),
	actions: {
		addNotification(title: string, type: 'success' | 'error', message?: string) {
			const id = this.counter++;
			const notification: INotification = {
				id,
				title,
				message,
				type
			};
			this.notifications.push(notification);
			setTimeout(() => {
				this.removeNotification(id);
			}, 3000);
		},
		removeNotification(id: number) {
			this.notifications = this.notifications.filter(notification => notification.id !== id);
		}
		
	} 
});
