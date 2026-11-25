<%@ Page Title="" Language="C#" MasterPageFile="~/HomemasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CafeandfoodRestaurant.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&family=Pacifico&display=swap" rel="stylesheet">

<style>

/* FONT */
body {
    font-family: 'Poppins', sans-serif;
}

/* CENTER LOGIN */
.login-wrapper {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 80vh;
}

/* CARD */
.login-card {
    width: 380px;
    padding: 40px 30px;
    border-radius: 25px;

    background: rgba(0, 0, 0, 0.75);
    backdrop-filter: blur(15px);

    border: 1px solid rgba(255,255,255,0.2);
    box-shadow: 0 20px 50px rgba(0,0,0,0.5);

    color: white;
    animation: fadeIn 0.8s ease-in-out;
}

/* TITLE */
.login-card::before {
    content: "☕ Welcome Back";
    display: block;
    text-align: center;
    font-family: 'Pacifico', cursive;
    font-size: 26px;
    margin-bottom: 20px;
    color: #ffb347;
}

/* INPUT */
.form-control {
    border-radius: 15px;
    padding: 12px;
    border: 1px solid #ddd;
    background: white;
    color: black;
    transition: 0.3s;
}

.form-control::placeholder {
    color: #888;
}

.form-control:focus {
    border-color: #ffb6c1;
    box-shadow: 0 0 10px rgba(255,182,193,0.5);
}

/* BUTTON (CUTE STYLE) */
.login-btn {
    background: linear-gradient(45deg, #ff9a9e, #fad0c4);
    color: #333;
    padding: 12px;
    border: none;
    border-radius: 50px;
    font-weight: 600;
    width: 100%;
    transition: 0.3s;
    box-shadow: 0 6px 15px rgba(255,182,193,0.5);
    letter-spacing: 0.5px;
}

.login-btn:hover {
    transform: scale(1.05);
    box-shadow: 0 10px 25px rgba(255,182,193,0.8);
}

/* TOGGLE */
.toggle-login {
    display: flex;
    justify-content: center;
    margin-bottom: 20px;
}

.toggle-login button {
    margin: 5px;
    border: none;
    padding: 8px 18px;
    border-radius: 30px;
    background: #f1f1f1;
    color: #444;
    font-weight: 500;
    transition: 0.3s;
}

.toggle-login button.active {
    background: linear-gradient(45deg, #ffb6c1, #ffc3a0);
    color: #333;
    box-shadow: 0 4px 12px rgba(255,182,193,0.6);
}

/* LABEL */
label {
    color: #eee;
    font-size: 13px;
}

/* LINK */
a {
    color: #ff9a9e;
    font-size: 13px;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}

/* OTP BUTTON */
#btnSendOTP {
    background: transparent;
    border: 1px solid #ffb6c1;
    color: #ffb6c1;
}

#btnSendOTP:hover {
    background: #ffb6c1;
    color: black;
}

/* ANIMATION */
@keyframes fadeIn {
    from { opacity: 0; transform: translateY(30px); }
    to { opacity: 1; transform: translateY(0); }
}

</style>

<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

<script>
    function showPassword() {
        document.getElementById("passwordDiv").style.display = "block";
        document.getElementById("otpDiv").style.display = "none";
        document.getElementById("btnPasswordMode").classList.add("active");
        document.getElementById("btnOtpMode").classList.remove("active");

        document.getElementById('<%= hfLoginMode.ClientID %>').value = 'Password';
}

function showOTP() {
    document.getElementById("passwordDiv").style.display = "none";
    document.getElementById("otpDiv").style.display = "block";
    document.getElementById("btnOtpMode").classList.add("active");
    document.getElementById("btnPasswordMode").classList.remove("active");

    document.getElementById('<%= hfLoginMode.ClientID %>').value = 'OTP';
    }
</script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">

    <div class="login-wrapper">

        <div class="login-card">

            <!-- Toggle -->
            <div class="toggle-login">
                <button type="button" id="btnPasswordMode" class="active" onclick="showPassword()">Password ☕</button>
                <button type="button" id="btnOtpMode" onclick="showOTP()">OTP ✉️</button>
            </div>

            <!-- Email -->
            <div class="mb-3">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
            </div>

            <!-- Password -->
            <div id="passwordDiv">
                <div class="mb-3">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <!-- OTP -->
            <div id="otpDiv" style="display:none;">
                <div class="mb-3">
                    <label>OTP</label>
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <asp:Button ID="btnSendOTP" runat="server"
                    Text="Send OTP ✉️"
                    CssClass="login-btn mb-3"
                    UseSubmitBehavior="false"
                    OnClick="btnSendOTP_Click" />
            </div>

            <!-- Login -->
            <asp:Button ID="btnLogin" runat="server"
                Text="Login ☕"
                CssClass="login-btn"
                OnClick="btnLogin_Click" />

            <a href="ForgotPassword.aspx">Forgot Password?</a>

            <asp:HiddenField ID="hfLoginMode" runat="server" Value="Password" />

            <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>

        </div>

    </div>

</asp:Panel>

</asp:Content>