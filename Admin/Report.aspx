<%@ Page Title="Admin Reports" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .report-card {
            background: #fff;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            margin-bottom: 20px;
            text-align: center;
            transition: transform 0.2s;
        }
        .report-card:hover { transform: translateY(-5px); }
        .report-card h4 { font-weight: 600; margin-bottom: 10px; }
        .report-card p { font-size: 18px; color: #0f4c5c; }
        @media (max-width: 576px) { .report-card p { font-size: 16px; } }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4">📊 Admin Dashboard Reports</h3>

        <!-- Summary Cards -->

        <div class="row g-4 mb-4">
            <div class="col-md-3 col-6">
                <div class="report-card">
                    <h4>Total Customers</h4>
                    <p><asp:Label ID="lblTotalCustomers" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="col-md-3 col-6">
                <div class="report-card">
                    <h4>Total Orders</h4>
                    <p><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="col-md-3 col-6">
                <div class="report-card">
                    <h4>Total Revenue</h4>
                    <p>₹<asp:Label ID="lblRevenue" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="col-md-3 col-6">
                <div class="report-card">
                    <h4>Active Memberships</h4>
                    <p><asp:Label ID="lblActiveMemberships" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
        </div>

        <!-- Recent Orders Table -->
        <div class="card mb-4">
            <div class="card-header bg-primary text-white">
                Recent Orders
            </div>
                <div style="overflow-x:auto;">

            <div class="card-body p-0">
<asp:GridView ID="gvRecentOrders" runat="server"
    CssClass="table table-striped table-bordered mb-0"
    AutoGenerateColumns="False"
    AllowPaging="true" PageSize="10"
    OnPageIndexChanging="gvRecentOrders_PageIndexChanging">                    <Columns>
                        <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="FinalAmount" HeaderText="Amount" DataFormatString="₹{0:N2}" />
                        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" />
                        <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Membership Table -->
        <div class="card">
            <div class="card-header bg-success text-white">
                Active Memberships
            </div>
            <div class="card-body p-0">
<asp:GridView ID="gvMemberships" runat="server"
    CssClass="table table-striped table-bordered mb-0"
    AutoGenerateColumns="False"
    AllowPaging="true" PageSize="10"
    OnPageIndexChanging="gvMemberships_PageIndexChanging">                    <Columns>
                        <asp:BoundField DataField="MembershipID" HeaderText="ID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="PlanName" HeaderText="Plan" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="EndDate" HeaderText="End" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="OrdersUsed" HeaderText="Used Orders" />
                        <asp:BoundField DataField="MaxOrders" HeaderText="Max Orders" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        </div>
    </div>
</asp:Content>