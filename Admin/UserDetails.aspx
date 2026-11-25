<%@ Page Title="User Details" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.UserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    .details-card {
        background: white;
        border-radius: 20px;
        padding: 30px;
        box-shadow: 0 10px 30px rgba(0,0,0,0.15);
        max-width: 700px;
        margin: auto;
    }

    .details-title {
        font-size: 24px;
        font-weight: 700;
        margin-bottom: 20px;
        text-align: center;
    }

    .detail-row {
        padding: 10px 0;
        border-bottom: 1px solid #eee;
    }

    .detail-label {
        font-weight: 600;
        color: #555;
    }

    .badge-active {
        background: #28a745;
        color: white;
        padding: 5px 10px;
        border-radius: 10px;
    }

    .badge-inactive {
        background: #dc3545;
        color: white;
        padding: 5px 10px;
        border-radius: 10px;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <div class="details-card">

        <div class="details-title">👤 User Details</div>

        <div class="detail-row">
            <span class="detail-label">Full Name:</span>
            <asp:Label ID="lblName" runat="server"></asp:Label>
        </div>

        <div class="detail-row">
            <span class="detail-label">Email:</span>
            <asp:Label ID="lblEmail" runat="server"></asp:Label>
        </div>

        <div class="detail-row">
            <span class="detail-label">Registration Date:</span>
            <asp:Label ID="lblDate" runat="server"></asp:Label>
        </div>

        <div class="detail-row">
            <span class="detail-label">Status:</span>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </div>

        <div class="detail-row">
            <span class="detail-label">Membership:</span>
            <asp:Label ID="lblMembership" runat="server"></asp:Label>
        </div>

        <div class="detail-row">
            <span class="detail-label">Total Orders:</span>
            <asp:Label ID="lblOrders" runat="server"></asp:Label>
        </div>

        <div class="text-center mt-4">
            <a href="UserList.aspx" class="btn btn-secondary">⬅ Back</a>
        </div>

    </div>

</div>

</asp:Content>