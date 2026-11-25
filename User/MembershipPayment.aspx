<%@ Page Title="Membership Payment" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="MembershipPayment.aspx.cs" Inherits="CafeandfoodRestaurant.User.MembershipPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .payment-card {
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            max-width: 500px;
            margin: auto;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">

        <h3 class="text-center mb-4">💳 Membership Payment</h3>

        <div class="payment-card">

            <asp:Label ID="lblAmount" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

            <hr />

            <label>Payment Method</label>
            <asp:RadioButtonList ID="rblPayment" runat="server" CssClass="mb-3">
                <asp:ListItem Text="Credit / Debit Card" Value="Card" Selected="True"></asp:ListItem>
                <asp:ListItem Text="UPI Payment" Value="UPI"></asp:ListItem>
            </asp:RadioButtonList>

            <!-- Card Panel -->
            <div id="cardPanel" class="payment-panel mb-3">
                <label>Card Number</label>
                <asp:TextBox ID="txtCardNumber" MaxLength="16" runat="server" CssClass="form-control"></asp:TextBox>

                <div class="row mt-2">
                    <div class="col">
                        <label>Expiry</label>
                        <asp:TextBox ID="txtExpiry" runat="server" CssClass="form-control" Placeholder="MM/YY"></asp:TextBox>
                    </div>
                    <div class="col">
                        <label>CVV</label>
                        <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>

                <div class="mt-2">
                    <label>Name on Card</label>
                    <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <!-- UPI Panel -->
            <div id="upiPanel" class="payment-panel mb-3" style="display:none;">
                <label>UPI ID</label>
                <asp:TextBox ID="txtUPI" runat="server" CssClass="form-control"></asp:TextBox>

                <div class="mt-2">
                    <label>Name</label>
                    <asp:TextBox ID="txtUPIName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="btn btn-success w-100 mt-2" OnClick="btnPay_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="text-success mt-3 d-block"></asp:Label>

        </div>

    </div>

    <!-- JavaScript to toggle panels -->
    <script>
        var rbl = document.getElementById('<%= rblPayment.ClientID %>');

        rbl.addEventListener('change', function () {
            var selectedValue = rbl.querySelector('input:checked').value;

            if (selectedValue === 'Card') {
                document.getElementById('cardPanel').style.display = 'block';
                document.getElementById('upiPanel').style.display = 'none';
            } else {
                document.getElementById('cardPanel').style.display = 'none';
                document.getElementById('upiPanel').style.display = 'block';
            }
        });
    </script>

</asp:Content>