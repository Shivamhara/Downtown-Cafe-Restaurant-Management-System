<%@ Page Title="Track Order" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="Trackorder.aspx.cs" Inherits="CafeandfoodRestaurant.User.Trackorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tracking-step{
            font-size:14px;
            padding:8px 16px;
            border-radius:6px;
            margin:3px;
            color:white;
        }
        .received{background:#6c757d;}
        .process{background:#0d6efd;}
        .ready{background:#ffc107;color:black;}
        .reached{background:#198754;}
        .current-step{
            box-shadow:0 0 10px rgba(0,0,0,0.3);
            transform: scale(1.05);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">
    <h3 class="mb-4">📍 Track Your Order</h3>

    <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block"></asp:Label>

<asp:TextBox ID="txtOrderID" autocomplete="off" runat="server" CssClass="form-control mb-2" Placeholder="Enter your Order ID"></asp:TextBox>
    <asp:Button ID="btnTrack" runat="server" Text="Track Order" CssClass="btn btn-primary mb-4" OnClick="btnTrack_Click" />
    <asp:Panel ID="pnlOrderDetails" runat="server" Visible="false">
        <h5>📄 Order Details</h5>
        <asp:Literal ID="litOrderDetails" runat="server"></asp:Literal>
        <asp:Literal ID="litMenuDetails" runat="server"></asp:Literal>
      
      <div id="trackingDiv" runat="server" visible="false">
       <div class="d-flex flex-wrap mb-3">
    <div id="stepReceived" visible="false" runat="server" class="tracking-step received">Received</div>
    <div id="stepProcess" visible="false" runat="server" class="tracking-step process">Process Started</div>
    <div id="stepReady" visible="false" runat="server" class="tracking-step ready">Ready to Send</div>
    <div id="stepReached" visible="false" runat="server" class="tracking-step reached">Reached</div>
</div>
          </div>
    </asp:Panel>
   
    </div>

</asp:Content>