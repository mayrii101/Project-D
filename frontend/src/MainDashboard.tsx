import React from "react";
import "./MainDashboard.css";

const Dashboard: React.FC = () => {
  return (
    <div className="dashboard">
      <header className="header">Dashboard (Admin View)</header>

      <div className="main">
        <div className="banner">Dashboard</div>

        <div className="stats">
          <div className="card">total shipments : 250</div>
          <div className="card">deliverd: 200</div>
          <div className="card">in transit : 40</div>
          <div className="card">delayed: 20</div>
        </div>

        <div className="details">
          <div className="section">
            <h2>Monthly Shipment Trends</h2>
            <img
              src="/shipment-trend-placeholder.png"
              alt="Monthly Trends"
              className="trend-img"
            />
          </div>

          <div className="section">
            <h2>Recent Shipments</h2>
            <p>#12345: AMS → RTD (In Transit)</p>
            <p>#12346: DH → GDA (Delivered)</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
