<%@ Page Title="My Membership Plans" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="ViewMyMembershipPlan.aspx.cs" Inherits="CafeandfoodRestaurant.User.ViewMyMembershipPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        /* Table styling */
        .membership-table { width: 100%; border-collapse: collapse; margin-top: 20px; min-width: 600px; }
        .membership-table th, .membership-table td { border: 1px solid #ddd; padding: 10px; text-align: center; }
        .membership-table th { background-color: #333; color: white; }

        /* Active / Expired styles */
        .active-plan { color: green; font-weight: bold; }
        .expired-plan { color: red; font-weight: bold; }

        /* Responsive wrapper */
        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
            width: 100%;
        }

        /* Smaller screen font adjustment */
        @media (max-width: 768px) {
            .membership-table th, .membership-table td {
                padding: 8px;
                font-size: 14px;
            }
        }
        @media (max-width: 480px) {
            .membership-table th, .membership-table td {
                padding: 6px;
                font-size: 12px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>My Membership Plans</h2>

    <div class="table-responsive">
        <asp:GridView ID="gvMembershipPlans" runat="server" AutoGenerateColumns="False" CssClass="membership-table">
            <Columns>
                <asp:BoundField HeaderText="Membership ID" DataField="MembershipID" />
                <asp:BoundField HeaderText="Amount" DataField="MembershipAmount" DataFormatString="{0:C}" />
                <asp:BoundField HeaderText="Start Date" DataField="StartDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="End Date" DataField="EndDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="Orders Used" DataField="OrdersUsed" />
                <asp:BoundField HeaderText="Max Orders" DataField="MaxOrders" />
                <asp:BoundField HeaderText="Discount %" DataField="DiscountPercent" DataFormatString="{0}%" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsActive")) ? 
                            "<span class='active-plan'>Active</span>" : 
                            "<span class='expired-plan'>Expired</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>