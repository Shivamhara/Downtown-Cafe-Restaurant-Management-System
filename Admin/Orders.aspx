<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Orders" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="mb-4">📦 Orders List</h3>

    <asp:GridView ID="gvOrders" runat="server" CssClass="table table-bordered table-striped"
        AutoGenerateColumns="False">

        <Columns>

            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
            <asp:BoundField DataField="FullName" HeaderText="Customer Name" />
            <asp:BoundField DataField="FinalAmount" HeaderText="Amount (₹)" />
            <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
            <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" />

        </Columns>

    </asp:GridView>

</div>

</asp:Content>
