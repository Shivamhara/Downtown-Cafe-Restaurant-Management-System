<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TableBookings.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.TableBookings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />--%>

    <style>
        .table th {
            background-color: #343a40;
            color: white;
        }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="mb-4">🍽 Table Bookings</h3>

    <asp:GridView ID="gvBookings" runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="False">

        <Columns>

            <asp:BoundField DataField="BookingID" HeaderText="Booking ID" />
            <asp:BoundField DataField="FullName" HeaderText="Customer Name" />
            <asp:BoundField DataField="BookingDate" HeaderText="Booking Date" />
            <asp:BoundField DataField="BookingStatus" HeaderText="Status" />
            <asp:BoundField DataField="TotalAmount" HeaderText="Amount (₹)" />

        </Columns>

    </asp:GridView>

</div>

</asp:Content>

