using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewMembershipPlans : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPlans();
            }
        }

        void LoadPlans()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM MembershipPlans WHERE IsActive=1";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                rptPlans.DataSource = dt;
                rptPlans.DataBind();
            }
        }

        protected void rptPlans_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "JoinPlan")
            {
                int planID = Convert.ToInt32(e.CommandArgument);

                Response.Redirect("MembershipPayment.aspx?plan=" + planID);
            }
            if (e.CommandName == "JoinPlan")
            {

                if (Session["CustomerID"] == null)
                {
                    lblMsg.Text = "Please login first.";
                    return;
                }

                int planID = Convert.ToInt32(e.CommandArgument);
                int customerID = Convert.ToInt32(Session["CustomerID"]);

                using (SqlConnection con = new SqlConnection(cs))
                {

                    string query = @"INSERT INTO Membership
                                    (CustomerID,MembershipAmount,StartDate,EndDate,
                                    OrdersUsed,MaxOrders,DiscountPercent,IsActive)

                                    SELECT
                                    @cid,
                                    Amount,
                                    GETDATE(),
                                    DATEADD(MONTH,DurationMonths,GETDATE()),
                                    0,
                                    MaxOrders,
                                    DiscountPercent,
                                    1
                                    FROM MembershipPlans
                                    WHERE PlanID=@pid";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@cid", customerID);
                    cmd.Parameters.AddWithValue("@pid", planID);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblMsg.Text = "Membership activated successfully!";
                }
            }
        }
    }
}