using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Earnings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTotalEarnings();
            }
        }

        // ✅ TOTAL EARNINGS (Orders + TableBookings)
        void GetTotalEarnings()
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                (
                    ISNULL((SELECT SUM(FinalAmount) FROM Orders WHERE PaymentStatus='Paid'),0) +
                    ISNULL((SELECT SUM(TotalAmount) FROM TableBookings WHERE PaymentStatus='Paid'),0)
                )";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                lblTotalEarning.Text = cmd.ExecuteScalar().ToString();
            }
        }

        // ✅ LOAD ORDERS DATA
        protected void btnOrders_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    O.OrderID AS ID,
                    C.FullName AS Customer,
                    O.FinalAmount AS Amount,
                    O.OrderStatus AS Status,
                    O.PaymentStatus,
                    O.OrderDate
                FROM Orders O
                JOIN Customers C ON O.CustomerID = C.CustomerID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEarnings.DataSource = dt;
                gvEarnings.DataBind();
            }
        }

        // ✅ LOAD TABLE BOOKINGS DATA
        protected void btnBookings_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    B.BookingID AS ID,
                    C.FullName AS Customer,
                    B.TotalAmount AS Amount,
                    B.BookingStatus AS Status,
                    B.PaymentStatus,
                    B.BookingDate
                FROM TableBookings B
                JOIN Customers C ON B.CustomerID = C.CustomerID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEarnings.DataSource = dt;
                gvEarnings.DataBind();
            }
        }
    }
}