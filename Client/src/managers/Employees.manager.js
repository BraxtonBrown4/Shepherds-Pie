const apiUrl = "/api/employees";

export const getAllEmployees = () => {
  return fetch(apiUrl).then((res) => res.json());
};