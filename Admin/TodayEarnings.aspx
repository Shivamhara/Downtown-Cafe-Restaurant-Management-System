<%@ Page Title="Today's Earnings" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TodayEarnings.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.TodayEarnings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .earn-card {
            background: linear-gradient(135deg, #27ae60, #2ecc71);
            color: white;
            border-radius: 15px;
            padding: 40px;
            text-align: center;
            box-shadow: 0 6px 20px rgba(0,0,0,0.2);
        }

        .earn-value {
            font-size: 40px;
            font-weight: bold;
        }

        .earn-title {
            font-size: 20px;
            margin-bottom: 10px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-5">

    <div class="earn-card">

        <div class="earn-title">💰 Today's Earnings</div>

        <div class="earn-value">
            <asp:Label ID="lblToday" runat="server"></asp:Label>
        </div>

    </div>

</div>

</asp:Content>