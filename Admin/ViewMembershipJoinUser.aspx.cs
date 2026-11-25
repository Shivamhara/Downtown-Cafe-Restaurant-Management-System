using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewMembershipJoinUser : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["amt"] != null)
                {
                    decimal amount = Convert.ToDecimal(Request.QueryString["amt"]);
                    int maxOrders = Convert.ToInt32(Request.QueryString["max"]);
                    int discount = Convert.ToInt32(Request.QueryString["disc"]);

                    LoadUsers(amount, maxOrders, discount);
                }
            }
        }
        void LoadUsers(decimal amount, int maxOrders, int discount)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
        SELECT c.CustomerID, c.FullName, c.Email, c.Phone,
               m.StartDate, m.EndDate,
               m.OrdersUsed, m.MaxOrders, m.DiscountPercent
        FROM Membership m
        INNER JOIN Customers c ON m.CustomerID = c.CustomerID
        WHERE m.MembershipAmount = @Amount
          AND m.MaxOrders = @MaxOrders
          AND m.DiscountPercent = @Discount";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@MaxOrders", maxOrders);
                cmd.Parameters.AddWithValue("@Discount", discount);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }
    }
}