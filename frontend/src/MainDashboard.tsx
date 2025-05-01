import React, { useEffect, useState } from "react";
import "./MainDashboard.css";

interface Order {
  id: number;
  status: string;
}

const Dashboard: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("https://localhost:5000/api/Order")
      .then((res) => res.json())
      .then((data) => {
        setOrders(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Failed to fetch orders:", err);
        setLoading(false);
      });
  }, []);

  const stats = {
    total: orders.length,
    delivered: orders.filter(o => o.status.toLowerCase() === "delivered").length,
    inTransit: orders.filter(o => o.status.toLowerCase() === "shipped").length,
    cancelled: orders.filter(o => o.status.toLowerCase() === "cancelled").length,
    pending: orders.filter(o => o.status.toLowerCase() === "pending").length,
    processing: orders.filter(o => o.status.toLowerCase() === "processing").length
  };

  return (
    <div className="dashboard">
      <header className="header">Database (Admin View)</header>

      <div className="main">
        <div className="banner">Dashboard</div>

        {loading ? (
          <div className="loading">Loading data...</div>
        ) : (
          <>
            <div className="stats">
              <div className="card">
                <h3>Total Orders</h3>
                <p>{stats.total}</p>
              </div>
              <div className="card">
                <h3>Delivered</h3>
                <p>{stats.delivered}</p>
              </div>
              <div className="card">
                <h3>In Transit</h3>
                <p>{stats.inTransit}</p>
              </div>
              <div className="card">
                <h3>Cancelled</h3>
                <p>{stats.cancelled}</p>
              </div>
            </div>

            <div className="details">
              <div className="section">
                <h2>Recent Orders</h2>
                {orders.slice(0, 5).map((o) => (
                  <p key={o.id}>
                    <span>Order #{o.id}</span>
                    <span className={`status ${o.status.toLowerCase()}`}>
                      {o.status}
                    </span>
                  </p>
                ))}
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Dashboard;