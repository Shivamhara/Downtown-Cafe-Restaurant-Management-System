<%@ Page Title="View Orders" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ViewOrders.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ViewOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .order-table {
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }

        .btn-action {
            margin: 2px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid mt-4">

        <h3 class="mb-4">📦 Restaurant Orders</h3>
        <div class="mb-3 d-flex">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control me-2" Placeholder="Search orders..." />
    <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    <asp:Button ID="btnClear" runat="server" Text="✖ Clear" CssClass="btn btn-secondary ms-2" OnClick="btnClear_Click" />
</div>
        <div class="table-responsive order-table">

            <asp:GridView ID="gvOrders" runat="server"
                CssClass="table table-hover"
                AutoGenerateColumns="False"
                OnRowCommand="gvOrders_RowCommand">

                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" />
                    <asp:BoundField DataField="FinalAmount" HeaderText="Amount ₹" />
                    <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
                    <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />

                    <asp:TemplateField HeaderText="User Info">
                        <ItemTemplate>
                            <asp:Button runat="server"
                                Text="👤 User"
                                CommandName="UserDetails"
                                CommandArgument='<%# Eval("CustomerID") %>'
                                CssClass="btn btn-primary btn-sm btn-action" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Order Menu">
                        <ItemTemplate>
                            <asp:Button runat="server"
                                Text="🍔 Menu"
                                CommandName="OrderDetails"
                                CommandArgument='<%# Eval("OrderID") %>'
                                CssClass="btn btn-success btn-sm btn-action" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Track Order">
                        <ItemTemplate>
                            <asp:Button runat="server"
                                Text="📍 Track"
                                CommandName="TrackOrder"
                                CommandArgument='<%# Eval("OrderID") %>'
                                CssClass="btn btn-warning btn-sm btn-action" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>

                            <asp:Button runat="server"
                                Text="🗑 Delete"
                                CommandName="DeleteOrder"
                                CommandArgument='<%# Eval("OrderID") %>'
                                CssClass="btn btn-danger btn-sm btn-action"
                                OnClientClick="return confirm('Are you sure you want to delete this order?');" />

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

    </div>


    <!-- USER DETAILS MODAL -->

    <div class="modal fade" id="userModal">

        <div class="modal-dialog modal-lg">

            <div class="modal-content">

                <div class="modal-header bg-primary text-white">

                    <h5>User Details</h5>

                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>

                </div>

                <div class="modal-body">

                    <asp:Label ID="lblUserDetails" runat="server"></asp:Label>

                </div>

            </div>

        </div>

    </div>


    <!-- ORDER DETAILS MODAL -->

    <div class="modal fade" id="orderModal">

        <div class="modal-dialog modal-lg">

            <div class="modal-content">

                <div class="modal-header bg-success text-white">

                    <h5>Order Menu Details</h5>

                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>

                </div>

                <div class="modal-body">

                    <asp:GridView ID="gvOrderDetails"
                        runat="server"
                        CssClass="table table-bordered"
                        AutoGenerateColumns="true" />

                </div>

            </div>

        </div>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>
