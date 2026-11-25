<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/HomemasterPage.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CafeandfoodRestaurant.ForgotPassword" %>



<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
       /* body {
            background: linear-gradient(135deg, #fbc2eb, #a6c1ee);
            font-family: 'Poppins', sans-serif;
        }*/

        .card-box {
            background: #ffffff;
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1);
            animation: fadeIn 0.6s ease-in-out;
        }

        h3 {
            font-weight: 600;
            color: #6c63ff;
        }

        .form-control {
            border-radius: 12px;
            border: 1px solid #ddd;
            padding: 12px;
            transition: 0.3s;
        }

        .form-control:focus {
            border-color: #ff8fab;
            box-shadow: 0 0 5px rgba(255, 143, 171, 0.5);
        }

        .btn {
            border-radius: 12px;
            padding: 10px;
            font-weight: 500;
            transition: 0.3s;
        }

        .btn-primary {
            background: #6c63ff;
            border: none;
        }

        .btn-primary:hover {
            background: #5a54d1;
        }

        .btn-success {
            background: #00c9a7;
            border: none;
        }

        .btn-success:hover {
            background: #00a98d;
        }

        .btn-danger {
            background: #ff6b6b;
            border: none;
        }

        .btn-danger:hover {
            background: #e55050;
        }

        .icon {
            font-size: 40px;
            color: #ff8fab;
            text-align: center;
            margin-bottom: 10px;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px);}
            to { opacity: 1; transform: translateY(0);}
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container d-flex justify-content-center align-items-center" style="min-height:90vh;">
        <div class="card-box" style="width:100%; max-width:420px;">

            <div class="icon">☕</div>
            <h3 class="text-center mb-4">Forgot Password</h3>

            <!-- Email Panel -->
            <asp:Panel ID="pnlEmail" runat="server">
                <div class="mb-3">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                        Placeholder="Enter your email ✉️"></asp:TextBox>
                </div>
                <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP ☕"
                    CssClass="btn btn-primary w-100 mb-3" OnClick="btnSendOTP_Click" />
            </asp:Panel>

            <!-- OTP Panel -->
            <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                <div class="mb-3">
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control"
                        Placeholder="Enter OTP 🔐"></asp:TextBox>
                </div>
                <asp:Button ID="btnVerifyOTP" runat="server" Text="Verify OTP ✔️"
                    CssClass="btn btn-success w-100 mb-3" OnClick="btnVerifyOTP_Click" />
            </asp:Panel>

            <!-- Reset Panel -->
            <asp:Panel ID="pnlReset" runat="server" Visible="false">
                <div class="mb-3">
                    <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control"
                        TextMode="Password" Placeholder="Current Password 🔑"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control"
                        TextMode="Password" Placeholder="New Password ✨"></asp:TextBox>
                </div>
                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password 💕"
                    CssClass="btn btn-danger w-100" OnClick="btnResetPassword_Click" />
            </asp:Panel>

        </div>
    </div>

</asp:Content>