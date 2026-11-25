<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="AllData.aspx.cs" Inherits="CafeandfoodRestaurant.Admin.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@600;700&family=DM+Sans:wght@300;400;500;600&display=swap" rel="stylesheet" />
    <style>
        :root {
            --bg: #0f0f0f;
            --surface: #1a1a1a;
            --surface2: #222222;
            --border: #2e2e2e;
            --gold: #c9a84c;
            --gold-light: #e8c97a;
            --gold-dim: rgba(201,168,76,0.12);
            --text: #f0ede8;
            --muted: #888;
            --green: #4caf80;
            --red: #e05c5c;
            --blue: #5c8ee0;
            --radius: 14px;
        }

        * { box-sizing: border-box; margin: 0; padding: 0; }

        body, .report-wrap {
            font-family: 'DM Sans', sans-serif;
            background: var(--bg);
            color: var(--text);
        }

        .report-wrap {
            padding: 32px 24px 60px;
            max-width: 1400px;
            margin: 0 auto;
        }

        /* ── Header ── */
        .report-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            margin-bottom: 36px;
            flex-wrap: wrap;
            gap: 16px;
        }

        .report-title {
            font-family: 'Playfair Display', serif;
            font-size: clamp(1.6rem, 4vw, 2.4rem);
            color: var(--gold);
            letter-spacing: -0.5px;
        }

        .report-title span {
            display: block;
            font-family: 'DM Sans', sans-serif;
            font-size: 0.85rem;
            font-weight: 400;
            color: var(--muted);
            letter-spacing: 0.5px;
            margin-bottom: 4px;
            text-transform: uppercase;
        }

        .report-date {
            font-size: 0.82rem;
            color: var(--muted);
            background: var(--surface);
            border: 1px solid var(--border);
            padding: 8px 16px;
            border-radius: 50px;
        }

        /* ── Summary Cards ── */
        .summary-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 16px;
            margin-bottom: 40px;
        }

        .stat-card {
            background: var(--surface);
            border: 1px solid var(--border);
            border-radius: var(--radius);
            padding: 22px 20px;
            position: relative;
            overflow: hidden;
            transition: transform 0.2s, border-color 0.2s;
        }

        .stat-card:hover { transform: translateY(-3px); border-color: var(--gold); }

        .stat-card::before {
            content: '';
            position: absolute;
            top: 0; left: 0; right: 0;
            height: 3px;
            background: var(--gold);
            opacity: 0;
            transition: opacity 0.2s;
        }
        .stat-card:hover::before { opacity: 1; }

        .stat-icon {
            font-size: 1.5rem;
            margin-bottom: 12px;
        }

        .stat-label {
            font-size: 0.75rem;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--muted);
            margin-bottom: 8px;
        }

        .stat-value {
            font-family: 'Playfair Display', serif;
            font-size: 1.9rem;
            color: var(--gold-light);
            line-height: 1;
        }

        .stat-value.green { color: var(--green); }
        .stat-value.blue { color: var(--blue); }

        /* ── Section Heading ── */
        .section-heading {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 16px;
        }

        .section-heading h2 {
            font-family: 'Playfair Display', serif;
            font-size: 1.25rem;
            color: var(--text);
        }

        .badge-count {
            background: var(--gold-dim);
            color: var(--gold);
            border: 1px solid rgba(201,168,76,0.3);
            font-size: 0.72rem;
            font-weight: 600;
            padding: 3px 10px;
            border-radius: 50px;
        }

        /* ── Table Wrapper ── */
        .table-wrap {
            background: var(--surface);
            border: 1px solid var(--border);
            border-radius: var(--radius);
            overflow: hidden;
            margin-bottom: 48px;
        }

        .table-scroll { overflow-x: auto; }

        table {
            width: 100%;
            border-collapse: collapse;
            min-width: 700px;
        }

        thead tr {
            background: var(--surface2);
            border-bottom: 1px solid var(--border);
        }

        th {
            padding: 14px 16px;
            text-align: left;
            font-size: 0.72rem;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--gold);
            font-weight: 600;
            white-space: nowrap;
        }

        td {
            padding: 13px 16px;
            font-size: 0.875rem;
            color: var(--text);
            border-bottom: 1px solid rgba(255,255,255,0.04);
            vertical-align: middle;
        }

        tbody tr:last-child td { border-bottom: none; }

        tbody tr:hover td { background: rgba(201,168,76,0.04); }

        /* ── Badges ── */
        .badge {
            display: inline-block;
            padding: 4px 11px;
            border-radius: 50px;
            font-size: 0.72rem;
            font-weight: 600;
            letter-spacing: 0.3px;
        }

        .badge-success { background: rgba(76,175,128,0.15); color: var(--green); border: 1px solid rgba(76,175,128,0.3); }
        .badge-warning { background: rgba(224,180,80,0.15); color: #e0b450; border: 1px solid rgba(224,180,80,0.3); }
        .badge-danger  { background: rgba(224,92,92,0.15);  color: var(--red);  border: 1px solid rgba(224,92,92,0.3); }
        .badge-info    { background: rgba(92,142,224,0.15); color: var(--blue); border: 1px solid rgba(92,142,224,0.3); }
        .badge-gold    { background: var(--gold-dim); color: var(--gold); border: 1px solid rgba(201,168,76,0.3); }
        .badge-partial { background: rgba(160,110,220,0.15); color: #b07de0; border: 1px solid rgba(160,110,220,0.3); }

        /* ── Amount cells ── */
        .amt { font-family: 'Playfair Display', serif; font-weight: 600; }
        .amt.earn { color: var(--green); }
        .amt.disc  { color: var(--red); }

        /* ── Empty State ── */
        .empty-state {
            text-align: center;
            padding: 48px 20px;
            color: var(--muted);
        }
        .empty-state span { font-size: 2.5rem; display: block; margin-bottom: 12px; }

        /* ── Total Row ── */
        .total-row td {
            background: rgba(201,168,76,0.06) !important;
            border-top: 1px solid rgba(201,168,76,0.2) !important;
            font-weight: 600;
            color: var(--gold-light);
        }

        /* ── Divider ── */
        .divider { height: 1px; background: var(--border); margin: 8px 0 40px; }

        /* ── Responsive ── */
        @media (max-width: 600px) {
            .report-wrap { padding: 20px 14px 40px; }
            .stat-value { font-size: 1.5rem; }
        }

        /* ── Alert ── */
        .alert-error {
            background: rgba(224,92,92,0.1);
            border: 1px solid rgba(224,92,92,0.3);
            color: var(--red);
            padding: 14px 18px;
            border-radius: var(--radius);
            margin-bottom: 24px;
            font-size: 0.875rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="report-wrap">

    <!-- Header -->
    <div class="report-header">
        <div class="report-title">
            <span>Admin Dashboard</span>
            Revenue &amp; Bookings Report
        </div>
        <div class="report-date">
            Generated: <asp:Label ID="lblReportDate" runat="server" />
        </div>
    </div>

    <!-- Error label -->
    <asp:Label ID="lblError" runat="server" CssClass="alert-error" Visible="false" />

    <!-- Summary Cards -->
    <div class="summary-grid">
        <div class="stat-card">
            <div class="stat-icon">🍽️</div>
            <div class="stat-label">Total Food Orders</div>
            <div class="stat-value blue"><asp:Label ID="lblTotalOrders" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-icon">💰</div>
            <div class="stat-label">Food Revenue</div>
            <div class="stat-value green">₹<asp:Label ID="lblFoodRevenue" runat="server" Text="0.00" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-icon">🏮</div>
            <div class="stat-label">Total Bookings</div>
            <div class="stat-value blue"><asp:Label ID="lblTotalBookings" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-icon">🏷️</div>
            <div class="stat-label">Booking Revenue</div>
            <div class="stat-value green">₹<asp:Label ID="lblBookingRevenue" runat="server" Text="0.00" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-icon">✨</div>
            <div class="stat-label">Total Earned</div>
            <div class="stat-value">₹<asp:Label ID="lblTotalEarned" runat="server" Text="0.00" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-icon">🎟️</div>
            <div class="stat-label">Total Discounts Given</div>
            <div class="stat-value" style="color:var(--red)">₹<asp:Label ID="lblTotalDiscount" runat="server" Text="0.00" /></div>
        </div>
    </div>

    <!-- ── FOOD ORDERS SECTION ── -->
    <div class="section-heading">
        <h2>Food Order Payments</h2>
        <span class="badge-count"><asp:Label ID="lblOrderCount" runat="server" Text="0" /> Orders</span>
    </div>

    <div class="table-wrap">
        <div class="table-scroll">
            <asp:GridView ID="gvOrders" runat="server"
                AutoGenerateColumns="false"
                CssClass="orders-table"
                ShowFooter="true"
                EmptyDataText=""
                OnRowDataBound="gvOrders_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="OrderID"       HeaderText="Order #"     />
                    <asp:BoundField DataField="CustomerName"  HeaderText="Customer"    />
                    <asp:BoundField DataField="OrderDate"     HeaderText="Date"        />
                    <asp:BoundField DataField="OrderType"     HeaderText="Type"        />
                    <asp:BoundField DataField="Items"         HeaderText="Items"       />
                    <asp:BoundField DataField="SubTotal"      HeaderText="Sub Total"   DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="DiscountAmount" HeaderText="Discount"   DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="FinalAmount"   HeaderText="Final Amt"   DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PaymentMode"   HeaderText="Payment"     />
                    <asp:BoundField DataField="TransactionID" HeaderText="Txn ID"      />
                    <asp:BoundField DataField="OrderStatus"   HeaderText="Order Status"/>
                    <asp:BoundField DataField="PaymentStatus" HeaderText="Pay Status"  />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-state"><span>🍽️</span>No food orders found.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <div class="divider"></div>

    <!-- ── TABLE BOOKINGS SECTION ── -->
    <div class="section-heading">
        <h2>Table Booking Payments</h2>
        <span class="badge-count"><asp:Label ID="lblBookingCount" runat="server" Text="0" /> Bookings</span>
    </div>

    <div class="table-wrap">
        <div class="table-scroll">
            <asp:GridView ID="gvBookings" runat="server"
                AutoGenerateColumns="false"
                CssClass="bookings-table"
                ShowFooter="true"
                EmptyDataText=""
                OnRowDataBound="gvBookings_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="BookingID"      HeaderText="Booking #"   />
                    <asp:BoundField DataField="CustomerName"   HeaderText="Customer"    />
                    <asp:BoundField DataField="TableType"      HeaderText="Table Type"  />
                    <asp:BoundField DataField="BookingDate"    HeaderText="Date"        />
                    <asp:BoundField DataField="BookingTime"    HeaderText="Time"        />
                    <asp:BoundField DataField="TotalAmount"    HeaderText="Total Amt"   DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="AdvanceAmount"  HeaderText="Advance"     DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="RemainingAmount" HeaderText="Remaining"  DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PaymentMethod"  HeaderText="Payment"     />
                    <asp:BoundField DataField="TransactionID"  HeaderText="Txn ID"      />
                    <asp:BoundField DataField="BookingStatus"  HeaderText="Status"      />
                    <asp:BoundField DataField="PaymentStatus"  HeaderText="Pay Status"  />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-state"><span>🏮</span>No table bookings found.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

</div>
</asp:Content>
