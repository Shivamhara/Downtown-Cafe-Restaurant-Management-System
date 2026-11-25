<%@ Page Title="User List" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .user-card {
            background: #fff;
            padding: 15px;
            border-radius: 10px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            margin-bottom: 15px;
            transition: transform 0.2s;
        }

        .user-card:hover {
            transform: translateY(-5px);
        }

        .user-card h5 {
            margin-bottom: 5px;
            font-weight: 600;
        }

        .user-card small {
            color: #555;
        }

        .user-actions .btn {
            margin-right: 5px;
        }

        @media (max-width: 576px) {
            .user-card {
                padding: 10px;
            }
            .user-actions .btn {
                margin-bottom: 5px;
                font-size: 12px;
                padding: 5px 8px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">
        <h3 class="mb-4">👥 Users List</h3>

        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <div class="user-card row align-items-center">
                    <div class="col-md-3 col-12">
                        <h5><%# Eval("FullName") %></h5>
                        <small>Email: <%# Eval("Email") %></small>
                    </div>
                    <div class="col-md-3 col-12">
                        <small>Registered: <%# Eval("RegistrationDate", "{0:dd/MM/yyyy}") %></small><br />
                        <small>Status: 
                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                        </small>
                    </div>
                    <div class="col-md-3 col-12">
                        <small>Membership: <%# Eval("MembershipStatus") %></small><br />
                        <small>Total Orders: <%# Eval("TotalOrders") %></small>
                    </div>
                    <div class="col-md-3 col-12 text-md-end user-actions mt-2 mt-md-0">
                    <%--    <asp:Button runat="server" CssClass="btn btn-primary btn-sm" Text="Edit"
                            CommandName="EditUser" CommandArgument='<%# Eval("CustomerID") %>' />
                        <asp:Button runat="server" CssClass="btn btn-danger btn-sm" Text="Delete"
                            CommandName="DeleteUser" CommandArgument='<%# Eval("CustomerID") %>'
                            OnClientClick="return confirm('Are you sure you want to delete this user?');" />
   <asp:Button runat="server" CssClass="btn btn-info btn-sm" Text="View"
                            CommandName="ViewUser" CommandArgument='<%# Eval("CustomerID") %>' />--%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
    </div>

</asp:Content>