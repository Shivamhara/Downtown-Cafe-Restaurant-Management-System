<%@ Page Title="Give Rating" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="GiveRating.aspx.cs" Inherits="CafeandfoodRestaurant.User.GiveRating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.rating-card {
    max-width: 500px;
    margin: auto;
    background: white;
    padding: 25px;
    border-radius: 15px;
    box-shadow: 0 8px 25px rgba(0,0,0,0.15);
}

.star {
    font-size: 25px;
    cursor: pointer;
    color: gray;
}

.star.selected {
    color: gold;
}

.btn-group {
    margin-top: 15px;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-5">
    <div class="rating-card text-center">

        <h4>⭐ Rate This Item</h4>
        <asp:Label ID="lblMenuName" runat="server" CssClass="fw-bold"></asp:Label>
        <hr />

        <!-- Star Rating -->
        <div id="starContainer">
            <span class="star" onclick="setRating(1)">★</span>
            <span class="star" onclick="setRating(2)">★</span>
            <span class="star" onclick="setRating(3)">★</span>
            <span class="star" onclick="setRating(4)">★</span>
            <span class="star" onclick="setRating(5)">★</span>
        </div>

        <asp:HiddenField ID="hfRating" runat="server" />

        <!-- Comment -->
        <div class="mt-3">
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="3"
                CssClass="form-control" Placeholder="Write your feedback..."></asp:TextBox>
        </div>

        <!-- Buttons -->
        <div class="btn-group">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Feedback"
                CssClass="btn btn-success mt-3" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update Feedback"
                CssClass="btn btn-primary mt-3" OnClick="btnUpdate_Click" Visible="false" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete Feedback"
                CssClass="btn btn-danger mt-3" OnClick="btnDelete_Click" Visible="false" />
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-3"></asp:Label>

    </div>
</div>

<script>
    function setRating(rating) {
        document.getElementById('<%= hfRating.ClientID %>').value = rating;

        let stars = document.querySelectorAll('.star');
        stars.forEach((s, index) => {
            if (index < rating) {
                s.classList.add('selected');
            } else {
                s.classList.remove('selected');
            }
        });
    }

    // Load stars on page load if rating exists
    document.addEventListener("DOMContentLoaded", function () {
        let rating = parseInt(document.getElementById('<%= hfRating.ClientID %>').value);
        if (rating > 0) setRating(rating);
    });
</script>
</asp:Content>