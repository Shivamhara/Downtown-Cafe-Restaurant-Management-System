using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        // Get connection string from Web.config
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear session on page load
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                lblMessage.Text = "Please enter username and password.";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = "SELECT * FROM Admin_Login WHERE username=@username AND password=@password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password); // Use hashed password in production

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            // Store admin info in session
                            Session["AdminID"] = dr["AdminID"].ToString();
                            Session["AdminUsername"] = dr["username"].ToString();

                            // Redirect to Admin Dashboard
                            Response.Redirect("~/Admin/Dashboard.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "Invalid username or password.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}