using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewPlans : System.Web.UI.Page
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
                string query = "SELECT * FROM MembershipPlans";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPlans.DataSource = dt;
                gvPlans.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT * FROM MembershipPlans
            WHERE PlanName LIKE @kw
            OR CAST(Amount AS NVARCHAR) LIKE @kw
            OR CAST(MaxOrders AS NVARCHAR) LIKE @kw
            OR CAST(DiscountPercent AS NVARCHAR) LIKE @kw
            OR CAST(DurationMonths AS NVARCHAR) LIKE @kw
            OR CAST(IsActive AS NVARCHAR) LIKE @kw";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPlans.DataSource = dt;
                gvPlans.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadPlans(); // reload all plans
        }
        protected void gvPlans_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            // DELETE PLAN
            if (e.CommandName == "DeletePlan")
            {
                int planID;

                // Safe conversion
                if (!int.TryParse(e.CommandArgument.ToString(), out planID))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "alert('Invalid Plan ID');", true);
                    return;
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // ✅ Check if plan is used
                    string checkQuery = "SELECT COUNT(*) FROM Membership WHERE PlanID = @PlanID";

                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                    {
                        cmdCheck.Parameters.AddWithValue("@PlanID", planID);

                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (count > 0)
                        {
                            // ❌ Block delete
                            ScriptManager.RegisterStartupScript(this, GetType(), "cannotDelete",
                                "alert('Cannot delete! This plan is already assigned to users.');", true);
                        }
                        else
                        {
                            // ✅ Safe delete
                            string deleteQuery = "DELETE FROM MembershipPlans WHERE PlanID = @PlanID";

                            using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, con))
                            {
                                cmdDelete.Parameters.AddWithValue("@PlanID", planID);
                                cmdDelete.ExecuteNonQuery();
                            }

                            ScriptManager.RegisterStartupScript(this, GetType(), "deleted",
                                "alert('Plan deleted successfully');", true);

                            LoadPlans();
                        }
                    }
                }
            }

            // EDIT PLAN
            if (e.CommandName == "EditPlan")
            {
                Response.Redirect("EditPlan.aspx?id=" + e.CommandArgument);
            }

            // VIEW USERS
            if (e.CommandName == "ViewUsers")
            {
                string[] data = e.CommandArgument.ToString().Split('|');

                string amount = data[0];
                string maxOrders = data[1];
                string discount = data[2];

                Response.Redirect("ViewMembershipJoinUser.aspx?amt=" + amount +
                                  "&max=" + maxOrders +
                                  "&disc=" + discount);
            }
        }
    }
}