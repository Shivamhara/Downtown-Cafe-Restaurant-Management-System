<%@ Page Title="View Feedback" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master"
    AutoEventWireup="true" CodeBehind="ViewFeedback.aspx.cs"
    Inherits="CafeandfoodRestaurant.Admin.ViewFeedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <h3 class="text-center mb-4">Customer Feedback</h3>

    <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped">

        <Columns>
            <asp:BoundField DataField="FullName" HeaderText="Customer" />
            <asp:BoundField DataField="Rating" HeaderText="Rating" />
            <asp:BoundField DataField="Comment" HeaderText="Comment" />
            <asp:BoundField DataField="RatingDate" HeaderText="Date"
                DataFormatString="{0:dd/MM/yyyy}" />
        </Columns>

    </asp:GridView>

</div>

</asp:Content>