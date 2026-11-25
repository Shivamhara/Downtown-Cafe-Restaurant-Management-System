<%@ Page Title="Admin Table Payment" Language="C#" 
    MasterPageFile="~/Admin/AdminMaster.Master" 
    AutoEventWireup="true" 
    CodeBehind="TablePayment.aspx.cs" 
    Inherits="CafeandfoodRestaurant.Admin.TablePayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
        .table-container {
            background: white;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 6px 15px rgba(0,0,0,0.1);
            margin-top: 20px;
        }
        .btn-update-payment {
            background:#4e73df; color:white; transition:0.3s;
        }
        .btn-update-payment:hover {
            background:#224abe;
        }
        .lbl-message {
            text-align:center; margin-top:10px; font-weight:500;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">
    <h3 class="text-center mb-4">📌 Table Booking & Payment Management</h3>

    <div class="table-container">
        <asp:GridView ID="gvBookings" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-bordered table-hover" OnRowCommand="gvBookings_RowCommand">
            <Columns>
                <asp:BoundField DataField="BookingID" HeaderText="Booking ID" ReadOnly="True" />
                <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                <asp:BoundField DataField="TableType" HeaderText="Table Type" />
                <asp:BoundField DataField="NoOfPersons" HeaderText="No. of Persons" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount (₹)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Paid (₹)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="RemainingAmount" HeaderText="Remaining (₹)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="BookingStatus" HeaderText="Status" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <%-- Button to mark remaining payment as paid --%>
                        <asp:Button ID="btnPayRemaining" runat="server" Text="Mark Payment Paid"
                            CssClass="btn btn-sm btn-update-payment"
                            CommandName="PayRemaining" CommandArgument='<%# Eval("BookingID") %>'
                            Enabled='<%# Convert.ToDecimal(Eval("RemainingAmount")) > 0 %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Label ID="lblMessage" runat="server" CssClass="lbl-message text-success"></asp:Label>
    </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>