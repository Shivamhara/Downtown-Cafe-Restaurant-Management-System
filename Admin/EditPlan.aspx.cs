using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class EditPlan : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        int planID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                planID = Convert.ToInt32(Request.QueryString["id"]);
            }

            if (!IsPostBack)
            {
                LoadPlan();
            }
        }

        void LoadPlan()
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = "SELECT * FROM MembershipPlans WHERE PlanID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", planID);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtPlanName.Text = dr["PlanName"].ToString();
                    txtAmount.Text = dr["Amount"].ToString();
                    txtMaxOrders.Text = dr["MaxOrders"].ToString();
                    txtDiscount.Text = dr["DiscountPercent"].ToString();
                    txtDuration.Text = dr["DurationMonths"].ToString();
                    ddlStatus.SelectedValue = dr["IsActive"].ToString();
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"UPDATE MembershipPlans
                                SET PlanName=@name,
                                Amount=@amount,
                                MaxOrders=@max,
                                DiscountPercent=@discount,
                                DurationMonths=@duration,
                                IsActive=@status
                                WHERE PlanID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", txtPlanName.Text);
                cmd.Parameters.AddWithValue("@amount", txtAmount.Text);
                cmd.Parameters.AddWithValue("@max", txtMaxOrders.Text);
                cmd.Parameters.AddWithValue("@discount", txtDiscount.Text);
                cmd.Parameters.AddWithValue("@duration", txtDuration.Text);
                cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@id", planID);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMsg.Text = "Plan updated successfully.";
            }
        }
    }
}