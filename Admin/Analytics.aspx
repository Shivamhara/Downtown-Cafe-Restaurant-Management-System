<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Analytics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="text-center mb-4">📊 Admin Analytics Dashboard</h3>

    <!-- TOP CARDS -->
    <div class="row text-center">

        <div class="col-md-3 mb-3">
            <div class="card p-3 bg-primary text-white">
                <h5>Total Orders</h5>
                <asp:Label ID="lblOrders" runat="server" Font-Size="Large"></asp:Label>
            </div>
        </div>

        <div class="col-md-3 mb-3">
            <div class="card p-3 bg-success text-white">
                <h5>Total Revenue</h5>
                <asp:Label ID="lblRevenue" runat="server" Font-Size="Large"></asp:Label>
            </div>
        </div>

        <div class="col-md-3 mb-3">
            <div class="card p-3 bg-warning text-dark">
                <h5>Total Customers</h5>
                <asp:Label ID="lblCustomers" runat="server" Font-Size="Large"></asp:Label>
            </div>
        </div>

        <div class="col-md-3 mb-3">
            <div class="card p-3 bg-info text-white">
                <h5>Membership Users</h5>
                <asp:Label ID="lblMembers" runat="server" Font-Size="Large"></asp:Label>
            </div>
        </div>

    </div>

    <hr />

    <!-- TOP SELLING PRODUCTS -->
    <h4>🔥 Top 5 Selling Products</h4>

    <asp:GridView ID="gvTopProducts" runat="server" CssClass="table table-bordered mt-3"></asp:GridView>

    <hr />

    <!-- TOP CUSTOMERS -->
    <h4>👑 Top 5 Customers</h4>

    <asp:GridView ID="gvTopCustomers" runat="server" CssClass="table table-bordered mt-3"></asp:GridView>

    <hr />

    <!-- MEMBERSHIP USERS -->
    <h4>💎 Active Membership Users</h4>

    <asp:GridView ID="gvMembers" runat="server" CssClass="table table-bordered mt-3"></asp:GridView>

</div>

</asp:Content>