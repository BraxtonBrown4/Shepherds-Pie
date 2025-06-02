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

export const deleteOrder = (id) => {
  return fetch(`${apiUrl}/${id}`, {
    method: "DELETE",
  }).then((res) => {
    if (!res.ok) {
      throw new Error("Failed to delete order");
    }
    return res.status === 204 ? null : res.json();
  });
};
