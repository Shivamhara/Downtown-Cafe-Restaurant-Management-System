<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ViewMembershipJoinUser.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ViewMembershipJoinUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="mb-3">👥 Users Joined This Membership</h3>

    <!-- Plan Info -->
    <div class="alert alert-info">
        Plan Amount: ₹<%= Request.QueryString["amt"] %> |
        Max Orders: <%= Request.QueryString["max"] %> |
        Discount: <%= Request.QueryString["disc"] %>%
    </div>

    <!-- Grid -->
    <asp:GridView ID="gvUsers" runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="False">

        <Columns>
            <asp:BoundField DataField="CustomerID" HeaderText="ID" />
            <asp:BoundField DataField="FullName" HeaderText="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="OrdersUsed" HeaderText="Used Orders" />
            <asp:BoundField DataField="MaxOrders" HeaderText="Max Orders" />
            <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" />
        </Columns>

    </asp:GridView>

</div>

</asp:Content>