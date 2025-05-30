const apiUrl = "/api/toppings";

export const getAllToppings = () => {
  return fetch(apiUrl).then((res) => res.json());
};