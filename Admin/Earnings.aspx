<%@ Page Title="Total Earnings" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Earnings.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Earnings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .earn-card {
            background: linear-gradient(135deg, #2c3e50, #4ca1af);
            color: white;
            border-radius: 15px;
            padding: 30px;
            text-align: center;
            margin-bottom: 30px;
        }

        .earn-value {
            font-size: 40px;
            font-weight: bold;
        }

        .table th {
            background-color: #343a40;
            color: white;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <!-- Total Earnings -->
    <div class="earn-card">
        <h4>💰 Total Earnings</h4>
        <div class="earn-value">
            ₹ <asp:Label ID="lblTotalEarning" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Buttons -->
    <div class="mb-3">
        <asp:Button ID="btnOrders" runat="server" Text="📦 Orders Earnings"
            CssClass="btn btn-primary me-2" OnClick="btnOrders_Click" />

        <asp:Button ID="btnBookings" runat="server" Text="🍽 Table Bookings Earnings"
            CssClass="btn btn-success" OnClick="btnBookings_Click" />
    </div>

    <!-- Grid -->
    <asp:GridView ID="gvEarnings" runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="True">
    </asp:GridView>

</div>

</asp:Content>