<%@ Page Title="Table Management" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TableInfo.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.TableInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
        body { background: #f4f6f9; font-family: 'Segoe UI', sans-serif; }
        h3 { font-weight: 600; color: #343a40; text-align: center; margin-bottom: 30px; }
        .table-card {
            background: white;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            justify-content: space-between;
            transition: transform 0.3s, box-shadow 0.3s;
        }
        .table-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 25px rgba(0,0,0,0.15);
        }
        .table-card label { font-weight: 600; }
        .table-card input { width: 120px; text-align: center; margin-right: 10px; }
        .btn-update, .btn-add { background: #4e73df; border: none; color: white; transition: 0.3s; margin-top: 10px; }
        .btn-update:hover, .btn-add:hover { background: #224abe; transform: scale(1.05); }
        .lbl-message { text-align: center; font-weight: 500; margin-top: 15px; color: green; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container mt-5">
    <h3>📊 Admin Table Management</h3>

    <asp:Repeater ID="rptTables" runat="server">
        <ItemTemplate>
            <div class="table-card">
                <div>
                    <label>Table Type:</label>
                    <span><%# Eval("TableType") %></span>
                </div>
                <div>
                    <label>Total Tables:</label>
                    <asp:TextBox ID="txtTotal" runat="server" Text='<%# Eval("TotalTables") %>' CssClass="form-control"></asp:TextBox>
                </div>
                <div>
                    <label>Booked Tables:</label>
                    <asp:TextBox ID="txtBooked" runat="server" Text='<%# Eval("BookedTables") %>' CssClass="form-control"></asp:TextBox>
                </div>
                <div>
                    <label>Amount Per Table:</label>
                    <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("AmountPerTable") %>' CssClass="form-control"></asp:TextBox>
                </div>
                <div>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-update"
                        CommandArgument='<%# Eval("TableID") %>' OnClick="btnUpdate_Click" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <hr />
    <h4>Add New Table Type</h4>
    <div class="table-card">
        <div>
            <label>Table Type:</label>
            <asp:TextBox ID="txtNewType" runat="server" CssClass="form-control" Placeholder="2-Seater / 4-Seater"></asp:TextBox>
        </div>
        <div>
            <label>Total Tables:</label>
            <asp:TextBox ID="txtNewTotal" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div>
            <label>Booked Tables:</label>
            <asp:TextBox ID="txtNewBooked" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
        </div>
        <div>
            <label>Amount Per Table:</label>
            <asp:TextBox ID="txtNewAmount" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btnAddNew" runat="server" Text="Add Table Type" CssClass="btn btn-add" OnClick="btnAddNew_Click" />
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="lbl-message"></asp:Label>
</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>