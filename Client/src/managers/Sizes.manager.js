const apiUrl = "/api/sizes";

export const getAllSizes = () => {
  return fetch(apiUrl).then((res) => res.json());
};