<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.Chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="text-center mb-4">📊 Analytics Charts</h3>

    <!-- ROW 1 -->
    <div class="row">

        <div class="col-md-6">
            <h5>📈 Monthly Revenue</h5>
            <canvas id="revenueChart"></canvas>
        </div>

        <div class="col-md-6">
            <h5>📦 Order Status</h5>
            <canvas id="statusChart"></canvas>
        </div>

    </div>

    <!-- ROW 2 -->
    <div class="row mt-4">

        <div class="col-md-6">
            <h5>🔥 Top Products</h5>
            <canvas id="productChart"></canvas>
        </div>

        <div class="col-md-6">
            <h5>👑 Top Customers</h5>
            <canvas id="customerChart"></canvas>
        </div>

    </div>

</div>

<!-- Hidden Fields -->
<asp:HiddenField ID="hfMonths" runat="server" />
<asp:HiddenField ID="hfRevenue" runat="server" />

<asp:HiddenField ID="hfStatus" runat="server" />
<asp:HiddenField ID="hfStatusCount" runat="server" />

<asp:HiddenField ID="hfProductNames" runat="server" />
<asp:HiddenField ID="hfProductSales" runat="server" />

<asp:HiddenField ID="hfCustomerNames" runat="server" />
<asp:HiddenField ID="hfCustomerSpending" runat="server" />

<!-- JS -->
<script>

    // Revenue Chart
    new Chart(document.getElementById("revenueChart"), {
        type: 'bar',
        data: {
            labels: document.getElementById('<%=hfMonths.ClientID%>').value.split(','),
            datasets: [{
                label: "Revenue",
                data: document.getElementById('<%=hfRevenue.ClientID%>').value.split(',')
            }]
        }
    });

    // Order Status
    new Chart(document.getElementById("statusChart"), {
        type: 'pie',
        data: {
            labels: document.getElementById('<%=hfStatus.ClientID%>').value.split(','),
            datasets: [{
                data: document.getElementById('<%=hfStatusCount.ClientID%>').value.split(',')
            }]
        }
    });

    // Top Products
    new Chart(document.getElementById("productChart"), {
        type: 'bar',
        data: {
            labels: document.getElementById('<%=hfProductNames.ClientID%>').value.split(','),
            datasets: [{
                label: "Sold",
                data: document.getElementById('<%=hfProductSales.ClientID%>').value.split(',')
            }]
        }
    });

    // Top Customers
    new Chart(document.getElementById("customerChart"), {
        type: 'bar',
        data: {
            labels: document.getElementById('<%=hfCustomerNames.ClientID%>').value.split(','),
            datasets: [{
                label: "Spent",
                data: document.getElementById('<%=hfCustomerSpending.ClientID%>').value.split(',')
            }]
        }
    });

</script>

</asp:Content>