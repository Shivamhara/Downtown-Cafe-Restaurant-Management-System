<%@ Page Title="My Membership" Language="C#" MasterPageFile="~/User/UserMaster.Master"
    AutoEventWireup="true" CodeBehind="MyMembership.aspx.cs" Inherits="CafeandfoodRestaurant.User.MyMembership" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">
        <h3 class="mb-4">🎟 My Membership Details</h3>

        <asp:Panel ID="pnlMembership" runat="server" CssClass="card p-4" Visible="false">
            <h5>Membership Amount: ₹<asp:Label ID="lblAmount" runat="server"></asp:Label></h5>
            <h5>Discount: <asp:Label ID="lblDiscount" runat="server"></asp:Label>%</h5>
            <h5>Max Orders: <asp:Label ID="lblMaxOrders" runat="server"></asp:Label></h5>
            <h5>Orders Used: <asp:Label ID="lblOrdersUsed" runat="server"></asp:Label></h5>
            <h5>Start Date: <asp:Label ID="lblStart" runat="server"></asp:Label></h5>
            <h5>End Date: <asp:Label ID="lblEnd" runat="server"></asp:Label></h5>
        </asp:Panel>

        <asp:Label ID="lblNoMembership" runat="server" CssClass="text-danger" Visible="false" Text="You do not have an active membership."></asp:Label>
    </div>

</asp:Content>