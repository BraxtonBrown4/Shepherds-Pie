import { useCallback, useEffect, useState } from "react";
import { CreateOrder, getAllOrders, deleteOrder } from "../managers/Orders.manager";
import { getAllToppings } from "../managers/Toppings.manager";
import { getAllCheeses } from "../managers/Cheeses.manager";
import { getAllSauces } from "../managers/Sauces.manager";
import { getAllSizes } from "../managers/Sizes.manager";
import { getAllDeliverers } from "../managers/Deliverers.manager";
import { getAllEmployees } from "../managers/Employees.manager";
import { X, Pizza } from "lucide-react";
import "./Orders.css";

export const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [toppings, setToppings] = useState([]);
  const [cheeses, setCheeses] = useState([]);
  const [sauces, setSauces] = useState([]);
  const [sizes, setSizes] = useState([]);
  const [deliverers, setDeliverers] = useState([]);

  const [employees, setEmployees] = useState([]);
  const [employeeId, setEmployeeId] = useState(0);
  const [delivererId, setDelivererId] = useState(null);
  const [tableNumber, setTableNumber] = useState(null);
  const [tip, setTip] = useState(0);

  const [newOrderModal, setNewOrderModal] = useState(false);
  const [editOrderById, setEditOrderById] = useState(false)
  const [pizzas, setPizzas] = useState([]);
  const [TotalCost, setTotalCost] = useState(0);
  const [toppingsPrice, settoppingsPrice] = useState([]);
  const [pizzaPrice, setPizzaPrice] = useState([])
  const [updatedOrder, setUpdatedOrder] = useState({})

  const resetForm = () => {
    setNewOrderModal(false);
    setEmployeeId(0);
    setDelivererId(null);
    setTableNumber(null);
    setTip(0);
    setPizzas([]);
    setPizzaPrice([]);
    settoppingsPrice([]);
  };

  const updatePizza = useCallback((index, newData) => {
    const updated = [...pizzas];
    updated[index] = { ...updated[index], ...newData };
    setPizzas(updated);
  });

  useEffect(() => {
    if (toppingsPrice?.length < 1) return
    if (pizzaPrice?.length < 1) return
    let toppingsprice = toppingsPrice.reduce((total, number) => total + number, 0)
    const basePrices = pizzaPrice.map(p =>
      parseFloat(p.match(/(?<=\()\d+(?="?\))/)[0])
    )
    const totalBasePrice = basePrices.reduce((sum, price) => sum + price, 0);
    let totalCost = parseInt(totalBasePrice) + parseFloat(toppingsprice) + parseInt(tip)
    if (tableNumber === null) {
      totalCost += 5
    }
    setTotalCost(totalCost)
  }, [pizzaPrice, tableNumber, toppingsPrice, tip])

  const handleToppingChange = (pizzaIndex, toppingId, checked) => {
    const current = pizzas[pizzaIndex];
    const currentToppings = current.toppings || [];
    const updatedToppings = checked
      ? [...currentToppings, toppingId]
      : currentToppings.filter((id) => id !== toppingId);
    const fullToppings = toppings.filter(t => updatedToppings.includes(t.id));
    updatePizza(pizzaIndex, { toppings: fullToppings });
  };

  const cancelPizza = (index) => {
    const updated = pizzas.filter((_, i) => i !== index);
    setPizzas(updated);
  };

  const createOrder = async () => {
    const order = {
      employeeId: employeeId,
      delivererId: delivererId,
      tableNumber: tableNumber,
      tip: tip,
      Pizza: pizzas,
      totalCost: TotalCost
    }
    await CreateOrder(order)
    await getAllOrders().then(setOrders)
    setNewOrderModal(false)
    resetForm()
  }

  useEffect(() => {
    getAllOrders().then(setOrders);
    getAllToppings().then(setToppings);
    getAllCheeses().then(setCheeses);
    getAllSauces().then(setSauces);
    getAllSizes().then(setSizes);
    getAllDeliverers().then(setDeliverers);
    getAllEmployees().then(setEmployees);
  }, []);

  return (
    <div className="orders-container">
      <h2 className="orders-title">Orders</h2>
      <button
        onClick={() => {
          setNewOrderModal(true);
          setEmployeeId(Math.floor(Math.random() * employees.length) + 1);
        }}
      >
        New Order
        <Pizza />
      </button>
      {orders.map((o) => (
        <div className="order-card" key={o.id}>
          <div className="order-grid">
            <div className="order-info">
              <h3>Order #{o.id}</h3>
              <button onClick={() => {
                deleteOrder(o.id).then(() => {
                  getAllOrders().then(setOrders);
                });
              }}>Delete</button>
              <button onClick={() => { setEditOrderById(o.id); setUpdatedOrder(orders.find(oo => oo.id == o.id)) }}>Edit</button>
              <p><strong>Employee ID:</strong> {o.employeeId}</p>
              <p><strong>Deliverer ID:</strong> {o.delivererId || "None"}</p>
              <p><strong>Table Number:</strong> {o.tableNumber || "None"}</p>
              <p><strong>Tip:</strong> ${o.tip}</p>
              <p><strong>Order Time:</strong> {new Date(o.orderTime).toLocaleDateString()} {new Date(o.orderTime).toLocaleTimeString()}</p>
              <p><strong>Total Cost:</strong> ${o.totalCost}</p>
            </div>

            <div className="pizza-list">
              {o.pizza.map((p) => (
                <div className="pizza-card" key={p.id}>
                  <h4>Pizza #{p.id}</h4>
                  <p><strong>Sauce ID:</strong> {p.sauceId}</p>
                  <p><strong>Size ID:</strong> {p.SizeId}</p>
                  <p><strong>Cheese ID:</strong> {p.cheeseId}</p>
                  <p><strong>Toppings:</strong></p>
                  <ul className="toppings-list">
                    {p.toppings.map((t) => (
                      <li key={t.id}>{t.topping.name}</li>
                    ))}
                  </ul>
                  <p><strong>Price:</strong> ${p.price}</p>
                </div>
              ))}
            </div>
          </div>
        </div>
      ))}

      {newOrderModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button onClick={() => setNewOrderModal(false)}><X /></button>
            <h3>New Order</h3>

            <div className="order-type-row">
              <span>
                Delivery{" "}
                <input
                  type="checkbox"
                  checked={typeof delivererId === "number"}
                  onChange={() => {
                    setDelivererId(Math.floor(Math.random() * deliverers.length) + 1);
                    setTableNumber(null);
                  }}
                />
              </span>
              <span>
                Dine In{" "}
                <input type="checkbox" checked={typeof tableNumber === "number"} onChange={() => {
                  setTableNumber(Math.floor(Math.random() * 15) + 1); setDelivererId(null)
                }} />
              </span>
            </div>
            <button onClick={() => setPizzas([...pizzas, {}])}>+ Add Pizza</button>
            {pizzas.map((pizza, index) => (
              <div key={index} className="pizza-section">
                <h4>Pizza {index + 1}</h4>
                <button onClick={() => cancelPizza(index)}>Cancel Pizza</button>

                <div className="flex-direction-row">
                  {sizes.map((s) => (
                    <div key={s.id}>
                      <input type="radio" name={`size-${index}`} onChange={() => { updatePizza(index, { sizeId: s.id }), setPizzaPrice(prev => [...prev, s.name]) }} />{s.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {cheeses.map((c) => (
                    <div key={c.id}>
                      <input type="radio" name={`cheese-${index}`} onChange={() => updatePizza(index, { cheeseId: c.id })} />
                      {c.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {sauces.map((s) => (
                    <div key={s.id}>
                      <input type="radio" name={`sauce-${index}`} onChange={() => updatePizza(index, { sauceId: s.id })} />{s.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {toppings.map((t) => (
                    <div key={t.id}>
                      <input type="checkbox" onChange={(e) => handleToppingChange(index, t.id, e.target.checked, settoppingsPrice(prev => [...prev, t.price]))} />
                      {t.name}
                    </div>
                  ))}
                </div>
              </div>
            ))}
            <div className="tip-input">
              <span>
                Tip $<input type="number" onChange={(e) => setTip(parseInt(e.target.value))} />
              </span>
              <p>Your total is: ${TotalCost}</p>
            </div>
            <button onClick={createOrder}>Submit</button>
            <button onClick={resetForm}>Cancel</button>
          </div>
        </div>
      )}

      {editOrderById > 0 && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button onClick={() => { setUpdatedOrder({}); setEditOrderById(0); }}><X /></button>
            <h3>Edit Order #{editOrderById}</h3>

            <div className="order-type-row">
              <span>
                Delivery{" "}
                <input
                  type="checkbox"
                  checked={updatedOrder.delivererId != null}
                  onChange={() => {
                    setUpdatedOrder({ ...updatedOrder, delivererId: Math.floor(Math.random() * deliverers.length) + 1, tableNumber: null });
                  }}
                />
              </span>
              <span>
                Dine In{" "}
                <input
                  type="checkbox"
                  checked={updatedOrder.tableNumber != null}
                  onChange={() => {
                    setUpdatedOrder({ ...updatedOrder, tableNumber: Math.floor(Math.random() * 15) + 1, delivererId: null });
                  }}
                />
              </span>
            </div>
            <button onClick={() => setUpdatedOrder({ ...updatedOrder, pizza: [...updatedOrder.pizza, {}] })}>+ Add Pizza</button>
            {updatedOrder.pizza.map((pizza, index) => (
              <div key={index} className="pizza-section">
                <h4>Pizza {index + 1}</h4>
                <button onClick={() => setUpdatedOrder({ ...updatedOrder, pizza: [...updatedOrder.pizza.filter((_, i) => i !== index)] })}>Cancel Pizza</button>

                <div className="flex-direction-row">
                  {sizes.map((s) => (
                    <div key={s.id}>
                      <input type="radio" name={`size-${index}`} defaultChecked={updatedOrder.pizza[index].sizeId == s.id} onChange={() => {
                        const newPizza = [...updatedOrder.pizza]; newPizza[index] = { ...newPizza[index], sizeId: s.id };
                        setUpdatedOrder({ ...updatedOrder, pizza: newPizza });
                      }} />{s.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {cheeses.map((c) => (
                    <div key={c.id}>
                      <input type="radio" name={`cheese-${index}`} defaultChecked={updatedOrder.pizza[index].cheeseId == c.id} onChange={() => {
                        const newPizza = [...updatedOrder.pizza]; newPizza[index] = { ...newPizza[index], cheeseId: c.id };
                        setUpdatedOrder({ ...updatedOrder, pizza: newPizza });
                      }} />{c.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {sauces.map((s) => (
                    <div key={s.id}>
                      <input type="radio" name={`sauce-${index}`} defaultChecked={updatedOrder.pizza[index].sauceId == s.id} onChange={() => {
                        const newPizza = [...updatedOrder.pizza]; newPizza[index] = { ...newPizza[index], sauceId: s.id };
                        setUpdatedOrder({ ...updatedOrder, pizza: newPizza });
                      }} />{s.name}
                    </div>
                  ))}
                </div>

                <div className="flex-direction-row">
                  {toppings.map((t) => (
                    <div key={t.id}>
                      <input
                        type="checkbox"
                        defaultChecked={updatedOrder.pizza[index].toppings.some(
                          (topping) => topping.toppingId === t.id
                        )}
                        onChange={(e) => {
                          const currentPizza = updatedOrder.pizza[index];
                          const currentToppings = [...currentPizza.toppings];
                          const toppingIndex = currentToppings.findIndex(
                            (topping) => topping.toppingId === t.id
                          );

                          if (!e.target.checked && toppingIndex !== -1) {
                            currentToppings.splice(toppingIndex, 1);
                          } else if (e.target.checked && toppingIndex === -1) {
                            currentToppings.push({id: 0, pizzaId: updatedOrder.pizza[index].id, toppingId: t.id });
                          }

                          const newPizza = [...updatedOrder.pizza];
                          newPizza[index] = {
                            ...currentPizza,
                            toppings: currentToppings,
                          };

                          setUpdatedOrder({
                            ...updatedOrder,
                            pizza: newPizza,
                          });
                        }}
                      />
                      {t.name}
                    </div>
                  ))}
                </div>
              </div>
            ))}
            <div className="tip-input">
              <span>
                Tip $<input type="number" defaultValue={updatedOrder.tip} onChange={(e) => setUpdatedOrder({...updatedOrder, tip: e.target.value})} />
              </span>
              <p></p>
            </div>
            <button >Submit</button>
            <button onClick={() => { setUpdatedOrder({}); setEditOrderById(0) }}>Cancel</button>
          </div>
        </div>
      )}
    </div>
  );
};
