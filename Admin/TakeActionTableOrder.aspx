<%@ Page Title="Take Action on Table Orders" MasterPageFile="~/Admin/AdminMaster.Master"  Language="C#" AutoEventWireup="true" CodeFile="TakeActionTableOrder.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.TakeActionTableOrder" %>
<%--<%@ Register Assembly="System.Web" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
        <div class="container mt-5">
             <div class="mb-3 d-flex">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control me-2" Placeholder="Search table bookings..." />
    <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    <asp:Button ID="btnReset" runat="server" Text="✖ Reset" CssClass="btn btn-secondary ms-2" OnClick="btnReset_Click" />
</div>
            <h2>Table Booking Management</h2>
            <asp:GridView ID="gvBookings" runat="server" CssClass="table table-bordered table-hover"
                AutoGenerateColumns="False" DataKeyNames="TransactionID"
                OnRowDataBound="gvBookings_RowDataBound">
                
                <Columns>
                    <asp:BoundField HeaderText="Transaction ID" DataField="TransactionID" />
                    <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" />
                    <asp:BoundField HeaderText="Booked Date" DataField="BookingDate" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField HeaderText="Booked Time" DataField="BookingTime" />
                    <asp:BoundField HeaderText="Booking Status" DataField="BookingStatus" />
                    <asp:BoundField HeaderText="Remaining Amount" DataField="RemainingAmount" DataFormatString="₹{0}" />
                    <asp:BoundField HeaderText="Payment Status" DataField="PaymentStatus" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success btn-sm me-1"
                                OnClick="btnApprove_Click" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm me-1"
                                OnClick="btnReject_Click" />
                            <asp:Button ID="btnPayRemaining" runat="server" Text="Pay Remaining" CssClass="btn btn-primary btn-sm me-1"
                                OnClick="btnPayRemaining_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-warning btn-sm"
                                OnClick="btnDelete_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>