<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CafeandfoodRestaurant.User.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dashboard-title {
            font-size: 28px;
            font-weight: 600;
            margin-bottom: 25px;
            color: #0f4c5c;
        }

        /* Cards */
        .stat-card {
            background: white;
            border-radius: 12px;
            padding: 20px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.08);
            transition: 0.3s;
            margin-bottom: 15px;
        }
        .stat-card:hover {
            transform: translateY(-5px);
        }

        .stat-icon {
            font-size: 30px;
            margin-bottom: 10px;
            color: #ff7f50;
        }

        /* Quick buttons */
        .quick-btn {
            border-radius: 10px;
            padding: 15px;
            font-weight: 500;
            transition: 0.3s;
            margin-bottom: 15px;
        }
        .quick-btn:hover {
            transform: scale(1.05);
        }

        /* Order table */
        .order-box {
            background: white;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.08);
            margin-top: 30px;
            overflow-x: auto; /* make GridView scrollable on mobile */
        }

        /* Mobile adjustments */
        @media (max-width: 767px) {
            .stat-card h3 { font-size: 20px; }
            .stat-card h5 { font-size: 16px; }
            .dashboard-title { font-size: 22px; }
        }

        @media (max-width: 480px) {
            .stat-card h3 { font-size: 18px; }
            .stat-card h5 { font-size: 14px; }
            .dashboard-title { font-size: 20px; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">

        <!-- Welcome -->
        <h2 class="dashboard-title">
            <i class="fa fa-user-circle"></i> Welcome to Your Dashboard
        </h2>

        <!-- Stats Cards -->
        <div class="row g-4">
            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <div class="stat-card text-center">
                    <div class="stat-icon"><i class="fa fa-shopping-cart"></i></div>
                    <h5>Total Orders</h5>
                    <h3><asp:Label ID="lblOrders" runat="server" Text="0"></asp:Label></h3>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <div class="stat-card text-center">
                    <div class="stat-icon"><i class="fa fa-chair"></i></div>
                    <h5>Table Bookings</h5>
                    <h3><asp:Label ID="lblBookings" runat="server" Text="0"></asp:Label></h3>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <div class="stat-card text-center">
                    <div class="stat-icon"><i class="fa fa-rupee-sign"></i></div>
                    <h5>Total Spent</h5>
                    <h3>₹<asp:Label ID="lblSpent" runat="server" Text="0"></asp:Label></h3>
                </div>
            </div>

            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <div class="stat-card text-center">
                    <div class="stat-icon"><i class="fa fa-crown"></i></div>
                    <h5>Membership</h5>
                    <h3><asp:Label ID="lblMembership" runat="server" Text="Active"></asp:Label></h3>
                </div>
            </div>
        </div>

        <!-- Quick Actions -->
        <div class="row mt-4 g-4">
            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <a href="ViewMenu.aspx" class="btn btn-warning w-100 quick-btn"><i class="fa fa-utensils"></i> Order Food</a>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <a href="BookTable.aspx" class="btn btn-success w-100 quick-btn"><i class="fa fa-chair"></i> Book Table</a>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <a href="Trackorder.aspx" class="btn btn-primary w-100 quick-btn"><i class="fa fa-list"></i> View Orders</a>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-12">
                <a href="MyMembership.aspx" class="btn btn-danger w-100 quick-btn"><i class="fa fa-crown"></i> Membership</a>
            </div>
        </div>

        <!-- Recent Orders -->
        <div class="order-box">
            <h5 class="mb-3"><i class="fa fa-clock"></i> Recent Orders</h5>
            <asp:GridView ID="GridRecentOrders" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="true">
            </asp:GridView>
        </div>

    </div>
</asp:Content>