<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="CafeandfoodRestaurant.User.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .profile-card {
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.08);
            max-width: 800px;
            margin: auto;
        }

        .profile-title {
            font-size: 26px;
            font-weight: 600;
            color: #0f4c5c;
            margin-bottom: 20px;
        }

        .profile-img {
            width: 120px;
            height: 120px;
            border-radius: 50%;
            object-fit: cover;
            border: 4px solid #ff7f50;
            margin-bottom: 15px;
        }

        .save-btn {
            background: #0f4c5c;
            color: white;
            border: none;
            padding: 10px 25px;
            border-radius: 8px;
            font-weight: 500;
        }

            .save-btn:hover {
                background: #1f7a8c;
            }
    </style>
            <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlProfile" runat="server" DefaultButton="btnUpdate">
    <div class="container">

        <div class="profile-card">

            <div class="text-center">

                <%--<img src="../Images/user.png" class="profile-img" />--%>

                <h3 class="profile-title">
                    <i class="fa fa-user"></i>Edit Profile
                </h3>

            </div>

            <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
            <div class="row g-3">

                <div class="col-md-6">

                    <label class="form-label">Full Name</label>

                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>

                </div>

                <div class="col-md-6">

                    <label class="form-label">Email</label>

                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>

                </div>
                <asp:Button ID="btnSendOTP" runat="server"
                            Text="Send OTP"
                            CssClass="btn btn-dark"  UseSubmitBehavior="false"
                            OnClick="btnSendOTP_Click" />

                        <!-- OTP -->
                        <div class="col-md-6 mb-3">
                            <i class="fas fa-key"></i>
                            <asp:TextBox ID="txtOTP" autocomplete="off" runat="server" CssClass="form-control" placeholder="Enter OTP"></asp:TextBox>
                        </div>

                        <!-- Verify OTP -->
                        <asp:Button ID="btnVerifyOTP" runat="server"
                            Text="Verify OTP"
                            CssClass="btn btn-info"
                            OnClick="btnVerifyOTP_Click" />
                <div class="col-md-6">

                    <label class="form-label">Phone</label>

                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>

                </div>

                <div class="col-md-6">

                    <label class="form-label">City</label>

                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>

                </div>
                 <div class="col-md-6">

                    <label class="form-label">Password</label>

                    <asp:TextBox ID="txtpassword" placeholder="Enter New Password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                       <asp:RegularExpressionValidator 
        ID="revPassword" 
        runat="server"
        ControlToValidate="txtpassword"
        CssClass="text-danger"
        ErrorMessage="Password must be at least 6 characters, include uppercase, lowercase, number, and symbol."
        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,}$"
        Display="Dynamic">
    </asp:RegularExpressionValidator>
                </div>

                <div class="col-md-12">

                    <label class="form-label">Address</label>

                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>

                </div>

                <div class="col-md-12 text-center mt-3">

                    <asp:Button ID="btnUpdate" runat="server" Text="Update Profile"
                        CssClass="btn save-btn" OnClick="btnUpdate_Click" />

                </div>

            </div>

        </div>

    </div>
        </asp:Panel>

</asp:Content>
