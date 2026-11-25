<%@ Page Title="Edit Membership Plan" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="EditPlan.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.EditPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .edit-card {
            background: white;
            padding: 25px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <h3 class="mb-4">✏ Edit Membership Plan</h3>

        <div class="edit-card">

            <asp:Label ID="lblMsg" runat="server" CssClass="text-success"></asp:Label>

            <div class="row">

                <div class="col-md-6 mb-3">
                    <label>Plan Name</label>
                    <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label>Amount</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label>Max Orders</label>
                    <asp:TextBox ID="txtMaxOrders" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label>Discount (%)</label>
                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label>Duration (Months)</label>
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label>Status</label>

                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>

                </div>

            </div>

            <asp:Button ID="btnUpdate"
                runat="server"
                Text="Update Plan"
                CssClass="btn btn-primary"
                OnClick="btnUpdate_Click" />

        </div>

    </div>

</asp:Content>
