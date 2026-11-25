<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Customers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .table th {
            background-color: #343a40;
            color: white;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="mb-4">👥 Customers List</h3>

    <asp:GridView ID="gvCustomers" runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="False">

        <Columns>

            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="City" HeaderText="City" />

        </Columns>

    </asp:GridView>

</div>

</asp:Content>