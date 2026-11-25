using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Analytics : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
                LoadTopProducts();
                LoadTopCustomers();
                LoadMembers();
            }
        }

        void LoadDashboard()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Total Orders
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Orders", con);
                lblOrders.Text = cmd1.ExecuteScalar().ToString();

                // Total Revenue
                string query = @"
        SELECT 
            ISNULL((SELECT SUM(FinalAmount) FROM Orders WHERE PaymentStatus='Paid'),0)
            +
            ISNULL((SELECT SUM(TotalAmount) FROM TableBookings WHERE PaymentStatus='Paid'),0)
        ";

                SqlCommand cmd2 = new SqlCommand(query, con);
                lblRevenue.Text = "₹ " + cmd2.ExecuteScalar().ToString();

                // Total Customers
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Customers", con);
                lblCustomers.Text = cmd3.ExecuteScalar().ToString();

                // Membership Users
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM Membership WHERE IsActive=1", con);
                lblMembers.Text = cmd4.ExecuteScalar().ToString();

                // ❌ No need to call con.Close() here (using will handle it)
            }
        }
        void LoadTopProducts()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT TOP 5 m.MenuName, SUM(od.Quantity) AS TotalSold
                             FROM OrderDetails od
                             INNER JOIN Menu m ON od.MenuID = m.MenuID
                             GROUP BY m.MenuName
                             ORDER BY TotalSold DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTopProducts.DataSource = dt;
                gvTopProducts.DataBind();
            }
        }

        void LoadTopCustomers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT TOP 5 c.FullName, SUM(o.FinalAmount) AS TotalSpent
                             FROM Orders o
                             INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                             WHERE o.PaymentStatus='Paid'
                             GROUP BY c.FullName
                             ORDER BY TotalSpent DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTopCustomers.DataSource = dt;
                gvTopCustomers.DataBind();
            }
        }

        void LoadMembers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT c.FullName, m.MembershipAmount, m.StartDate, m.EndDate, m.DiscountPercent
                             FROM Membership m
                             INNER JOIN Customers c ON m.CustomerID = c.CustomerID
                             WHERE m.IsActive = 1";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMembers.DataSource = dt;
                gvMembers.DataBind();
            }
        }
    }
}