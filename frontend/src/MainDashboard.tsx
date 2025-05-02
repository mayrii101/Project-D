import React, { useEffect, useState } from "react";
import "./MainDashboard.css";
import SearchBar from "./SearchBar";
import Filter from "./Filter";

interface Order {
  id: number;
  status: string;
}

const Dashboard: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [filteredOrders, setFilteredOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedStatus, setSelectedStatus] = useState("");

  useEffect(() => {
    fetch("https://localhost:5000/api/Order")
      .then((res) => res.json())
      .then((data) => {
        setOrders(data);
        setFilteredOrders(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Failed to fetch orders:", err);
        setLoading(false);
      });
  }, []);

  useEffect(() => {
    const filtered = orders.filter(order =>
      order.id.toString().includes(searchTerm) ||
      order.status.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredOrders(filtered);
  }, [searchTerm, orders]);

  useEffect(() => {
    const filtered = orders.filter(order => {
      const matchesSearch =
        order.id.toString().includes(searchTerm) ||
        order.status.toLowerCase().includes(searchTerm.toLowerCase());

      const matchesStatus =
        !selectedStatus || order.status.toLowerCase() === selectedStatus.toLowerCase();

      return matchesSearch && matchesStatus;
    });
    setFilteredOrders(filtered);
  }, [searchTerm, orders, selectedStatus]);

  const stats = {
    total: filteredOrders.length,
    delivered: filteredOrders.filter(o => o.status.toLowerCase() === "delivered").length,
    inTransit: filteredOrders.filter(o => o.status.toLowerCase() === "shipped").length,
    cancelled: filteredOrders.filter(o => o.status.toLowerCase() === "cancelled").length,
  };

  return (
    <div className="dashboard">
      <header className="header">Database (Admin View)</header>

      <div className="main">
        <div className="dashboard-header">
          <div className="banner">Dashboard</div>
          <div className="header-controls">
            <SearchBar searchTerm={searchTerm} onSearchChange={setSearchTerm} />
            <Filter selectedStatus={selectedStatus} onStatusChange={setSelectedStatus} />
          </div>
        </div>

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
                {filteredOrders.slice(0, 5).map((o) => (
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
