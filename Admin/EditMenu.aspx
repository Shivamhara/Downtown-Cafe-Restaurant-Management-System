<%@ Page Title="Edit Menu" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="EditMenu.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.EditMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <div class="card p-4">

            <h3>Edit Menu</h3>

            <asp:HiddenField ID="hfMenuID" runat="server" />

            <div class="mb-3">
                <label>Menu Name</label>
                <asp:TextBox ID="txtMenuName" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label>Cost</label>
                <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" />
            </div>
              <div class="mb-3">
                <label>Description</label>
                <asp:TextBox ID="txtdetails" runat="server" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label>Category</label>

                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">

                    <asp:ListItem>Starter</asp:ListItem>
                    <asp:ListItem>Main Course</asp:ListItem>
                    <asp:ListItem>Dessert</asp:ListItem>
                    <asp:ListItem>Beverage</asp:ListItem>

                </asp:DropDownList>

            </div>
            <div class="mb-3">
    <label>Availability</label>

    <asp:RadioButtonList ID="rblIsAvailable" runat="server" CssClass="form-control">
        <asp:ListItem Text="Available" Value="1" />
        <asp:ListItem Text="Not Available" Value="0" />
    </asp:RadioButtonList>
</div>
            <div class="mb-3">

                <img id="imgPreview" runat="server" style="height: 150px" />

            </div>

            <div class="mb-3">

                <asp:FileUpload ID="fuImage" runat="server" />

            </div>

            <asp:Button ID="btnUpdate"
                runat="server"
                Text="Update Menu"
                CssClass="btn btn-success"
                OnClick="btnUpdate_Click" />

        </div>

    </div>

</asp:Content>
