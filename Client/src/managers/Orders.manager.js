const apiUrl = "/api/orders";

export const getAllOrders = () => {
  return fetch(apiUrl).then((res) => res.json());
};
export const CreateOrder = async(order) => {
  const res = await fetch(apiUrl, {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(order)
})
 return res 
};