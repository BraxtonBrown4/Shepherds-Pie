import { useEffect, useState } from "react"
import { getAllOrders } from "../managers/Orders.manager"
import "./Orders.css"

export const Orders = () => {
    const [orders, setOrders] = useState([])
    const getSetOrders = () => { getAllOrders().then(setOrders) }

    useEffect(() => {
        getSetOrders()
    }, [])

    return (
        <div className="orders-container">
            <h2 className="orders-title">Orders</h2>
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
        </div>
    )
}
