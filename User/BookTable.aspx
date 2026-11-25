<%@ Page Title="Book Table" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="BookTable.aspx.cs" Inherits="CafeandfoodRestaurant.User.BookTable" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
body {
    background: #f8f9fa;
    font-family: 'Segoe UI', sans-serif;
}

/* TITLE */
h3 {
    text-align: center;
    margin-bottom: 25px;
    font-weight: 600;
    color: #343a40;
}

/* CARD */
.booking-card {
    background: white;
    padding: 25px;
    border-radius: 14px;
    box-shadow: 0 6px 18px rgba(0,0,0,0.08);
    margin-bottom: 20px;
}

/* LABEL */
.form-label {
    font-weight: 500;
    margin-bottom: 6px;
}

/* INPUT */
.form-control,
.form-select {
    border-radius: 8px;
    padding: 10px;
}

/* BUTTON */
.btn-book {
    background: #4e73df;
    color: white;
    width: 100%;
    font-weight: 600;
    border-radius: 8px;
    padding: 12px;
}

.btn-book:hover {
    background: #224abe;
}

/* VIEW BUTTON */
.btn-view {
    width: 100%;
    border-radius: 8px;
    padding: 12px;
}

/* MESSAGE */
.lbl-message {
    text-align: center;
    margin-bottom: 10px;
    font-weight: 500;
}

/* COST */
.lbl-cost {
    font-weight: 600;
    color: #198754;
    margin-bottom: 10px;
}

/* 📱 MOBILE OPTIMIZATION */
@media (max-width: 768px) {

    .container {
        padding: 10px;
    }

    .booking-card {
        padding: 18px;
    }

    h3 {
        font-size: 20px;
    }

    /* Stack buttons */
    .btn-group-custom {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    /* Better spacing */
    .mb-3 {
        margin-bottom: 15px !important;
    }
}    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">
    <h3>📌 Book Your Table</h3>

    <div class="booking-card">
        <asp:Label ID="Label1" runat="server" CssClass="lbl-message text-danger"></asp:Label>

        <!-- Hidden field to store BookingID -->
        <asp:HiddenField ID="hfBookingID" runat="server" />

        <!-- Table Type Selection -->
        <div class="mb-3">
            <label class="form-label">Table Type</label>
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="ddlTableType_SelectedIndexChanged">
                <asp:ListItem Text="-- Select Table Type --" Value="0"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Total Cost Display -->
        <div class="mb-3">
            <asp:Label ID="Label2" runat="server" CssClass="lbl-cost"></asp:Label>
        </div>

        <!-- Number of Persons -->
        <div class="mb-3">
            <label class="form-label">Time</label>
            <asp:TextBox ID="txttime" runat="server" CssClass="form-control" Placeholder="Enter Time with Am,PM"></asp:TextBox>
        </div>

        <!-- Arrival Date & Time -->
        <div class="mb-3">
            <label class="form-label">Arrival Date </label>
            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>

        <!-- Payment Option (hidden until admin approval) -->
        <asp:Panel ID="Panel1" runat="server" Visible="false">
            <div class="mb-3">
                <label class="form-label">Payment Option</label>
                <asp:RadioButtonList ID="RadioButtonList1"
                    runat="server"
                    CssClass="form-check form-check-inline"
                    AutoPostBack="True"
                    OnSelectedIndexChanged="rblPayment_SelectedIndexChanged">
                    <asp:ListItem Text="Full Payment" Value="Full" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Advance Payment (50%)" Value="Advance"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </asp:Panel>

        <!-- Buttons -->
        <div class="btn-group">
            <asp:Button ID="Button1" runat="server" Text="Book Table" CssClass="btn btn-book mt-2" OnClick="btnBookTable_Click" />
<div class="mt-4 text-center">
    <asp:Button ID="btnViewBookings" runat="server" Text="📋 View My Bookings" CssClass="btn btn-info btn-lg" OnClick="btnViewBookings_Click" />
</div>
<%--            <asp:Button ID="Button2" runat="server" Text="Update Booking" CssClass="btn btn-primary mt-2" OnClick="btnUpdate_Click" Visible="false" />
            <asp:Button ID="Button3" runat="server" Text="Cancel Booking" CssClass="btn btn-danger mt-2" OnClientClick="return confirm('Are you sure you want to cancel this booking?');" OnClick="btnDelete_Click" Visible="false" />--%>
        </div>
    </div>
</div>
    <h3 class="mt-5">📋 My Bookings</h3>
<div class="row" id="bookingContainer" runat="server">
    <!-- Existing bookings will be added here dynamically -->
</div>
</asp:Content>