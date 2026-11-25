<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="Veg.aspx.cs" Inherits="CafeandfoodRestaurant.User.Veg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
           <style>
        /* Container & spacing */
        .menu-container {
            display: flex;
            flex-wrap: wrap;
            gap: 1rem;
            justify-content: center;
        }

        /* Card styling */
        .menu-card {
            width: 18rem;
            transition: transform 0.3s, box-shadow 0.3s;
        }
        .menu-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.2);
        }

        .menu-card img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-top-left-radius: .25rem;
            border-top-right-radius: .25rem;
        }

        /* Feedback styling */
        .feedback-item {
            background: #f8f9fa;
            padding: .5rem;
            border-radius: .25rem;
            margin-bottom: .5rem;
        }

        .feedback-rating {
            color: gold;
            font-weight: bold;
            margin-left: 5px;
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .menu-card {
                width: 100%;
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-4">
    <h3 class="mb-4">Veg Menu</h3>

    <asp:Label ID="lblNoItems" runat="server" CssClass="text-danger"></asp:Label>

<asp:Repeater ID="repeaterMenu" runat="server" OnItemCommand="rptMenu_ItemCommand">
    <ItemTemplate>
        <div class="card mb-3" style="width: 18rem; display:inline-block; margin-right:10px;">
            <img src='<%# ResolveUrl("~/" + Eval("Image")) %>' 
                 class="card-img-top" style="width:100%; height:200px; object-fit:cover;" 
                 alt='<%# Eval("MenuName") %>' />
            <div class="card-body">
                <h5 class="card-title"><%# Eval("MenuName") %></h5>
                <p class="card-text"><%# Eval("Description") %></p>
                <p class="card-text"><b>Price:</b> ₹<%# Eval("Price") %></p>
            </div>
<%--                <asp:Button ID="btnGiveFeedback" runat="server" CssClass="btn btn-primary btn-sm mt-2" Visible="false"
                                Text="Give Feedback" OnClick="btnGiveFeedback_Click" CommandArgument='<%# Eval("MenuID") %>' />--%>
            <asp:Button
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
                                PostBackUrl='<%# "GiveRating.aspx?id=" + Eval("MenuID") %>' />
                  
            <!-- Feedback Repeater -->
<asp:Repeater ID="rptFeedback" runat="server">
                <ItemTemplate>
                    <div class="border rounded p-2 mb-2" style="background:#f9f9f9;">
                        <b><%# Eval("UserName") %></b>
                        <span style="color:gold;">⭐ <%# Eval("Rating") %></span>
                        <p><%# Eval("Comment") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="lblNoFeedback" runat="server" CssClass="text-muted" Visible="false" 
                       Text="No feedback yet."></asp:Label>
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:Label ID="Label1" runat="server" CssClass="text-danger"></asp:Label></div>
</asp:Content>