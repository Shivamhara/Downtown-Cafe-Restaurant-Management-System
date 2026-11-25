<%@ Page Title="Add Categories" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master"
    AutoEventWireup="true" CodeBehind="AddFoodCategories.aspx.cs"
    Inherits="CafeandfoodRestaurant.Admin.AddFoodCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">

    <h3 class="text-center mb-4">Add Food Category</h3>

    <div class="card p-4">

        <!-- Category Name -->
        <div class="mb-3">
            <label>Category Name</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" />
        </div>

        <!-- Description -->
        <div class="mb-3">
            <label>Description</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" />
        </div>

        <!-- Active -->
        <div class="mb-3">
            <asp:CheckBox ID="chkActive" runat="server" Text="Is Active" Checked="true" />
        </div>
       <div class="mb-3">
    <label>Category Image</label>
    <asp:FileUpload ID="fuCategoryImage" runat="server" CssClass="form-control" onchange="previewImage(this)" />
</div>

<!-- ✅ IMAGE PREVIEW SECTION -->
<div class="row text-center mt-3">

    <!-- Current Image -->
    <div class="col-md-6">
        <label>Current Image</label><br />
        <asp:Image ID="imgCurrent" runat="server" Width="120" Height="120"
            Style="border-radius:10px; border:1px solid #ccc;" />
    </div>

    <!-- New Image Preview -->
    <div class="col-md-6">
        <label>New Image Preview</label><br />
        <img id="imgPreview" src="#" style="display:none; width:120px; height:120px; border-radius:10px; border:1px solid #ccc;" />
    </div>

</div>
        <!-- Buttons -->
        <asp:Button ID="btnAdd" runat="server" Text="Add Category"
            CssClass="btn btn-success" OnClick="btnAdd_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="text-success mt-3 d-block"></asp:Label>

    </div>

    <hr />

    <!-- Show Categories -->
    <h4>All Categories</h4>
    <asp:HiddenField ID="hfCategoryID" runat="server" />
   <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False"
    CssClass="table table-bordered mt-3"
    OnRowCommand="gvCategories_RowCommand">

    <Columns>
        <asp:BoundField DataField="CategoryID" HeaderText="ID" />
        <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:BoundField DataField="IsActive" HeaderText="Active" />

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>

                <asp:Button ID="btnEdit" runat="server"
                    Text="Edit"
                    CssClass="btn btn-warning btn-sm"
                    CommandName="EditCat"
                    CommandArgument='<%# Eval("CategoryID") %>' />

                <asp:Button ID="btnDelete" runat="server"
                    Text="Delete"
                    CssClass="btn btn-danger btn-sm"
                    CommandName="DeleteCat"
                    CommandArgument='<%# Eval("CategoryID") %>'
                    OnClientClick="return confirm('Are you sure you want to delete this category?');" />
                    
                <asp:Button ID="btnclear" runat="server"
                    Text="Reset"
                    CommandArgument='<%# Eval("CategoryID") %>'
                    CssClass="btn btn-secondary btn-sm"
                    CommandName="clear" />

            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

</div>
    <script>
    function previewImage(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                var img = document.getElementById("imgPreview");
                img.src = e.target.result;
                img.style.display = "block";
            };

            reader.readAsDataURL(input.files[0]);
        }
    }
    </script>
</asp:Content>