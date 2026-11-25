using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class AddPLans : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void btnAddPlan_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"INSERT INTO MembershipPlans
                                (PlanName,Amount,MaxOrders,DiscountPercent,DurationMonths,IsActive)

                                VALUES
                                (@name,@amount,@max,@discount,@duration,@status)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", txtPlanName.Text);
                cmd.Parameters.AddWithValue("@amount", txtAmount.Text);
                cmd.Parameters.AddWithValue("@max", txtMaxOrders.Text);
                cmd.Parameters.AddWithValue("@discount", txtDiscount.Text);
                cmd.Parameters.AddWithValue("@duration", txtDuration.Text);
                cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMsg.Text = "Membership plan added successfully.";

                txtPlanName.Text = "";
                txtAmount.Text = "";
                txtMaxOrders.Text = "";
                txtDiscount.Text = "";
                txtDuration.Text = "";
            }
        }
    }
}