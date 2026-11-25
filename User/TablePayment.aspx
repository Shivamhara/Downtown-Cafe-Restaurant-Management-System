<%@ Page Title="Table Payment" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="TablePayment.aspx.cs" Inherits="CafeandfoodRestaurant.User.TablePayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
        .payment-card {
            background: white;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 6px 15px rgba(0,0,0,0.1);
            margin-top: 20px;
        }
        .payment-box {
            padding: 12px;
            border: 1px solid #e5e5e5;
            border-radius: 8px;
            margin-bottom: 10px;
            transition: 0.2s;
            cursor: pointer;
        }
        .payment-box:hover {
            background: #f8f9ff;
            border-color: #198754;
        }
        #cardSection, #upiSection {
            display: none;
            animation: fadeIn 0.3s ease;
            margin-top: 10px;
        }
        @keyframes fadeIn {
            from { opacity:0; transform:translateY(10px);}
            to { opacity:1; transform:translateY(0);}
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container">
    <h3 class="text-center mt-4">💳 Table Payment Gateway</h3>

    <div class="payment-card">
        <label class="fw-bold mt-2">Select Payment Type</label>

<div class="form-check">
    <asp:RadioButton ID="rdoAdvance" runat="server" GroupName="ptype" 
        Text="Advance (50%)" AutoPostBack="true" 
        OnCheckedChanged="PaymentType_Changed" />
</div>

<div class="form-check">
    <asp:RadioButton ID="rdoFull" runat="server" GroupName="ptype" 
        Text="Full Payment (100%)" AutoPostBack="true" 
        OnCheckedChanged="PaymentType_Changed" />
</div>
        <!-- Booking Info -->
        <div class="mb-3 row">
            <div class="col-md-6">
                <label>Transaction ID</label>
                <asp:Label ID="lblTxnID" runat="server" CssClass="form-control bg-light fw-bold"></asp:Label>
            </div>
            <div class="col-md-6">
                <label>Amount (₹)</label>
                <asp:Label ID="lblAmount" runat="server" CssClass="form-control bg-light text-success fw-bold"></asp:Label>
            </div>
        </div>

        <!-- Payment Method -->
        <label class="fw-bold mb-2">Select Payment Method</label>
       <%-- <div class="form-check payment-box">
            <input class="form-check-input paymentRadio" type="radio" name="payment" id="rdoCash" runat="server" ClientIDMode="Static" value="Cash">
            <label class="form-check-label ms-2">💵 Cash on Delivery</label>
        </div>--%>
        <div class="form-check payment-box">
            <input class="form-check-input paymentRadio" type="radio" name="payment" id="rdoUPI" runat="server" ClientIDMode="Static" value="UPI">
            <label class="form-check-label ms-2">📱 Pay via UPI</label>
        </div>
        <div class="form-check payment-box">
            <input class="form-check-input paymentRadio" type="radio" name="payment" id="rdoCard" runat="server" ClientIDMode="Static" value="Card">
            <label class="form-check-label ms-2">💳 Debit / Credit Card</label>
        </div>

        <!-- UPI Section -->
        <div id="upiSection">
            <asp:TextBox ID="txtUPI" runat="server" CssClass="form-control mt-2" Placeholder="example@upi"></asp:TextBox>
        </div>

        <!-- Card Section -->
        <div id="cardSection">
            <div class="mb-2">
                <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control mb-2" Placeholder="Name on Card"></asp:TextBox>
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control mb-2" Placeholder="16-digit Card Number" MaxLength="16"></asp:TextBox>
                <div class="row">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtExpiry" runat="server" CssClass="form-control" Placeholder="MM/YY" MaxLength="5"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" Placeholder="CVV" TextMode="Password" MaxLength="3"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <asp:Button ID="btnPayNow" runat="server" Text="Pay Now" CssClass="btn btn-success w-100 mt-3" OnClick="btnPayNow_Click"/>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2 text-center fw-bold text-success"></asp:Label>

    </div>
</div>

<script>
document.addEventListener("DOMContentLoaded", function() {
    const card = document.getElementById("rdoCard");
    const cash = document.getElementById("rdoCash");
    const upi = document.getElementById("rdoUPI");
    const cardSection = document.getElementById("cardSection");
    const upiSection = document.getElementById("upiSection");

    function toggle() {
        if(card.checked){ cardSection.style.display="block"; upiSection.style.display="none"; }
        else if(upi.checked){ upiSection.style.display="block"; cardSection.style.display="none"; }
        else { cardSection.style.display="none"; upiSection.style.display="none"; }
    }

    card.addEventListener("change", toggle);
    cash.addEventListener("change", toggle);
    upi.addEventListener("change", toggle);
});
</script>
</asp:Content>