<%@ Page Title="View Booking Details" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="ViewBookingDetails.aspx.cs" Inherits="CafeandfoodRestaurant.User.ViewBookingDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2 class="text-center mb-4">My Table Bookings</h2>
        <div class="container mt-4 mb-4 p-3 border rounded bg-light">
    <h4>Edit Booking</h4>

    <asp:HiddenField ID="hfBookingID" runat="server" />

    <div class="mb-3">
        <label class="form-label">Table Type</label>
        <asp:DropDownList ID="ddlTableType" runat="server" CssClass="form-select"></asp:DropDownList>
    </div>

    <div class="mb-3">
        <label class="form-label">Time</label>
        <asp:TextBox ID="txtBookingTime" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label class="form-label">Arrival Date</label>
        <asp:TextBox ID="txtArrivalDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
    </div>

    <asp:Button ID="btnUpdateBooking" runat="server" Text="Update Booking" CssClass="btn btn-primary" OnClick="btnUpdateBooking_Click" />
</div>
        <asp:HiddenField ID="hfDeleteBookingID" runat="server" ClientIDMode="Static" />

<asp:Button ID="btnDeleteBooking" runat="server"
    Style="display:none;"
    ClientIDMode="Static"
    OnClick="btnDeleteBooking_Click" />
        <asp:GridView ID="gvBookings" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
            OnRowCommand="gvBookings_RowCommand">
            <Columns>
                <asp:BoundField DataField="BookingID" HeaderText="Booking ID" />
                <asp:BoundField DataField="TableType" HeaderText="Table Type" />
<%--                <asp:BoundField DataField="TotalAmount" HeaderText="Total Cost" DataFormatString="₹{0:N2}" />--%>
                <asp:BoundField DataField="BookingDate" HeaderText="Arrival Date" DataFormatString="{0:dd MMM yyyy}" />
<asp:BoundField DataField="BookingTime" HeaderText="Time" DataFormatString="{0:hh:mm tt}" /><%--                <asp:BoundField DataField="BookingStatus" HeaderText="Status" />--%>
                <asp:TemplateField HeaderText="Actions">

  <ItemTemplate>

    <!-- EDIT -->
    <asp:Button ID="btnEdit" runat="server"
        CommandName="EditBooking"
        CommandArgument='<%# Eval("BookingID") %>'
        CssClass="btn btn-sm btn-primary"
        Text="Edit" />

    <!-- CANCEL -->
 <asp:Button ID="btnCancel" runat="server"
    CommandName="CancelBooking"
    CommandArgument='<%# Eval("BookingID") %>'
    CssClass="btn btn-sm btn-warning"
    Text="Cancel"
    Visible='<%# Eval("BookingStatus").ToString() == "Pending" %>' />

    <!-- DELETE (only for rejected) -->
  <asp:Button ID="btnDelete" runat="server"
    Text="Delete"
    CommandName="DeleteBooking"
    CommandArgument='<%# Eval("BookingID") %>'
    CssClass="btn btn-danger btn-sm"
    Visible='<%# Eval("BookingStatus").ToString() == "Rejected" || Eval("BookingStatus").ToString() == "Cancelled" %>' />
      <asp:Button ID="btnResetForm" runat="server"
    Text="Reset"
    CssClass="btn btn-secondary"
    OnClick="btnResetForm_Click" />
</ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>