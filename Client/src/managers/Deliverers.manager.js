const apiUrl = "/api/deliverers";

export const getAllDeliverers = () => {
  return fetch(apiUrl).then((res) => res.json());
};