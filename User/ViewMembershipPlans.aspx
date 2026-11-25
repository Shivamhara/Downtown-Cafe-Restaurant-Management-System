<%@ Page Title="Membership Plans" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="ViewMembershipPlans.aspx.cs" Inherits="CafeandfoodRestaurant.User.ViewMembershipPlans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .plan-card {
            background: white;
            border-radius: 12px;
            padding: 25px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            text-align: center;
            transition: 0.3s;
        }

            .plan-card:hover {
                transform: translateY(-5px);
            }

        .plan-name {
            font-size: 22px;
            font-weight: 600;
        }

        .plan-price {
            font-size: 32px;
            color: #28a745;
            font-weight: 700;
        }

        .plan-feature {
            margin-bottom: 6px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">

        <h3 class="text-center mb-4">⭐ Choose Your Membership</h3>

        <div class="row">

            <asp:Repeater ID="rptPlans" runat="server" OnItemCommand="rptPlans_ItemCommand">

                <ItemTemplate>

                    <div class="col-md-4 mb-4">

                        <div class="plan-card">

                            <div class="plan-name">
                                <%# Eval("PlanName") %>
                            </div>

                            <div class="plan-price">
                                ₹ <%# Eval("Amount") %>
                            </div>

                            <hr />

                            <div class="plan-feature">
                                ✔ Discount: <%# Eval("DiscountPercent") %>%
                            </div>

                            <div class="plan-feature">
                                ✔ Max Orders: <%# Eval("MaxOrders") %>
                            </div>

                            <div class="plan-feature">
                                ✔ Duration: <%# Eval("DurationMonths") %> Months
                            </div>

                            <hr />

                            <asp:Button runat="server"
                                Text="Join Plan"
                                CommandName="JoinPlan"
                                CommandArgument='<%# Eval("PlanID") %>'
                                CssClass="btn btn-success w-100" />

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-success text-center d-block mt-3"></asp:Label>

    </div>

</asp:Content>
