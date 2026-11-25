<%@ Page Title="Track Table Booking" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="TableBookingTrack.aspx.cs" Inherits="CafeandfoodRestaurant.User.TableBookingTrack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tracking-step {
            font-size: 14px;
            padding: 8px 16px;
            border-radius: 6px;
            margin: 3px;
            color: white;
        }

        .requested {
            background: #6c757d;
        }

        .advance {
            background: #0d6efd;
        }

        .confirmed {
            background: #ffc107;
            color: black;
        }

        .arrival {
            background: #198754;
        }

        .current-step {
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
            transform: scale(1.05);
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <h3 class="mb-4">🍽 Track Your Table Booking</h3>

        <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block"></asp:Label>

        <asp:TextBox ID="txtTxnID" autocomplete="off" runat="server" CssClass="form-control mb-2"
            Placeholder="Enter Transaction ID"></asp:TextBox>

        <asp:Button ID="btnTrack" runat="server" Text="Track Booking"
            CssClass="btn btn-success mb-4" OnClick="btnTrack_Click" />


        <asp:Panel ID="pnlBookingDetails" runat="server" Visible="false">

            <h5>📄 Booking Details</h5>

            <asp:Literal ID="litBookingDetails" runat="server"></asp:Literal>


            <h5 class="mt-3">📍 Booking Status</h5>
            <asp:Label ID="lblStatus" runat="server" CssClass="fw-bold fs-5"></asp:Label>
           <%-- <div class="d-flex flex-wrap mb-3">

                <div id="stepRequested" runat="server" class="tracking-step requested">
                    Booking Requested
                </div>

                <div id="stepAdvance" runat="server" class="tracking-step advance">
                    Advance Paid
                </div>

                <div id="stepConfirmed" runat="server" class="tracking-step confirmed">
                    Table Confirmed
                </div>

                <div id="stepArrival" runat="server" class="tracking-step arrival">
                    Ready for Arrival
                </div>
               
            </div>--%>

        </asp:Panel>

    </div>

</asp:Content>
