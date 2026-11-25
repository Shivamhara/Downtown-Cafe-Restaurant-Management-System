using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class AllData : System.Web.UI.Page
    {
        private string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblReportDate.Text = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");
                LoadReport();
            }
        }

        private void LoadReport()
        {
            try
            {
                LoadFoodOrders();
                LoadTableBookings();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading report: " + ex.Message;
                lblError.Visible = true;
            }
        }

        // ── Food Orders ────────────────────────────────────────────────────────
        private void LoadFoodOrders()
        {
            string sql = @"
                SELECT TOP 5
                    o.OrderID,
                    c.FullName                              AS CustomerName,
                    o.OrderDate,
                    ISNULL(o.OrderType,     'N/A')          AS OrderType,
                    ISNULL(
                        (SELECT STUFF(
                            (SELECT ', ' + m2.MenuName + ' x' + CAST(od2.Quantity AS VARCHAR(10))
                             FROM   OrderDetails od2
                             JOIN   Menu m2 ON m2.MenuID = od2.MenuID
                             WHERE  od2.OrderID = o.OrderID
                             FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),
                        1, 2, ''))
                    , 'N/A')                                AS Items,
                    ISNULL(o.SubTotal,       0)             AS SubTotal,
                    ISNULL(o.DiscountAmount, 0)             AS DiscountAmount,
                    ISNULL(o.FinalAmount,    0)             AS FinalAmount,
                    ISNULL(p.PaymentMode,    'N/A')         AS PaymentMode,
                    ISNULL(p.TransactionID,  'N/A')         AS TransactionID,
                    ISNULL(o.OrderStatus,    'N/A')         AS OrderStatus,
                    ISNULL(o.PaymentStatus,  'N/A')         AS PaymentStatus
                FROM Orders o
                JOIN  Customers c   ON c.CustomerID = o.CustomerID
                LEFT JOIN Payments p ON p.OrderID   = o.OrderID
                ORDER BY o.OrderID DESC";

            DataTable dt = GetData(sql);
            gvOrders.DataSource = dt;
            gvOrders.DataBind();

            int totalOrders = dt.Rows.Count;
            decimal foodRevenue = SumColumn(dt, "FinalAmount");
            decimal totalDisc = SumColumn(dt, "DiscountAmount");

            lblTotalOrders.Text = totalOrders.ToString();
            lblOrderCount.Text = totalOrders.ToString();
            lblFoodRevenue.Text = foodRevenue.ToString("N2");
            lblTotalDiscount.Text = totalDisc.ToString("N2");

            ViewState["FoodRevenue"] = foodRevenue;
            ViewState["TotalDiscount"] = totalDisc;
            UpdateGrandTotal();
        }

        // ── Table Bookings ─────────────────────────────────────────────────────
        private void LoadTableBookings()
        {
            string sql = @"
                SELECT TOP 5
                    tb.BookingID,
                    c.FullName                            AS CustomerName,
                    ISNULL(tm.TableType,      'N/A')      AS TableType,
                    ISNULL(tb.BookingDate,    'N/A')      AS BookingDate,
                    ISNULL(tb.BookingTime,    'N/A')      AS BookingTime,
                    ISNULL(tb.TotalAmount,     0)         AS TotalAmount,
                    ISNULL(tb.AdvanceAmount,   0)         AS AdvanceAmount,
                    ISNULL(tb.RemainingAmount, 0)         AS RemainingAmount,
                    ISNULL(tb.PaymentMethod,  'N/A')      AS PaymentMethod,
                    ISNULL(tb.TransactionID,  'N/A')      AS TransactionID,
                    ISNULL(tb.BookingStatus,  'N/A')      AS BookingStatus,
                    ISNULL(tb.PaymentStatus,  'N/A')      AS PaymentStatus
                FROM TableBookings tb
                JOIN Customers   c  ON c.CustomerID = tb.CustomerID
                JOIN TableMaster tm ON tm.TableID   = tb.TableID
                ORDER BY tb.BookingID DESC";

            DataTable dt = GetData(sql);
            gvBookings.DataSource = dt;
            gvBookings.DataBind();

            int totalBookings = dt.Rows.Count;
            decimal bookingRevenue = SumColumn(dt, "AdvanceAmount");

            lblTotalBookings.Text = totalBookings.ToString();
            lblBookingCount.Text = totalBookings.ToString();
            lblBookingRevenue.Text = bookingRevenue.ToString("N2");

            ViewState["BookingRevenue"] = bookingRevenue;
            UpdateGrandTotal();
        }

        // ── Grand Total ────────────────────────────────────────────────────────
        private void UpdateGrandTotal()
        {
            decimal food = ViewState["FoodRevenue"] != null ? (decimal)ViewState["FoodRevenue"] : 0m;
            decimal booking = ViewState["BookingRevenue"] != null ? (decimal)ViewState["BookingRevenue"] : 0m;
            lblTotalEarned.Text = (food + booking).ToString("N2");
        }

        // ══════════════════════════════════════════════════════════════════════
        // GridView RowDataBound
        // ══════════════════════════════════════════════════════════════════════
        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                StyleAmountCell(e.Row.Cells[5], "amt");
                StyleAmountCell(e.Row.Cells[6], "amt disc");
                StyleAmountCell(e.Row.Cells[7], "amt earn");
                BadgeOrderType(e.Row.Cells[3]);
                BadgeOrderStatus(e.Row.Cells[10]);
                BadgePayStatus(e.Row.Cells[11]);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.CssClass = "total-row";
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[0].ColumnSpan = 7;
                for (int i = 1; i < 7; i++) e.Row.Cells[i].Visible = false;

                DataTable dt = ((GridView)sender).DataSource as DataTable;
                if (dt != null)
                {
                    e.Row.Cells[7].Text = "Rs. " + SumColumn(dt, "FinalAmount").ToString("N2");
                    e.Row.Cells[7].CssClass = "amt earn";
                }
            }
        }

        protected void gvBookings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                StyleAmountCell(e.Row.Cells[5], "amt");
                StyleAmountCell(e.Row.Cells[6], "amt earn");
                StyleAmountCell(e.Row.Cells[7], "amt disc");
                BadgeBookingStatus(e.Row.Cells[10]);
                BadgePayStatus(e.Row.Cells[11]);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.CssClass = "total-row";
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[0].ColumnSpan = 6;
                for (int i = 1; i < 6; i++) e.Row.Cells[i].Visible = false;

                DataTable dt = ((GridView)sender).DataSource as DataTable;
                if (dt != null)
                {
                    e.Row.Cells[6].Text = "Rs. " + SumColumn(dt, "AdvanceAmount").ToString("N2");
                    e.Row.Cells[6].CssClass = "amt earn";
                    e.Row.Cells[7].Text = "Rs. " + SumColumn(dt, "RemainingAmount").ToString("N2");
                    e.Row.Cells[7].CssClass = "amt disc";
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // Badge + Style Helpers
        // ══════════════════════════════════════════════════════════════════════
        private void StyleAmountCell(TableCell cell, string cssClass)
        {
            cell.CssClass = cssClass;
            string t = (cell.Text ?? "").Trim();
            if (!string.IsNullOrEmpty(t) && t != "N/A" && t != "0")
                cell.Text = "Rs. " + t;
        }

        private void BadgeOrderStatus(TableCell cell)
        {
            string val = (cell.Text ?? "").Trim();
            string css;
            if (val == "Delivered" || val == "Reached") css = "badge badge-success";
            else if (val == "Processing") css = "badge badge-info";
            else if (val == "Pending") css = "badge badge-warning";
            else if (val == "Cancelled") css = "badge badge-danger";
            else css = "badge badge-gold";
            cell.Text = "<span class='" + css + "'>" + val + "</span>";
        }

        private void BadgePayStatus(TableCell cell)
        {
            string val = (cell.Text ?? "").Trim();
            string css;
            if (val == "Paid" || val == "Success") css = "badge badge-success";
            else if (val == "Partial") css = "badge badge-partial";
            else if (val == "Pending") css = "badge badge-warning";
            else if (val == "Failed") css = "badge badge-danger";
            else css = "badge badge-gold";
            cell.Text = "<span class='" + css + "'>" + val + "</span>";
        }

        private void BadgeBookingStatus(TableCell cell)
        {
            string val = (cell.Text ?? "").Trim();
            string css;
            if (val == "Approved") css = "badge badge-success";
            else if (val == "Completed") css = "badge badge-info";
            else if (val == "Pending") css = "badge badge-warning";
            else if (val == "Cancelled") css = "badge badge-danger";
            else css = "badge badge-gold";
            cell.Text = "<span class='" + css + "'>" + val + "</span>";
        }

        private void BadgeOrderType(TableCell cell)
        {
            string val = (cell.Text ?? "").Trim();
            string css;
            if (val == "Delivery") css = "badge badge-info";
            else if (val == "Takeaway") css = "badge badge-success";
            else css = "badge badge-gold";
            cell.Text = "<span class='" + css + "'>" + val + "</span>";
        }

        // ══════════════════════════════════════════════════════════════════════
        // Data Helpers
        // ══════════════════════════════════════════════════════════════════════
        private decimal SumColumn(DataTable dt, string colName)
        {
            decimal total = 0m;
            if (dt == null || !dt.Columns.Contains(colName)) return total;
            foreach (DataRow row in dt.Rows)
                if (row[colName] != DBNull.Value)
                    total += Convert.ToDecimal(row[colName]);
            return total;
        }

        private DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnStr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }
}
