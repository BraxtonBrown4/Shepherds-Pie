const apiUrl = "/api/sauces";

export const getAllSauces = () => {
  return fetch(apiUrl).then((res) => res.json());
};