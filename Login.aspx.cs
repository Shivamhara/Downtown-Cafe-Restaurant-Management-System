using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace CafeandfoodRestaurant
{
    public partial class Login : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtOTP.Text = "";
                // Do NOT reset hfLoginMode here; let it persist
            }
        }

        // SEND OTP
        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lblmsg.Text = "Enter your email first!";
                return;
            }

            string otp = new Random().Next(100000, 999999).ToString();
            Session["UserOTP"] = otp;
            Session["OTPEmail"] = txtEmail.Text.Trim();

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&FoodRestaurant");
                mail.To.Add(txtEmail.Text.Trim());
                mail.Subject = "Your Login OTP";
                mail.Body = "Your OTP is: " + otp;
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

                txtEmail.ReadOnly = true; // Keep email locked but posted back

                // Keep OTP mode active after sending
                hfLoginMode.Value = "OTP";
                ScriptManager.RegisterStartupScript(this, GetType(), "showOtpDiv",
                    "showOTP(); Swal.fire('Success','OTP sent to your email','success');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "otpError",
                    "Swal.fire('Error','Failed to send OTP','error');", true);
            }
        }
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
        // LOGIN
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lblmsg.Text = "Enter your email.";
                return;
            }

            string email = txtEmail.Text.Trim();
            string loginMode = hfLoginMode.Value;

            // --- OTP LOGIN ---
            if (loginMode == "OTP")
            {
                if (string.IsNullOrEmpty(txtOTP.Text))
                {
                    lblmsg.Text = "Enter the OTP.";
                    return;
                }

                if (Session["UserOTP"] == null || Session["OTPEmail"] == null)
                {
                    lblmsg.Text = "Request OTP first.";
                    return;
                }

                string enteredOTP = txtOTP.Text.Trim();
                string savedOTP = Session["UserOTP"].ToString();
                string savedEmail = Session["OTPEmail"].ToString();

                if (enteredOTP == savedOTP && email.Equals(savedEmail, StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch customer info
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SELECT CustomerID, FullName FROM Customers WHERE Email=@Email", con);
                        cmd.Parameters.AddWithValue("@Email", email);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            Session["UserEmail"] = email;
                            Session["CustomerID"] = dr["CustomerID"].ToString();
                            Session["CustomerName"] = dr["FullName"].ToString();

                            // Clear OTP sessions
                            Session.Remove("UserOTP");
                            Session.Remove("OTPEmail");

                            txtOTP.Text = ""; // clear OTP after successful login
                            ScriptManager.RegisterStartupScript(this, GetType(), "otpSuccess",
                                "Swal.fire({icon:'success',title:'Login Successful',text:'OTP Verified!',confirmButtonColor:'#18c1d6'}).then(()=>{window.location='../User/Dashboard.aspx'});", true);
                        }
                        else
                        {
                            lblmsg.Text = "Email not registered.";
                        }
                        dr.Close();
                    }
                }
                else
                {
                    lblmsg.Text = "Invalid OTP.";
                }

                return; // exit after OTP login
            }

            // --- PASSWORD LOGIN ---
            if (loginMode == "Password")
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    lblmsg.Text = "Enter password.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT CustomerID, FullName FROM Customers WHERE Email=@Email AND PasswordHash=@Password",
                        con);

                    cmd.Parameters.AddWithValue("@Email", email);
                    string hashedPassword = GetMD5Hash(txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Session["UserEmail"] = email;
                        Session["CustomerID"] = dr["CustomerID"].ToString();
                        Session["CustomerName"] = dr["FullName"].ToString();

                        ScriptManager.RegisterStartupScript(this, GetType(), "passSuccess",
                            "Swal.fire({icon:'success',title:'Login Successful',text:'Password Verified!',confirmButtonColor:'#18c1d6'}).then(()=>{window.location='../User/Dashboard.aspx'});", true);
                    }
                    else
                    {
                        lblmsg.Text = "Invalid Email or Password.";
                    }
                    dr.Close();
                }

                return;
            }

            lblmsg.Text = "Enter either OTP or Password.";
        }
    }
}