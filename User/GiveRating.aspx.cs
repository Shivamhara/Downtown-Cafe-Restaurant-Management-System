using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class GiveRating : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    string menuId = Request.QueryString["id"];
                    LoadMenu(menuId);
                    LoadExistingFeedback(menuId);
                }
            }
        }

        private void LoadMenu(string id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT MenuName FROM Menu WHERE MenuID=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                object name = cmd.ExecuteScalar();
                if (name != null)
                    lblMenuName.Text = name.ToString();
            }
        }

        private void LoadExistingFeedback(string menuId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT Rating, Comment FROM Ratings WHERE MenuID=@mid AND UserID=@uid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mid", menuId);
                cmd.Parameters.AddWithValue("@uid", Session["CustomerID"].ToString());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hfRating.Value = dr["Rating"].ToString();
                    txtComment.Text = dr["Comment"].ToString();

                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    // No feedback yet
                    hfRating.Value = "";
                    txtComment.Text = "";
                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfRating.Value))
            {
                lblMsg.Text = "Please select rating!";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Ratings(MenuID, UserID, Rating, Comment) VALUES(@mid, @uid, @rating, @comment)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mid", Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@uid", Session["CustomerID"].ToString());
                cmd.Parameters.AddWithValue("@rating", hfRating.Value);
                cmd.Parameters.AddWithValue("@comment", txtComment.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Reload page to display feedback correctly
            Response.Redirect(Request.RawUrl);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfRating.Value))
            {
                lblMsg.Text = "Please select rating!";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Ratings SET Rating=@rating, Comment=@comment WHERE MenuID=@mid AND UserID=@uid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mid", Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@uid", Session["CustomerID"].ToString());
                cmd.Parameters.AddWithValue("@rating", hfRating.Value);
                cmd.Parameters.AddWithValue("@comment", txtComment.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Feedback updated successfully!";
            Response.Redirect(Request.RawUrl);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Ratings WHERE MenuID=@mid AND UserID=@uid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mid", Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@uid", Session["CustomerID"].ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Feedback deleted successfully!";
            Response.Redirect(Request.RawUrl);
        }
    }
}