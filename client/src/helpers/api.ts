/**
 * This method automaticly inlcudes API url before call
 * @param {string} url API endpoint
 * @param {RequestInit?} options fetch options
 * @returns {Promise} fetch response
 */
export const apiCall = (
	url: string,
	options?: RequestInit
): Promise<Response> => fetch(`${import.meta.env.VITE_API_URL}${url}`, options);
