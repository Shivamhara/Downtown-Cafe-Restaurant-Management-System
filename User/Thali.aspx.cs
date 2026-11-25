using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.User
{
    public partial class Thali : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMenu("Thali"); // Using Category Name
        }

        private void LoadMenu(string categoryName)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT MenuID, MenuName, Cost AS Price, Description, ImagePath AS Image " +
                    "FROM Menu WHERE Category=@Category AND IsAvailable=1", con);
                cmd.Parameters.AddWithValue("@Category", categoryName);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    DataTable dtMenu = new DataTable();
                    dtMenu.Load(dr);

                    rptMenu.DataSource = dtMenu;
                    rptMenu.DataBind();

                    // Bind feedback for each menu item
                    foreach (RepeaterItem item in rptMenu.Items)
                    {
                        int menuId = Convert.ToInt32(dtMenu.Rows[item.ItemIndex]["MenuID"]);
                        Repeater rptFeedback = (Repeater)item.FindControl("rptFeedback");
                        Label lblNoFeedback = (Label)item.FindControl("lblNoFeedback");
                        BindFeedback(rptFeedback, menuId, lblNoFeedback);
                    }

                    lblNoItems.Text = "";
                }
                else
                {
                    lblNoItems.Text = "No items found in this category.";
                }

                dr.Close();
            }
        }

        private void BindFeedback(Repeater rpt, int menuId, Label lblNoFeedback)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT Top 1 R.Comment, R.Rating, U.FullName AS UserName " +
                               "FROM Ratings R INNER JOIN Customers U ON R.UserID=U.CustomerID " +
                               "WHERE R.MenuID=@MenuID AND R.UserID <> @CurrentUserID" + " ORDER BY R.RatingID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MenuID", menuId);
                cmd.Parameters.AddWithValue("@CurrentUserID", Session["CustomerID"].ToString());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);

                if (dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    lblNoFeedback.Visible = false;
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                    lblNoFeedback.Visible = true;
                }

                dr.Close();
            }
        }

        protected void btnGiveFeedback_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string menuId = btn.CommandArgument;

            Response.Redirect("~/User/GiveRating.aspx?id=" + menuId);
        }
    }
}