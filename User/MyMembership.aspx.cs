using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class MyMembership : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMembership();
            }
        }

        void LoadMembership()
        {
            if (Session["CustomerID"] == null)
                return;

            int customerID = Convert.ToInt32(Session["CustomerID"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TOP 1 MembershipAmount, DiscountPercent, MaxOrders, OrdersUsed, StartDate, EndDate
                                 FROM Membership
                                 WHERE CustomerID=@cid AND IsActive=1 AND EndDate >= GETDATE()
                                 ORDER BY EndDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cid", customerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    pnlMembership.Visible = true;
                    lblAmount.Text = dr["MembershipAmount"].ToString();
                    lblDiscount.Text = dr["DiscountPercent"].ToString();
                    lblMaxOrders.Text = dr["MaxOrders"].ToString();
                    lblOrdersUsed.Text = dr["OrdersUsed"].ToString();
                    lblStart.Text = Convert.ToDateTime(dr["StartDate"]).ToString("dd MMM yyyy");
                    lblEnd.Text = Convert.ToDateTime(dr["EndDate"]).ToString("dd MMM yyyy");
                }
                else
                {
                    pnlMembership.Visible = false;
                    lblNoMembership.Visible = true;
                }

                con.Close();
            }
        }
    }
}