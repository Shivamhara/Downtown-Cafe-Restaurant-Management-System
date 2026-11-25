<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="CafeandfoodRestaurant.User.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-7 col-md-10">

                <div class="card shadow-lg border-0 rounded-4 p-4">

                    <h4 class="text-center fw-bold text-success mb-4">💳 Restaurant Payment Gateway
                    </h4>

                    <!-- ORDER INFO -->

                    <div class="row mb-4">

                        <div class="col-md-6">
                            <label class="fw-semibold">Order ID</label>
                            <asp:Label ID="lblOrderID" runat="server"
                                CssClass="form-control bg-light fw-bold"></asp:Label>
                        </div>

                        <div class="col-md-6">
                            <label class="fw-semibold">Total Amount (₹)</label>
                            <asp:Label ID="lblAmount" runat="server"
                                CssClass="form-control bg-light text-success fw-bold"></asp:Label>
                        </div>

                    </div>

                    <!-- PAYMENT METHOD -->

                    <div class="mb-4">

                        <label class="fw-bold mb-3">Select Payment Method</label>

                       <%-- <div class="form-check payment-box">
                            <input class="form-check-input paymentRadio" type="radio"
                                name="payment" id="rdoCash" runat="server"
                                clientidmode="Static" value="Cash">

                           <%-- <label class="form-check-label ms-2">
                                💵 Cash on Delivery
                            </label>--%>
                        </div>

                        <div class="form-check payment-box">
                            <input class="form-check-input paymentRadio" type="radio"
                                name="payment" id="rdoUPI" runat="server"
                                clientidmode="Static" value="UPI">

                            <label class="form-check-label ms-2">
                                📱 Pay via UPI
                            </label>
                        </div>

                        <div class="form-check payment-box">
                            <input class="form-check-input paymentRadio" type="radio"
                                name="payment" id="rdoCard" runat="server"
                                clientidmode="Static" value="Card">

                            <label class="form-check-label ms-2">
                                💳 Debit / Credit Card
                            </label>
                        </div>

                    </div>

                    <!-- UPI SECTION -->

                    <div id="upiSection" style="display: none">

                        <div class="card p-3 border-0 shadow-sm rounded-3 mb-3">

                            <h6 class="fw-bold text-secondary mb-3">📱 Enter UPI ID
                            </h6>

                            <asp:TextBox ID="txtUPI"
                                runat="server"
                                CssClass="form-control"
                                placeholder="example@upi"></asp:TextBox>

                        </div>

                    </div>

                    <!-- CARD SECTION -->

                    <div id="cardSection" style="display: none">

                        <div class="card border-0 shadow-sm p-3 rounded-3 mb-3">

                            <h6 class="fw-bold text-secondary mb-3">💳 Card Details
                            </h6>

                            <div class="mb-3">
                                <label class="form-label">Cardholder Name</label>

                                <asp:TextBox ID="txtCardName"
                                    runat="server"
                                    CssClass="form-control"
                                    placeholder="Name on Card"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Card Number</label>

                                <asp:TextBox ID="txtCardNumber"
                                    runat="server"
                                    CssClass="form-control"
                                    MaxLength="16"
                                    placeholder="Enter 16 digit card"></asp:TextBox>
                            </div>

                            <div class="row">

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Expiry</label>

                                    <asp:TextBox ID="txtExpiry"
                                        runat="server"
                                        CssClass="form-control"
                                        MaxLength="5"
                                        placeholder="MM/YY"></asp:TextBox>
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">CVV</label>

                                    <asp:TextBox ID="txtCVV"
                                        runat="server"
                                        CssClass="form-control"
                                        TextMode="Password"
                                        MaxLength="3"
                                        placeholder="CVV"></asp:TextBox>
                                </div>

                            </div>

                        </div>

                    </div>

                    <!-- PAY BUTTON -->

                    <asp:Button ID="btnRegister"
                        runat="server"
                        Text="Pay Now"
                        CssClass="btn btn-success w-100 fw-semibold py-2 rounded-3"
                        OnClick="btnPayment_Click" />

                    <asp:Label ID="lblMessage"
                        runat="server"
                        CssClass="d-block mt-3 fw-semibold text-center text-success"></asp:Label>

                </div>
            </div>
        </div>
    </div>

    <!-- STYLE -->

    <style>
        .payment-box {
            padding: 12px;
            border: 1px solid #e5e5e5;
            border-radius: 8px;
            margin-bottom: 10px;
            transition: 0.2s;
        }

            .payment-box:hover {
                background: #f8f9ff;
                border-color: #198754;
                cursor: pointer;
            }

        #cardSection, #upiSection {
            animation: fadeIn 0.3s ease;
        }

        @keyframes fadeIn {

            from {
                opacity: 0;
                transform: translateY(10px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>

    <!-- SCRIPT -->

    <script>

document.addEventListener("DOMContentLoaded", function () {

                 var cardRadio = document.getElementById("rdoCard");
                 var cashRadio = document.getElementById("rdoCash");
                 var upiRadio = document.getElementById("rdoUPI");

                 var cardSection = document.getElementById("cardSection");
                 var upiSection = document.getElementById("upiSection");

                 function togglePayment() {

                     if (cardRadio.checked) {

                         cardSection.style.display = "block";
                         upiSection.style.display = "none";

                     }

                     else if (upiRadio.checked) {

                         upiSection.style.display = "block";
                         cardSection.style.display = "none";

                     }

                     else {

                         cardSection.style.display = "none";
                         upiSection.style.display = "none";

                     }

                 }

                 cardRadio.addEventListener("change", togglePayment);
                 cashRadio.addEventListener("change", togglePayment);
                 upiRadio.addEventListener("change", togglePayment);

             });

    </script>

</asp:Content>
