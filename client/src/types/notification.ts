
export interface INotification{
	id: number;
	title: string;
	message?: string;
	type: 'success' | 'error';
};
