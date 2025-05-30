const apiUrl = "/api/orders";

export const getAllOrders = () => {
  return fetch(apiUrl).then((res) => res.json());
};