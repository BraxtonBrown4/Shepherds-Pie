import { useEffect, useState } from "react"
import { getAllOrders } from "../managers/Orders.manager"
import { getAllToppings } from "../managers/Toppings.manager"
import { getAllCheeses } from "../managers/Cheeses.manager"
import { getAllSauces } from "../managers/Sauces.manager"
import { getAllSizes } from "../managers/Sizes.manager"
import { getAllDeliverers } from "../managers/Deliverers.manager"
import { getAllEmployees } from "../managers/Employees.manager"
import { X, Pizza } from "lucide-react"
import "./Orders.css"

export const Orders = () => {
    const [orders, setOrders] = useState([])
    const [toppings, setToppings] = useState([])
    const [cheeses, setCheeses] = useState([])
    const [sauces, setSauces] = useState([])
    const [sizes, setSizes] = useState([])
    const [deliverers, setDeliverers] = useState([])

    const [employees, setEmployees] = useState([])
    const [employeeId, setEmployeeId] = useState(null)
    const [delivererId, setDelivererId] = useState(null)
    const [tableNumber, setTableNumber] = useState(null)
    const [tip, setTip] = useState(null)

    const [newOrderModal, setNewOrderModal] = useState(false)


    useEffect(() => {
        getAllOrders().then(setOrders)
        getAllToppings().then(setToppings)
        getAllCheeses().then(setCheeses)
        getAllSauces().then(setSauces)
        getAllSizes().then(setSizes)
        getAllDeliverers().then(setDeliverers)
        getAllEmployees().then(setEmployees)
    }, [])

    return (
        <div className="orders-container">
            <h2 className="orders-title">Orders</h2>
            <button onClick={() => setNewOrderModal(true)}>New Order<Pizza/></button>
            {
                orders.map(o =>
                    <div className="order-card" key={o.id}>
                        <div className="order-grid">
                            <div className="order-info">
                                <h3>Order #{o.id}</h3>
                                <p><strong>Employee ID:</strong> {o.employeeId}</p>
                                <p><strong>Deliverer ID:</strong> {o.delivererId || "None"}</p>
                                <p><strong>Table Number:</strong> {o.tableNumber || "None"}</p>
                                <p><strong>Tip:</strong> ${o.tip}</p>
                                <p><strong>Order Time:</strong> {new Date(o.orderTime).toLocaleDateString()} {new Date(o.orderTime).toLocaleTimeString()}</p>
                                <p><strong>Total Cost:</strong> ${o.totalCost}</p>
                            </div>

                            <div className="pizza-list">
                                {o.pizza.map(p =>
                                    <div className="pizza-card" key={p.id}>
                                        <h4>Pizza #{p.id}</h4>
                                        <p><strong>Sauce ID:</strong> {p.sauceId}</p>
                                        <p><strong>Size ID:</strong> {p.SizeId}</p>
                                        <p><strong>Cheese ID:</strong> {p.cheeseId}</p>
                                        <p><strong>Toppings:</strong></p>
                                        <ul className="toppings-list">
                                            {p.toppings.map(t =>
                                                <li key={t.id}>{t.topping.name}</li>
                                            )}
                                        </ul>
                                        <p><strong>Price:</strong> ${p.price}</p>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                )
            }

            {newOrderModal && (
                <div className="modal-overlay">
                    <div className="modal-content">
                        <button onClick={() => setNewOrderModal(false)}><X/></button>
                        <h3>New Order</h3>
                        <div>
                            <span>Dellivery <input type="checkbox" checked={typeof(delivererId) == "number"} onChange={() => {setDelivererId(1); setTableNumber(null)}}/></span>
                            <span>Dine In <input type="checkbox" checked={typeof(tableNumber) == "number"} onChange={() => {setTableNumber(1); setDelivererId(null)}}/></span>
                        </div>

                        <span>Tip $<input type="number" onChange={(e) => {setTip(parseInt(e.target.value))}}/></span>
                    </div>
                </div>
            )}
        </div>
    )
}