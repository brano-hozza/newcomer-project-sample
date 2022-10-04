//Type definition for the user object
export interface User {
  name: string;
  surname: string;
  address?: string;
  birth_date: string;
  start_date: string;
  position: number; // defines user position based on DB value
  salary: number;
}
