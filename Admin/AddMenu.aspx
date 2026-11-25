<%@ Page Title="Manage Menu" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="AddMenu.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.AddMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .menu-container {
            max-width: 1200px;
            margin: auto;
            margin-top: 40px;
        }

        .card-box {
            background: white;
            border-radius: 12px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.08);
            padding: 25px;
        }

        .page-title {
            color: #ff6f61;
            font-weight: 600;
        }

        .menu-card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
            padding: 15px;
            transition: 0.3s;
        }

            .menu-card:hover {
                transform: translateY(-5px);
            }

        .menu-img {
            width: 100%;
            height: 180px;
            object-fit: cover;
            border-radius: 10px;
        }

        .btn-custom {
            background: #ff6f61;
            color: white;
            border: none;
        }

            .btn-custom:hover {
                background: #ff3b2e;
                color: white;
            }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="menu-container">

        <div class="card-box">

            <h3 class="text-center page-title mb-4">Manage Menu</h3>

            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>

            <!-- ADD MENU -->

            <div class="row g-3 mb-3">

                <div class="col-md-3">
                    <asp:TextBox ID="txtMenuName" runat="server" CssClass="form-control" Placeholder="Menu Name"></asp:TextBox>
                </div>

                <div class="col-md-2">
                    <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" Placeholder="Cost"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtdetails" runat="server" CssClass="form-control" Placeholder="Description"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Select Category" Value="" />
                        <asp:ListItem Text="Starter" Value="Starter" />
                        <asp:ListItem Text="Main Course" Value="Main Course" />
                        <asp:ListItem Text="Dessert" Value="Dessert" />
                        <asp:ListItem Text="Beverage" Value="Beverage" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-2">
                    <asp:Button ID="btnAddMenu" runat="server"
                        Text="Add Menu"
                        CssClass="btn btn-custom w-100"
                        OnClick="btnAddMenu_Click" />
                </div>

            </div>


            <!-- SEARCH -->

<%--            <div class="row mb-4">

                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="form-control"
                        Placeholder="Search menu..."></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-control">
                        <asp:ListItem Text="All Category" Value="" />
                        <asp:ListItem Text="Starter" Value="Starter" />
                        <asp:ListItem Text="Main Course" Value="Main Course" />
                        <asp:ListItem Text="Dessert" Value="Dessert" />
                        <asp:ListItem Text="Beverage" Value="Beverage" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:Button ID="btnSearch"
                        runat="server"
                        Text="Search"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnSearch_Click" />
                </div>

            </div>--%>


            <!-- MENU CARDS -->

       <%--     <div class="row">

                <asp:Repeater ID="rptMenu" runat="server">

                    <ItemTemplate>

                        <div class="col-md-4 mb-4">

                            <div class="menu-card text-center">

<img src='<%# ResolveUrl("~/"+ Eval("ImagePath")) %>' class="menu-img" />
                                <h5 class="mt-3"><%# Eval("MenuName") %></h5>

                                <p>
                                    <b>Cost :</b> ₹ <%# Eval("Cost") %>
                                </p>

                                <p>
                                    <b>Category :</b> <%# Eval("Category") %>
                                </p>

                                <div class="d-flex justify-content-between">

                                    <asp:Button ID="btnEdit"
                                        runat="server"
                                        Text="Edit"
                                        CssClass="btn btn-warning btn-sm"
                                        CommandArgument='<%# Eval("MenuID") %>'
                                        OnCommand="EditMenu" />

                                    <asp:Button ID="btnDelete"
                                        runat="server"
                                        Text="Delete"
                                        CssClass="btn btn-danger btn-sm"
                                        CommandArgument='<%# Eval("MenuID") %>'
                                        OnCommand="DeleteMenu" />

                                </div>

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>--%>

            </div>

        </div>

    </div>

</asp:Content>
