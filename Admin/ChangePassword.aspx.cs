using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string adminid = Session["AdminID"].ToString();
            using(SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = @"select username,password from Admin_Login where AdminID=@id";
                using(SqlCommand cmd = new SqlCommand(query,con))
                {
                    cmd.Parameters.AddWithValue("@id", adminid);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if(dr.Read())
                    {
                        txtCurrentUsername.Text = dr["username"].ToString();
                        txtCurrentPassword.Text = dr["password"].ToString();
                    }
                    con.Close();
                }
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            string currentUsername = txtCurrentUsername.Text.Trim();
            string currentPassword = txtCurrentPassword.Text.Trim();

            string newUsername = txtNewUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // ✅ Fix username
            if (string.IsNullOrEmpty(newUsername))
            {
                newUsername = currentUsername;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // ✅ Check current credentials FIRST
                string checkQuery = "SELECT password FROM Admin_Login WHERE username=@u";
                SqlCommand cmd = new SqlCommand(checkQuery, con);
                cmd.Parameters.AddWithValue("@u", currentUsername);

                object dbPassObj = cmd.ExecuteScalar();

                if (dbPassObj == null)
                {
                    lblMsg.Text = "User not found!";
                    return;
                }

                string dbPassword = dbPassObj.ToString();

                // 🔴 Validate current password
                if (dbPassword != currentPassword)
                {
                    lblMsg.Text = "Incorrect current password!";
                    return;
                }

                // ✅ Fix password
                if (string.IsNullOrEmpty(newPassword))
                {
                    newPassword = dbPassword; // keep old password
                }
                else
                {
                    if (newPassword != confirmPassword)
                    {
                        lblMsg.Text = "Passwords do not match!";
                        return;
                    }
                }

                // ✅ Update using AdminID (BEST PRACTICE)
                string updateQuery = @"UPDATE Admin_Login 
                              SET username=@newU, password=@newP 
                              WHERE AdminID=@id";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@newU", newUsername);
                updateCmd.Parameters.AddWithValue("@newP", newPassword);
                updateCmd.Parameters.AddWithValue("@id", Session["AdminID"]);

                updateCmd.ExecuteNonQuery();

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Credentials updated successfully!";
                Response.Redirect("~/Admin/ChangePassword.aspx");
            }
        }
    }
}