<%@ Page Title="Cart" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="ViewCart.aspx.cs" Inherits="CafeandfoodRestaurant.User.ViewCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

.cart-container{
    padding:40px;
}

.cart-title{
    font-size:28px;
    font-weight:bold;
    margin-bottom:20px;
}

.total-box{
    font-size:20px;
    font-weight:bold;
    margin-top:20px;
    color:#e74c3c;
}

</style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container cart-container">

<div class="cart-title">
Your Cart
</div>

<asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False"
    OnRowCommand="gvCart_RowCommand" CssClass="table table-bordered">

    <Columns>

        <asp:BoundField DataField="MenuName" HeaderText="Menu Item" />

        <asp:BoundField DataField="Cost" HeaderText="Price" />

<asp:TemplateField HeaderText="Quantity">
<ItemTemplate>

<asp:LinkButton ID="btnMinus"
runat="server"
Text="-"
CommandName="DecreaseQty"
CommandArgument='<%# Eval("MenuID") %>'
CssClass="btn btn-sm btn-warning" />

&nbsp;

<asp:Label ID="lblQty"
runat="server"
Text='<%# Eval("Qty") %>' />

&nbsp;

<asp:LinkButton ID="btnPlus"
runat="server"
Text="+"
CommandName="IncreaseQty"
CommandArgument='<%# Eval("MenuID") %>'
CssClass="btn btn-sm btn-success" />

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Total">
<ItemTemplate>
₹ <%# Convert.ToDecimal(Eval("Cost")) * Convert.ToInt32(Eval("Qty")) %>
</ItemTemplate>
</asp:TemplateField>
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:Button ID="btnRemove" runat="server"
                    Text="Remove"
                    CommandName="RemoveItem"
                    CommandArgument='<%# Eval("MenuID") %>'
                    CssClass="btn btn-danger btn-sm" />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

<br />

<br />

<div class="row mt-4">

<div class="col-md-6">
<h4 class="text-danger fw-bold">
Total Amount : ₹ <asp:Label ID="lblTotal" runat="server"></asp:Label>
</h4>
</div>

<div class="col-md-6 text-end">

<asp:Button ID="btnBackToMenu"
runat="server"
Text="➕ Add More Items"
CssClass="btn btn-primary me-2"
PostBackUrl="~/User/ViewMenu.aspx" />

<asp:Button ID="btnPayment"
runat="server"
Text="Proceed To Payment 💳"
CssClass="btn btn-success"
OnClick="btnPayment_Click"/>
</div>

</div>

<br />


</asp:Content>