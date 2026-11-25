<%@ Page Title="Take Action" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TakeAction.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.TakeAction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>

<style>
/* Card style for orders grid */
.order-card{
    border-radius:12px;
    box-shadow:0 4px 15px rgba(0,0,0,0.1);
    padding:20px;
    margin-bottom:20px;
    background:white;
}

/* Status buttons style */
.status-btn{
    margin:3px;
}

/* Tracking buttons style */
.tracking-step{
    font-size:14px;
    padding:12px 20px;
    border-radius:6px;
    margin:3px;
    color:white;
    cursor:pointer;
    transition: transform 0.2s, box-shadow 0.2s;
}

.received{background:#6c757d;}
.process{background:#0d6efd;}
.ready{background:#ffc107;color:black;}
.reached{background:#198754;}

.current-step{
    box-shadow:0 0 12px rgba(0,0,0,0.4);
    transform: scale(1.1);
}

@media (max-width: 767px){
    .tracking-step{
        flex: 1 1 100%;
        text-align:center;
    }
}
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container-fluid mt-4">
    <h3 class="mb-4">🚀 Take Action on Orders</h3>

    <div class="row">
        <!-- Orders List -->
        <div class="col-12 col-lg-6 mb-4">
            <div class="card order-card">
                <h5>📋 Orders List</h5>
                <div class="table-responsive mt-3">
                    <asp:GridView ID="gvOrders" runat="server"
                        CssClass="table table-hover table-bordered"
                        AutoGenerateColumns="False"
                        OnRowCommand="gvOrders_RowCommand">

                        <Columns>
                            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                            <asp:BoundField DataField="FinalAmount" HeaderText="Amount ₹" />
                            <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
                            <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:Button runat="server"
                                        Text="👤 User"
                                        CommandName="UserDetails"
                                        CommandArgument='<%# Eval("CustomerID") %>'
                                        CssClass="btn btn-primary btn-sm status-btn"/>
                                    
                                    <asp:Button runat="server"
                                        Text="🍔 Menu"
                                        CommandName="OrderDetails"
                                        CommandArgument='<%# Eval("OrderID") %>'
                                        CssClass="btn btn-success btn-sm status-btn"/>
                                    
                                    <asp:Button runat="server"
                                        Text="📍 Track"
                                        CommandName="TrackOrder"
                                        CommandArgument='<%# Eval("OrderID") %>'
                                        CssClass="btn btn-warning btn-sm status-btn"/>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Track / Update Status -->
        <div class="col-12 col-lg-6 mb-4">
            <div class="card order-card">
                <h5>🚚 Track / Update Status</h5>
                <asp:Label ID="lblOrderInfo" runat="server" CssClass="mb-3 d-block fw-bold"></asp:Label>
                
                <div class="d-flex flex-wrap mt-3">
                    <asp:Button ID="btnReceived" runat="server" Text="Received" CssClass="tracking-step received btn" OnClick="UpdateStatus_Click" CommandArgument="Received"/>
                    <asp:Button ID="btnProcess" runat="server" Text="Process Started" CssClass="tracking-step process btn" OnClick="UpdateStatus_Click" CommandArgument="Process Started"/>
                    <asp:Button ID="btnReady" runat="server" Text="Ready to Send" CssClass="tracking-step ready btn" OnClick="UpdateStatus_Click" CommandArgument="Ready to Send"/>
                    <asp:Button ID="btnReached" runat="server" Text="Reached" CssClass="tracking-step reached btn" OnClick="UpdateStatus_Click" CommandArgument="Reached"/>
                </div>
            </div>
        </div>
    </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>