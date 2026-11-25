<%@ Page Title="Membership Plans" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ViewPlans.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ViewPlans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .plan-card {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            overflow-x: auto; /* allow horizontal scroll for mobile */
        }

        /* Adjust GridView table styling */
        .gvPlans th, .gvPlans td {
            text-align: center;
            padding: 8px;
            min-width: 80px;
        }

        .gvPlans th {
            background-color: #f8f9fa;
        }

        /* Mobile adjustments */
        @media (max-width: 768px) {
            .gvPlans th, .gvPlans td {
                font-size: 14px;
                padding: 6px;
            }

            .btn-sm {
                font-size: 12px;
                padding: 4px 8px;
            }
        }

        @media (max-width: 480px) {
            .gvPlans th, .gvPlans td {
                font-size: 12px;
                padding: 4px;
            }

            .btn-sm {
                font-size: 10px;
                padding: 3px 6px;
            }
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">
          
        <h3 class="mb-4">⭐ Membership Plans</h3>
        <div class="mb-3 d-flex">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control me-2" Placeholder="Search membership plans..." />
            <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            <asp:Button ID="btnReset" runat="server" Text="✖ Reset" CssClass="btn btn-secondary ms-2" OnClick="btnReset_Click" />
        </div>
        <div class="plan-card">

            <div class="table-responsive"> <!-- scrollable container -->
                <asp:GridView ID="gvPlans"
                    runat="server"
                    CssClass="table table-bordered table-hover gvPlans"
                    AutoGenerateColumns="False"
                    OnRowCommand="gvPlans_RowCommand">

                    <Columns>

                        <asp:BoundField DataField="PlanID" HeaderText="Plan ID" />
                        <asp:BoundField DataField="PlanName" HeaderText="Plan Name" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="MaxOrders" HeaderText="Max Orders" />
                        <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" />
                        <asp:BoundField DataField="DurationMonths" HeaderText="Duration (Months)" />
                        <asp:BoundField DataField="IsActive" HeaderText="Active" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button runat="server"
                                    Text="Edit"
                                    CommandName="EditPlan"
                                    CommandArgument='<%# Eval("PlanID") %>'
                                    CssClass="btn btn-primary btn-sm" />

                                <asp:Button runat="server"
                                    Text="Delete"
                                    CommandName="DeletePlan"
                                    CommandArgument='<%# Eval("PlanID") %>'
                                    CssClass="btn btn-danger btn-sm"
                                    OnClientClick="return confirm('Delete this plan?');" />
                          
                           <asp:Button runat="server"
    Text="View Users"
    CommandName="ViewUsers"
    CommandArgument='<%# Eval("Amount") + "|" + Eval("MaxOrders") + "|" + Eval("DiscountPercent") %>'
    CssClass="btn btn-success btn-sm" />
                                  </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>

        </div>

    </div>

</asp:Content>