import React, { useEffect, useState } from "react";
import "./MainDashboard.css";
import SearchBar from "./SearchBar";
import Filter from "./Filter";

interface Order {
  id: number;
  customerId: number;
  customer: {
    id: number;
    bedrijfsNaam: string;
    contactPersoon: string;
    email: string;
    telefoonNummer: string;
    adres: string;
    isDeleted: boolean;
  };
  productLines: Array<{
    productId: number;
    productName: string;
    quantity: number;
    price: number;
    product: {
      weightKg: number;
    };
  }>;
  status: string;
  orderDate: string;
  expectedDeliveryDate: string;
  actualDeliveryDate?: string;
  deliveryAddress: string;
  totalWeight: number;
  isDeleted: boolean;
}

const Dashboard: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [filteredOrders, setFilteredOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedStatus, setSelectedStatus] = useState("");
  const [showOrderDetails, setShowOrderDetails] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);

  useEffect(() => {
    fetch("http://localhost:5000/api/order")
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
    const filtered = orders.filter((order) => {
      const lowerSearch = searchTerm.toLowerCase();

      const matchesSearch =
        order.id.toString().includes(searchTerm) ||
        order.status.toLowerCase().includes(lowerSearch) ||
        order.customer.bedrijfsNaam.toLowerCase().includes(lowerSearch) ||
        order.customer.email.toLowerCase().includes(lowerSearch);

      const matchesStatus =
        !selectedStatus ||
        order.status.toLowerCase() === selectedStatus.toLowerCase();

      return matchesSearch && matchesStatus;
    });

    // Sorteren op orderdatum, nieuwste eerst
    const sorted = [...filtered].sort(
      (a, b) =>
        new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime()
    );

    setFilteredOrders(sorted);
  }, [searchTerm, orders, selectedStatus]);

  const stats = {
    total: filteredOrders.length,
    delivered: filteredOrders.filter((o) => o.status.toLowerCase() === "delivered").length,
    inTransit: filteredOrders.filter((o) => o.status.toLowerCase() === "shipped").length,
    cancelled: filteredOrders.filter((o) => o.status.toLowerCase() === "cancelled").length,
  };

  const handleFilterClick = (status?: string) => {
    setShowOrderDetails(true);
    if (status) {
      const filtered = orders.filter(
        (order) => order.status.toLowerCase() === status.toLowerCase()
      );
      setFilteredOrders(filtered);
    } else {
      setFilteredOrders(orders);
    }
    setSelectedOrder(null);
  };

  const handleOrderClick = (order: Order) => {
    setSelectedOrder(order);
    setShowOrderDetails(true);
  };

  const closeModal = () => {
    setShowOrderDetails(false);
    setSelectedOrder(null);
    setFilteredOrders(orders);
  };

  return (
    <div className="dashboard">
      <header className="header">Lafeber</header>

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
              <div className="card clickable" onClick={() => handleFilterClick()}>
                <h3>Total Orders</h3>
                <p>{stats.total}</p>
              </div>
              <div className="card clickable" onClick={() => handleFilterClick("delivered")}>
                <h3>Delivered</h3>
                <p>{stats.delivered}</p>
              </div>
              <div className="card clickable" onClick={() => handleFilterClick("shipped")}>
                <h3>Shipped</h3>
                <p>{stats.inTransit}</p>
              </div>
              <div className="card clickable" onClick={() => handleFilterClick("cancelled")}>
                <h3>Cancelled</h3>
                <p>{stats.cancelled}</p>
              </div>
            </div>

            <div className="details">
              <div className="section">
                <h2>Recent Orders</h2>
                {filteredOrders.length === 0 ? (
                  <p>Geen resultaten gevonden.</p>
                ) : (
                  filteredOrders.slice(0, 5).map((o) => (
                    <p key={o.id} onClick={() => handleOrderClick(o)} className="clickable">
                      <span>Order #{o.id}</span>
                      <span className={`status ${o.status.toLowerCase()}`}>
                        {o.status}
                      </span>
                    </p>
                  ))
                )}
              </div>
            </div>
          </>
        )}
      </div>

      {showOrderDetails && (
        <div className="modal-overlay">
          <div className="modal">
            <div className="modal-header">
              <h2>
                {selectedOrder
                  ? `Order #${selectedOrder.id} Details`
                  : `Filtered Orders (${filteredOrders.length})`}
              </h2>
              <div className="modal-header-buttons">
                {selectedOrder && (
                  <button className="back-button" onClick={() => setSelectedOrder(null)}>
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none"
                      xmlns="http://www.w3.org/2000/svg">
                      <path d="M15 18L9 12L15 6" stroke="currentColor"
                        strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
                    </svg>
                  </button>
                )}
                <button className="close-button" onClick={closeModal}>
                  &times;
                </button>
              </div>
            </div>
            <div className="modal-content">
              {selectedOrder ? (
                <div className="order-details">
                  <div className="detail-row">
                    <span className="detail-label">Status:</span>
                    <span className={`status ${selectedOrder.status.toLowerCase()}`}>
                      {selectedOrder.status}
                    </span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Bedrijfsnaam:</span>
                    <span>{selectedOrder.customer.bedrijfsNaam}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Contactpersoon:</span>
                    <span>{selectedOrder.customer.contactPersoon}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">E-mail:</span>
                    <span>{selectedOrder.customer.email}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Telefoon:</span>
                    <span>{selectedOrder.customer.telefoonNummer}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Order Date:</span>
                    <span>{new Date(selectedOrder.orderDate).toLocaleDateString()}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Expected Delivery:</span>
                    <span>{new Date(selectedOrder.expectedDeliveryDate).toLocaleDateString()}</span>
                  </div>
                  {selectedOrder.actualDeliveryDate && (
                    <div className="detail-row">
                      <span className="detail-label">Actual Delivery:</span>
                      <span>{new Date(selectedOrder.actualDeliveryDate).toLocaleDateString()}</span>
                    </div>
                  )}
                  <div className="detail-row">
                    <span className="detail-label">Delivery Address:</span>
                    <span>{selectedOrder.deliveryAddress}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Total Weight:</span>
                    <span>{selectedOrder.totalWeight} kg</span>
                  </div>
                </div>
              ) : (
                <div className="all-orders">
                  <div className="orders-list">
                    {filteredOrders.map((order) => (
                      <div
                        key={order.id}
                        className="order-summary clickable"
                        onClick={() => handleOrderClick(order)}
                      >
                        <span>Order #{order.id}</span>
                        <span className={`status ${order.status.toLowerCase()}`}>
                          {order.status}
                        </span>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Dashboard;
