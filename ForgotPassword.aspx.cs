using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace CafeandfoodRestaurant
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e) { }

        // 1️⃣ Send OTP
        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                ShowAlert("Error", "Enter email first!", "error");
                return;
            }

            // Check if email exists
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", email);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    ShowAlert("Error", "Email not registered!", "error");
                    return;
                }
            }

            // Generate OTP
            string otp = new Random().Next(100000, 999999).ToString();
            Session["ForgotOTP"] = otp;
            Session["ForgotEmail"] = email;

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&FoodRestaurant");
                mail.To.Add(email);
                mail.Subject = "Password Reset OTP";
                mail.Body = "Your OTP is: " + otp;
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

                // Show OTP panel
                pnlEmail.Visible = false;
                pnlOTP.Visible = true;
                ShowAlert("Success", "OTP sent to your email!", "success");
            }
            catch
            {
                ShowAlert("Error", "Failed to send OTP. Try again.", "error");
            }
        }

        // 2️⃣ Verify OTP
        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string enteredOTP = txtOTP.Text.Trim();
            if (Session["ForgotOTP"] == null || Session["ForgotEmail"] == null)
            {
                ShowAlert("Error", "Request OTP first!", "error");
                return;
            }

            if (enteredOTP == Session["ForgotOTP"].ToString())
            {
                Session["OTPVerified"] = true;
                pnlOTP.Visible = false;
                pnlReset.Visible = true;
                ShowAlert("Success", "OTP Verified!", "success");
            }
            else
            {
                ShowAlert("Error", "Invalid OTP!", "error");
            }
        }

        // 3️⃣ Reset Password
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (Session["OTPVerified"] == null || !(bool)Session["OTPVerified"])
            {
                ShowAlert("Error", "Verify OTP first!", "error");
                return;
            }

            string email = Session["ForgotEmail"].ToString();
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                ShowAlert("Error", "Enter both current and new password!", "error");
                return;
            }

            // Hash passwords
            string hashedCurrent = GetMD5Hash(currentPassword);
            string hashedNew = GetMD5Hash(newPassword);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT PasswordHash FROM Customers WHERE Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", email);
                string dbPassword = cmd.ExecuteScalar()?.ToString();

                if (dbPassword != hashedCurrent)
                {
                    ShowAlert("Error", "Current password does not match!", "error");
                    return;
                }

                // Update password
                cmd = new SqlCommand("UPDATE Customers SET PasswordHash=@NewPass WHERE Email=@Email", con);
                cmd.Parameters.AddWithValue("@NewPass", hashedNew);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }

            Session.Clear();
            ShowAlertRedirect("Success", "Password updated successfully!", "success", "Login.aspx");
        }

        // 🔒 Hashing
        public string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // convert to hex
                }
                return sb.ToString();
            }
        }

        // 🔹 SweetAlert helpers
        private void ShowAlert(string title, string msg, string icon)
        {
            string script = $"Swal.fire('{title}','{msg}','{icon}');";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private void ShowAlertRedirect(string title, string msg, string icon, string url)
        {
            string script = $"Swal.fire('{title}','{msg}','{icon}').then(()=>{{window.location='{url}'}});";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}