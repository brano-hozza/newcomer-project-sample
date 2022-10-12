export const apiCall = (
	url: string,
	options?: RequestInit
): Promise<Response> => fetch(`${import.meta.env.VITE_API_URL}${url}`, options);
