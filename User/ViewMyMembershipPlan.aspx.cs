using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewMyMembershipPlan : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMembershipPlans();
            }
        }

        private void LoadMembershipPlans()
        {
            if (Session["CustomerID"] == null)
            {
                lblMsg.Text = "Please login to view your membership plans.";
                gvMembershipPlans.Visible = false;
                return;
            }

            int customerId = Convert.ToInt32(Session["CustomerID"]);

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        SELECT MembershipID, MembershipAmount, StartDate, EndDate, OrdersUsed, MaxOrders, DiscountPercent, IsActive
                        FROM Membership
                        WHERE CustomerID = @CustomerID
                        ORDER BY StartDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@CustomerID", customerId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        gvMembershipPlans.DataSource = dt;
                        gvMembershipPlans.DataBind();
                        gvMembershipPlans.Visible = true;
                        lblMsg.Text = "";
                    }
                    else
                    {
                        gvMembershipPlans.Visible = false;
                        lblMsg.Text = "You have not joined any membership plan yet.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error loading membership plans: " + ex.Message;
            }
        }
    }
}