using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        void LoadDashboard()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Today's Earnings (Orders Only)
                string earningQuery = @"SELECT ISNULL(SUM(FinalAmount),0)
                                        FROM Orders
                                        WHERE CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE)";

                SqlCommand cmd1 = new SqlCommand(earningQuery, con);
                lblTodayEarning.Text = cmd1.ExecuteScalar().ToString();


                // Today's Table Bookings (excluding Rejected)
                string bookingQuery = @"SELECT COUNT(*) 
                                        FROM TableBookings 
                                        WHERE CAST(CreatedDate AS DATE) = CAST(GETDATE() AS DATE) 
                                        AND BookingStatus <> 'Rejected'";

                SqlCommand cmd2 = new SqlCommand(bookingQuery, con);
                lblTodayBookings.Text = cmd2.ExecuteScalar().ToString();


                // Today's Orders
                string orderQuery = @"SELECT COUNT(*)
                                      FROM Orders
                                      WHERE CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE)";

                SqlCommand cmd3 = new SqlCommand(orderQuery, con);
                lblTodayOrders.Text = cmd3.ExecuteScalar().ToString();


                // Total Earnings (Orders + TableBookings)
                string totalEarningQuery = @"     SELECT 
                (
                    ISNULL((SELECT SUM(FinalAmount) FROM Orders WHERE PaymentStatus='Paid'),0) +
                    ISNULL((SELECT SUM(TotalAmount) FROM TableBookings WHERE PaymentStatus='Paid'),0)
                )";

                SqlCommand cmdTotal = new SqlCommand(totalEarningQuery, con);
                lblTotalEarning.Text = cmdTotal.ExecuteScalar().ToString();


                // Total Customers
                string customerQuery = @"SELECT COUNT(*) FROM Customers";

                SqlCommand cmd4 = new SqlCommand(customerQuery, con);
                lblCustomers.Text = cmd4.ExecuteScalar().ToString();
            }
        }
    }
}