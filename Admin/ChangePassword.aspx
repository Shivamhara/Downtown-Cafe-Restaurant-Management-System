<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="max-width:500px;margin:auto;padding:30px;background:white;border-radius:10px;box-shadow:0 5px 15px rgba(0,0,0,0.1);">

    <h2 style="text-align:center;margin-bottom:20px;">Change Admin Credentials</h2>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>

    <div class="form-group">
        <label>Current Username</label>
        <asp:TextBox ID="txtCurrentUsername" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Current Password</label>
        <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="SingleLine" CssClass="form-control" />
    </div>

    <hr />

    <div class="form-group">
        <label>New Username</label>
        <asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>New Password</label>
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Confirm Password</label>
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <br />

    <asp:Button ID="btnChange" runat="server" Text="Update Credentials"
        CssClass="btn btn-primary btn-block"
        OnClick="btnChange_Click" />

</div>
</asp:Content>
