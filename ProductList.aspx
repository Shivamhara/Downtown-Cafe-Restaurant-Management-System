<%@ Page Title="Products" Language="C#" MasterPageFile="~/HomemasterPage.Master"
    AutoEventWireup="true" CodeBehind="ProductList.aspx.cs"
    Inherits="CafeandfoodRestaurant.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .menu-container {
            padding: 40px;
        }

        .menu-card {
            border-radius: 10px;
            box-shadow: 0px 4px 10px rgba(0,0,0,0.1);
            padding: 20px;
            margin-bottom: 25px;
            transition: 0.3s;
            background: white;
        }

            .menu-card:hover {
                transform: scale(1.05);
            }

        .menu-name {
            font-size: 20px;
            font-weight: bold;
        }

        .menu-category {
            color: gray;
            font-size: 14px;
        }

        .menu-price {
            color: #e74c3c;
            font-size: 18px;
            font-weight: bold;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    


    <!-- ✅ CATEGORY DROPDOWN -->
<!-- CATEGORY DROPDOWN -->
<div class="mb-4 text-center">
    <asp:DropDownList ID="ddlCategory" runat="server"
        CssClass="form-select w-25 d-inline"
        AutoPostBack="true"
        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
    </asp:DropDownList>
</div>

<!-- ✅ ADD ROW HERE -->
<div class="row menu-container">

<asp:Repeater ID="rptMenu" runat="server" OnItemCommand="rptMenu_ItemCommand">
    <ItemTemplate>

        <!-- 3 COLUMN -->
        <div class="col-md-4">
            <div class="menu-card">

                <img src='<%# ResolveUrl("~/"+ Eval("ImagePath")) %>'
                    style="height:180px; width:100%; object-fit:cover" />

                <div class="menu-name">
                    <%# Eval("MenuName") %>
                </div>

                <div class="menu-category">
                    Description : <%# Eval("Description") %>
                </div>

                <div class="menu-category">
                    Category : <%# Eval("Category") %>
                </div>

                <div class="menu-price">
                    ₹ <%# Eval("Cost") %>
                </div>

                <!-- BUTTONS -->
         <%--       <asp:Button
                    ID="btnCart"
                    runat="server"
                    Text="Add to Cart"
                    CssClass="btn btn-success mt-2"
                    CommandName="addcart"
                    CommandArgument='<%# Eval("MenuID") %>' />

                <asp:Button
                    ID="btnFeedback"
                    runat="server"
                    Text="Give Feedback ⭐"
                    CssClass="btn btn-warning mt-2"
                    PostBackUrl='<%# "GiveRating.aspx?id=" + Eval("MenuID") %>' />--%>

                <!-- FEEDBACK -->
                <asp:Repeater ID="rptFeedback" runat="server">
                    <ItemTemplate>
                        <div class="border rounded p-2 mt-2" style="background:#f9f9f9;">
                            <b><%# Eval("UserName") %></b>
                            <span style="color:gold;">⭐ <%# Eval("Rating") %></span>
                            <p><%# Eval("Comment") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label ID="lblNoFeedback" runat="server"
                    CssClass="text-muted"
                    Visible="false"
                    Text="No feedback yet."></asp:Label>

            </div>
        </div>

    </ItemTemplate>
</asp:Repeater>

</div> <!-- ✅ CLOSE ROW -->             
 
   
<asp:Label ID="Label1" runat="server" CssClass="text-danger"></asp:Label>
 

</asp:Content>
