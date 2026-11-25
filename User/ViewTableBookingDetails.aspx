<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="ViewTableBookingDetails.aspx.cs" Inherits="CafeandfoodRestaurant.User.ViewTableBookingDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Make GridView responsive */
        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch; /* smooth scroll for iOS */
            width: 100%;
            margin-top: 20px;
        }

        .gvMyBookings th, .gvMyBookings td {
            padding: 8px;
            text-align: center;
            min-width: 100px;
        }

        /* Mobile adjustments */
        @media (max-width: 768px) {
            .gvMyBookings th, .gvMyBookings td {
                font-size: 14px;
                padding: 6px;
            }

            .btn {
                font-size: 12px;
                padding: 6px 10px;
            }
        }

        @media (max-width: 480px) {
            .gvMyBookings th, .gvMyBookings td {
                font-size: 12px;
                padding: 4px;
            }

            .btn {
                font-size: 10px;
                padding: 4px 8px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-3">
    <asp:TextBox ID="txtSearchTxn" runat="server" CssClass="form-control"
        Placeholder="Enter Transaction ID"></asp:TextBox>

    <asp:Button ID="btnSearch" runat="server" Text="Search"
        CssClass="btn btn-primary mt-2"
        OnClick="btnSearch_Click" />

    <asp:Button ID="btnReset" runat="server" Text="Reset"
        CssClass="btn btn-secondary mt-2"
        OnClick="btnReset_Click" />
</div>
    
    <h2>My Table Bookings</h2>
  
    <div class="table-responsive">
        <asp:GridView ID="gvMyBookings" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered gvMyBookings"
            OnRowCommand="gvMyBookings_RowCommand">
            <Columns>
                <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="BookingTime" HeaderText="Booked Time" />
                <asp:BoundField DataField="BookingStatus" HeaderText="Status" />
                <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                    
        <asp:Button ID="btnPay" runat="server"
    Text="Pay Now"
    CommandName="Pay"
    CommandArgument='<%# Eval("TransactionID") %>'
    CssClass="btn btn-success"
    UseSubmitBehavior="false"
    Enabled='<%# Eval("PaymentStatus").ToString() != "Paid" && Eval("BookingStatus").ToString() == "Approved" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>