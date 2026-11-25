<%@ Page Title="" Language="C#" MasterPageFile="~/HomemasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CafeandfoodRestaurant.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&family=Pacifico&display=swap" rel="stylesheet">

<style>
/* 🌟 PAGE CENTER */
.register-wrapper {
    display: flex;
    justify-content: center;
    padding: 40px 0;
}

/* 💎 CARD */
.register-card {
    width: 700px;   /* ✅ wider */
    background: #ffffff;
    padding: 40px;
    border-radius: 12px;

    box-shadow: 0 8px 25px rgba(0,0,0,0.1);
}

/* 🧁 TITLE */
.register-title {
    text-align: center;
    font-size: 28px;
    font-weight: 600;
    margin-bottom: 30px;
    color: #222;
}

/* LABEL */
label {
    font-size: 14px;
    font-weight: 500;
    color: #444;
}

/* INPUT */
.form-control {
    border-radius: 6px;
    padding: 10px;
    border: 1px solid #ccc;
}

.form-control:focus {
    border-color: #ff4d00;
    box-shadow: 0 0 5px rgba(255,77,0,0.3);
}

/* BUTTON */
.register-btn {
    background: #ff4d00;
    color: white;
    border: none;
    padding: 12px;
    border-radius: 6px;
    width: 100%;
    font-weight: 600;
    margin-top: 15px;
}

.register-btn:hover {
    background: #e64400;
}

/* OTP SECTION */
.otp-box {
    display: flex;
    gap: 10px;
}

.btn-otp {
    background: #fff;
    border: 1px solid #ff4d00;
    color: #ff4d00;
    padding: 10px;
    border-radius: 6px;
    width: 120px;
}

.btn-otp:hover {
    background: #ff4d00;
    color: white;
}

.btn-verify {
    background: #28a745;
    color: white;
    border: none;
    padding: 10px;
    border-radius: 6px;
    width: 120px;
}

/* LINK */
.login-link {
    text-align: center;
    margin-top: 15px;
}
/* INPUT GROUP FIX */
.input-group {
    display: flex;
    width: 100%;
}

.input-group .form-control {
    flex: 1;
    border-radius: 6px 0 0 6px;
}

.input-group-append .btn {
    border-radius: 0 6px 6px 0;
    height: 100%;
}

/* SEND OTP BUTTON */
.otp-send-btn {
    background: #ff4d00;
    color: white;
    border: none;
    padding: 0 18px;
    font-weight: 500;
}

.otp-send-btn:hover {
    background: #e64400;
}

/* VERIFY BUTTON */
.otp-verify-btn {
    background: #28a745;
    color: white;
    border: none;
    padding: 0 18px;
    font-weight: 500;
}

.otp-verify-btn:hover {
    background: #218838;
}

</style>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>


<asp:Panel ID="pnlRegister" runat="server">

<div class="register-wrapper">

<div class="register-card">

<div class="register-title">☕ Create Your Account</div>

<div class="row">

    <div class="col-md-6 mb-3">
        <label>Full Name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <!-- EMAIL + SEND OTP -->
<div class="col-md-12 mb-3">
    <label>Email</label>

    <div class="input-group">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>

        <div class="input-group-append">
            <asp:Button ID="btnSendOTP" runat="server"
                Text="Send OTP"
                CssClass="btn otp-send-btn"
                OnClick="btnSendOTP_Click" />
        </div>
    </div>

    <asp:RegularExpressionValidator
        ID="revEmail"
        runat="server"
        ControlToValidate="txtEmail"
        CssClass="text-danger"
        ErrorMessage="Enter valid email"
        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
</div>

<!-- OTP + VERIFY -->
<div class="col-md-12 mb-3">
    <label>Enter OTP</label>

    <div class="input-group">
        <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control"></asp:TextBox>

        <div class="input-group-append">
            <asp:Button ID="btnVerifyOTP" runat="server"
                Text="Verify"
                CssClass="btn otp-verify-btn"
                OnClick="btnVerifyOTP_Click" />
        </div>
    </div>
</div>
    <div class="col-md-6 mb-3">
        <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>

        <asp:RegularExpressionValidator ID="revPassword" runat="server"
            ControlToValidate="txtPassword"
            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&#]).{6,}$"
            ErrorMessage="Strong password required"
            ForeColor="Red" />
    </div>

    <div class="col-md-6 mb-3">
        <label>Phone</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="col-md-12 mb-3">
        <label>Address</label>
        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="col-md-6 mb-3">
        <label>City</label>
        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="col-md-6 mb-3">
        <label>Pincode</label>
        <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="col-md-6 mb-3">
        <label>Latitude</label>
        <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>

    <div class="col-md-6 mb-3">
        <label>Longitude</label>
        <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>

</div>

<asp:Button ID="btnRegister" runat="server"
    Text="Create Account"
    CssClass="register-btn"
    OnClick="btnRegister_Click" />

<div class="login-link">
    Already have an account?
    <a href="Login.aspx">Login Here</a>
</div>

<asp:Label ID="lblmsg" runat="server" ForeColor="Yellow"></asp:Label>

</div>
</div>

</asp:Panel>

<script>
    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        }
    }

    function showPosition(position) {
        document.getElementById('<%=txtLatitude.ClientID%>').value = position.coords.latitude;
    document.getElementById('<%=txtLongitude.ClientID%>').value = position.coords.longitude;
    }

    window.onload = getLocation;
</script>

</asp:Content>