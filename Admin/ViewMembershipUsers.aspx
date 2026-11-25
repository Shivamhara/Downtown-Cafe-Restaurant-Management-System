<%@ Page Title="Membership Users" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ViewMembershipUsers.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.ViewMembershipUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .card-container {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }

        .btn-send {
            width: 150px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4">💳 Users with Active Membership</h3>

        <div class="card-container">
            <asp:GridView ID="gvMembershipUsers" runat="server"
                CssClass="table table-bordered table-hover"
                AutoGenerateColumns="False"
                OnRowCommand="gvMembershipUsers_RowCommand">
                
                <Columns>
                    <asp:BoundField DataField="MembershipID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="User Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="DiscountPercent" HeaderText="Discount %" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button runat="server"
                                Text="Send Expiry Mail"
                                CommandName="SendExpiryMail"
                                CommandArgument='<%# Eval("MembershipID") %>'
                                CssClass="btn btn-warning btn-sm btn-send" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
            <asp:Label ID="lblMsg" runat="server" CssClass="text-success mt-3"></asp:Label>
        </div>
    </div>
</asp:Content>