<%@ Page Title="Admin Settings" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master"
    AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3>⚙ Admin Settings</h3>

    <!-- Change Username -->
    <div class="card p-3 mb-4">
        <h5>Change Username</h5>

        <asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control mb-2"
            placeholder="Enter New Username"></asp:TextBox>

        <asp:Button ID="btnUpdateUsername" runat="server"
            Text="Update Username"
            CssClass="btn btn-primary"
            OnClick="btnUpdateUsername_Click" />

        <asp:Label ID="lblUserMsg" runat="server" CssClass="text-success mt-2"></asp:Label>
    </div>

    <!-- Change Password -->
    <div class="card p-3">
        <h5>Change Password</h5>

        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"
            CssClass="form-control mb-2"
            placeholder="Enter New Password"></asp:TextBox>

        <asp:Button ID="btnUpdatePassword" runat="server"
            Text="Update Password"
            CssClass="btn btn-danger"
            OnClick="btnUpdatePassword_Click" />

        <asp:Label ID="lblPassMsg" runat="server" CssClass="text-success mt-2"></asp:Label>
    </div>

</div>

</asp:Content>