<%@ Page Title="View Menu" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ViewMenu.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ViewMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style>
    .menu-card {
        border-radius: 15px;
        overflow: hidden;
        transition: 0.3s;
        box-shadow: 0 4px 12px rgba(0,0,0,0.08);
        background: #fff;
    }

    .menu-card:hover {
        transform: translateY(-8px);
        box-shadow: 0 10px 25px rgba(0,0,0,0.15);
    }

    .menu-img {
        height: 180px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #366a9e;
    }

    .menu-img img {
        max-height: 100%;
        max-width: 100%;
        object-fit: contain;
        transition: 0.3s;
    }

    .menu-card:hover img {
        transform: scale(1.05);
    }

    .menu-body {
        padding: 15px;
        text-align: left;
    }

    .menu-title {
        font-weight: 600;
        font-size: 18px;
        margin-bottom: 5px;
    }

    .menu-desc {
        font-size: 13px;
        color: #6b7280;
        height: 40px;
        overflow: hidden;
    }

    .menu-footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-top: 10px;
    }

    .price {
        font-weight: bold;
        color: #16a34a;
        font-size: 16px;
    }

    .category-badge {
        background: #e0f2fe;
        color: #0284c7;
        padding: 3px 8px;
        border-radius: 8px;
        font-size: 12px;
    }

    .action-btns a {
        margin-right: 5px;
    }

    .filter-box {
        background: white;
        padding: 15px;
        border-radius: 10px;
        box-shadow: 0 3px 10px rgba(0,0,0,0.08);
    }
</style>


<div class="container mt-4">

    <h3 class="text-center mb-4 fw-bold">🍽️ Menu Management</h3>

    <!-- FILTER BOX -->
    <div class="row mb-4 justify-content-center">
        <div class="col-md-4">
            <div class="filter-box">
                <asp:DropDownList ID="ddlCategory" runat="server"
                    AutoPostBack="true"
                    CssClass="form-select"
                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="row">

        <asp:Repeater ID="rptMenu" runat="server">
            <ItemTemplate>

                <div class="col-md-4 mb-4">

                    <div class="menu-card">

                        <!-- IMAGE -->
                        <div class="menu-img">
                            <img src='<%# ResolveUrl("~/"+ Eval("ImagePath")) %>' />
                        </div>

                        <!-- BODY -->
                        <div class="menu-body">

                            <div class="menu-title">
                                <%# Eval("MenuName") %>
                            </div>

                            <div class="menu-desc">
                                <%# Eval("Description") %>
                            </div>

                            <div class="menu-footer">

                                <span class="price">₹ <%# Eval("Cost") %></span>

                                <span class="category-badge">
                                    <%# Eval("Category") %>
                                </span>

                            </div>

                            <!-- BUTTONS -->
                            <div class="action-btns mt-3">

                                <a href='EditMenu.aspx?id=<%# Eval("MenuID") %>'
                                    class="btn btn-warning btn-sm">
                                    <i class="fa fa-edit"></i>
                                </a>

                                <a href='ViewFeedback.aspx?menuid=<%# Eval("MenuID") %>'
                                    class="btn btn-info btn-sm">
                                    <i class="fa fa-comments"></i>
                                </a>

                            </div>

                        </div>

                    </div>

                </div>

            </ItemTemplate>
        </asp:Repeater>

    </div>

</div>
</asp:Content>
