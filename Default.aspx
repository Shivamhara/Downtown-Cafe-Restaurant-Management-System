<%@ Page Title="Categories" Language="C#" MasterPageFile="~/HomemasterPage.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="CafeandfoodRestaurant.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
<style>
    .category-wrapper {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    gap: 40px;
}

.category-item {
    text-align: center;
}

.category-item a {
    text-decoration: none;
    color: black;
}

.circle-img {
    width: 140px;
    height: 140px;
    border-radius: 50%;
    overflow: hidden;
    margin: auto;
    border: 3px solid #333;
    box-shadow: 0 5px 15px rgba(0,0,0,0.3);
    transition: 0.3s;
}

.circle-img img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.circle-img:hover {
    transform: scale(1.1);
}

.category-item h5 {
    margin-top: 10px;
    font-weight: 600;
}
.category-card {
    border-radius: 15px;
    padding: 25px;
    text-align: center;
    background: #fff8f0;
    box-shadow: 0 5px 20px rgba(0,0,0,0.2);
    transition: 0.3s;
}

.category-card:hover {
    transform: scale(1.05);
    background: #8b4513;
    color: white;
}

.category-card i {
    font-size: 40px;
    margin-bottom: 10px;
}

.category-link {
    text-decoration: none;
    color: inherit;
}

</style>

<!-- 🔥 CATEGORY ROUND UI -->
<h2 class="section-title mt-5 text-center">👨‍🍳 Handcrafted Curations 👨‍🍳</h2>

<div class="category-wrapper">

    <asp:Repeater ID="rptCategories" runat="server">
        <ItemTemplate>

            <div class="category-item">

            <a href='ProductList.aspx?CategoryName=<%# Eval("CategoryName") %>'>

                   <!-- ✅ IMAGE ADDED -->
                <div class="circle-img">
               <img src='<%# string.IsNullOrEmpty(Eval("ImagePath").ToString()) 
    ? "CategoryImg/default.jpg" 
    : (Eval("ImagePath").ToString().Contains("/") 
        ? ResolveUrl(Eval("ImagePath").ToString()) 
        : ResolveUrl("CategoryImg/" + Eval("ImagePath").ToString())) %>' />
                </div>


                    <h5><%# Eval("CategoryName") %></h5>

                </a>

            </div>

        </ItemTemplate>
    </asp:Repeater>

</div>
</asp:Content>