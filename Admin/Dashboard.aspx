<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .dashboard-card {
            border-radius: 12px;
            padding: 25px;
            color: white;
            font-size: 18px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }

        .card-title {
            font-size: 16px;
            opacity: 0.9;
        }

        .card-value {
            font-size: 30px;
            font-weight: bold;
        }

        .bg-earn {
            background: #27ae60;
        }

        .bg-book {
            background: #2980b9;
        }

        .bg-order {
            background: #f39c12;
        }

        .bg-user {
            background: #8e44ad;
        }
        .dashboard-card {
    cursor: pointer;
    transition: transform 0.2s ease;
}

.dashboard-card:hover {
    transform: scale(1.05);
}
    </style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid mt-4">

        <h3 class="mb-4">📊 Admin Dashboard</h3>

        <div class="row">
           <div class="col-md-3">
    <a href="Earnings.aspx" style="text-decoration:none;">
        <div class="dashboard-card bg-dark">
            <div class="card-title">💰 Total Earnings</div>
            <div class="card-value">
                ₹ <asp:Label ID="lblTotalEarning" runat="server"></asp:Label>
            </div>
        </div>
    </a>
</div>
            <!-- Total Earnings -->
          <div class="col-md-3">
    <a href="TodayEarnings.aspx" style="text-decoration:none;">
        <div class="dashboard-card bg-earn">
            <div class="card-title">💰 Today's Earnings</div>
            <div class="card-value">
                ₹ <asp:Label ID="lblTodayEarning" runat="server"></asp:Label>
            </div>
        </div>
    </a>
</div>

           <div class="col-md-3">
    <a href="TakeActionTableOrder.aspx" style="text-decoration:none;">
        <div class="dashboard-card bg-book">
            <div class="card-title">🍽 Today's Table Bookings</div>
            <div class="card-value">
                <asp:Label ID="lblTodayBookings" runat="server"></asp:Label>
            </div>
        </div>
    </a>
</div>

         <div class="col-md-3">
    <a href="ViewOrders.aspx" style="text-decoration:none;">
        <div class="dashboard-card bg-order">
            <div class="card-title">📦 Today's Orders</div>
            <div class="card-value">
                <asp:Label ID="lblTodayOrders" runat="server"></asp:Label>
            </div>
        </div>
    </a>
</div>

          <div class="col-md-3">
    <a href="Customers.aspx" style="text-decoration:none;">
        <div class="dashboard-card bg-user">
            <div class="card-title">👥 Total Customers</div>
            <div class="card-value">
                <asp:Label ID="lblCustomers" runat="server"></asp:Label>
            </div>
        </div>
    </a>
</div>
        </div>

    </div>

</asp:Content>
