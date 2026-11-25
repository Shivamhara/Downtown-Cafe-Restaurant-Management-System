<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="CafeandfoodRestaurant.Menu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Restaurant Menu</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body{
            background:#f8f9fa;
            font-family:'Segoe UI';
        }

        .menu-card{
            background:white;
            border-radius:12px;
            box-shadow:0 4px 15px rgba(0,0,0,0.1);
            overflow:hidden;
            transition:0.3s;
        }

        .menu-card:hover{
            transform:translateY(-5px);
        }

        .menu-img{
            width:100%;
            height:200px;
            object-fit:cover;
        }

        .menu-body{
            padding:15px;
        }

        .menu-title{
            font-weight:600;
            font-size:18px;
        }

        .menu-price{
            color:#28a745;
            font-weight:600;
        }

        .menu-desc{
            font-size:14px;
            color:#666;
        }
    </style>

</head>
<body>

<form id="form1" runat="server">

<div class="container mt-5">
    <asp:Button ID="btnback" runat="server" Text="Go Back" OnClick="btnback_Click" />
    <h2 class="text-center mb-4">🍽 Our Menu</h2>

    <div class="row">

        <asp:Repeater ID="rptMenu" runat="server">

            <ItemTemplate>

                <div class="col-md-4 mb-4">

                    <div class="menu-card">

                        <img src='<%# Eval("ImagePath") %>' class="menu-img" />

                        <div class="menu-body">

                            <div class="menu-title">
                                <%# Eval("MenuName") %>
                            </div>

                            <div class="menu-price">
                                ₹ <%# Eval("Cost") %>
                            </div>
                            <div class="menu-price">
                                 <%# Eval("Description") %>
                            </div>
                            <div class="menu-desc mt-2">
                                <%# Eval("Category") %>
                            </div>

                        </div>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

</div>

</form>

</body>
</html>