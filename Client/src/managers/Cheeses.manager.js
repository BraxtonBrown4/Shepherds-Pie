const apiUrl = "/api/cheeses";

export const getAllCheeses = () => {
  return fetch(apiUrl).then((res) => res.json());
};