//Type definition for the user object
export interface User {
	id: number;
	name: string;
	surname: string;
	address?: string;
	birthDate: string;
	startDate: string;
	position: number; // defines user position based on DB value
	salary: number;
}
