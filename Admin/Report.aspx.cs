using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Report : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSummary();
                LoadRecentOrders();
                LoadMemberships();
            }
        }

        void LoadSummary()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // Total customers
                lblTotalCustomers.Text = new SqlCommand("SELECT COUNT(*) FROM Customers", con).ExecuteScalar().ToString();

                // Total orders
                lblTotalOrders.Text = new SqlCommand("SELECT COUNT(*) FROM Orders", con).ExecuteScalar().ToString();

                // Total revenue
                lblRevenue.Text = new SqlCommand("SELECT ISNULL(SUM(FinalAmount),0) FROM Orders WHERE PaymentStatus='Completed'", con).ExecuteScalar().ToString();

                // Active memberships
                lblActiveMemberships.Text = new SqlCommand("SELECT COUNT(*) FROM Membership WHERE IsActive=1 AND EndDate >= GETDATE()", con).ExecuteScalar().ToString();
                con.Close();
            }
        }

        void LoadRecentOrders()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TOP 10 o.OrderID, c.FullName AS CustomerName, o.OrderDate, o.FinalAmount, o.PaymentStatus, o.OrderStatus
                                 FROM Orders o
                                 JOIN Customers c ON o.CustomerID = c.CustomerID
                                 ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRecentOrders.DataSource = dt;
                gvRecentOrders.DataBind();
            }
        }
        protected void gvRecentOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRecentOrders.PageIndex = e.NewPageIndex;
            LoadRecentOrders();
        }

        protected void gvMemberships_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMemberships.PageIndex = e.NewPageIndex;
            LoadMemberships();
        }
        void LoadMemberships()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT m.MembershipID, c.FullName AS CustomerName, p.PlanName, m.StartDate, m.EndDate, m.OrdersUsed, m.MaxOrders
                                 FROM Membership m
                                 JOIN Customers c ON m.CustomerID = c.CustomerID
                                 JOIN MembershipPlans p ON m.MembershipAmount = p.Amount
                                 WHERE m.IsActive=1 AND m.EndDate >= GETDATE()
                                 ORDER BY m.StartDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMemberships.DataSource = dt;
                gvMemberships.DataBind();
            }
        }
    }
}