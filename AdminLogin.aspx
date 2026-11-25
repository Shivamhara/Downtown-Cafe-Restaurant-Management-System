<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/HomemasterPage.Master" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="CafeandfoodRestaurant.AdminLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Add Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #f5f5f5, #ffe5d9);
            height: 100vh;
        }
        .login-container {
            max-width: 400px;
            margin: 80px auto;
            padding: 40px;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0px 8px 20px rgba(0,0,0,0.2);
        }
        .login-container h2 {
            font-weight: 700;
            color: #ff6f61;
            margin-bottom: 30px;
            text-align: center;
        }
        .btn-primary {
            background-color: #ff6f61;
            border: none;
        }
        .btn-primary:hover {
            background-color: #ff3b2e;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-container">
        <h2>Admin Login</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mb-3 d-block" />

        <asp:Panel ID="pnlLogin" runat="server">
            <div class="mb-3">
                <asp:Label ID="lblUsername" runat="server" Text="Email" AssociatedControlID="txtUsername" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Enter username"></asp:TextBox>
            </div>
            <div class="mb-3">
                <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Placeholder="Enter password"></asp:TextBox>
            </div>
            <div class="d-grid">
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>