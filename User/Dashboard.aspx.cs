using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class Dashboard : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

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

                string cid = Session["CustomerID"].ToString();

                // TOTAL ORDERS
                SqlCommand cmd1 = new SqlCommand(
                    "SELECT COUNT(*) FROM Orders WHERE CustomerID=@cid", con);

                cmd1.Parameters.AddWithValue("@cid", cid);

                lblOrders.Text = cmd1.ExecuteScalar().ToString();


                // TOTAL SPENT
                SqlCommand cmd2 = new SqlCommand(
                    "SELECT ISNULL(SUM(FinalAmount),0) FROM Orders WHERE CustomerID=@cid", con);

                cmd2.Parameters.AddWithValue("@cid", cid);

                lblSpent.Text = cmd2.ExecuteScalar().ToString();


                // TABLE BOOKINGS
                SqlCommand cmd3 = new SqlCommand(
                    "SELECT COUNT(*) FROM TableBookings WHERE CustomerID=@cid", con);

                cmd3.Parameters.AddWithValue("@cid", cid);

                lblBookings.Text = cmd3.ExecuteScalar().ToString();


                // RECENT ORDERS
                SqlCommand cmd4 = new SqlCommand(
                    @"SELECT TOP 5 OrderID, OrderDate, FinalAmount, OrderStatus
                      FROM Orders
                      WHERE CustomerID=@cid
                      ORDER BY OrderDate DESC", con);

                cmd4.Parameters.AddWithValue("@cid", cid);

                SqlDataAdapter da = new SqlDataAdapter(cmd4);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridRecentOrders.DataSource = dt;
                GridRecentOrders.DataBind();
            }
        }
    }
}