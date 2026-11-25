using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;

namespace CafeandfoodRestaurant
{
    public partial class Register : System.Web.UI.Page
    {
        string  cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
         string.IsNullOrWhiteSpace(txtEmail.Text) ||
         string.IsNullOrWhiteSpace(txtPassword.Text) ||
         string.IsNullOrWhiteSpace(txtPhone.Text) ||
         string.IsNullOrWhiteSpace(txtAddress.Text) ||
         string.IsNullOrWhiteSpace(txtCity.Text) ||
         string.IsNullOrWhiteSpace(txtPincode.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('Please fill all fields');", true);
                return;
            }
            using (SqlConnection con = new SqlConnection(cs))
            {

                try
                {

                    con.Open();

                    // Check duplicate email or phone

                    SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE Email=@Email OR Phone=@Phone", con);

                    check.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    check.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                    int count = Convert.ToInt32(check.ExecuteScalar());

                    if (count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "alert('Email or Phone already exists');", true);
                        return;
                    }


                    string hashedPassword = GetMD5Hash(txtPassword.Text.Trim());
                    SqlCommand cmd = new SqlCommand("INSERT INTO Customers (FullName,Email,PasswordHash,Phone,Address,City,Pincode,Latitude,Longitude,RegistrationDate,IsActive) VALUES (@FullName,@Email,@Password,@Phone,@Address,@City,@Pincode,@Latitude,@Longitude,GETDATE(),1)", con);


                    cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.Trim());


                    string latitude = Request.Form[txtLatitude.UniqueID];
                    string longitude = Request.Form[txtLongitude.UniqueID];

                    cmd.Parameters.AddWithValue("@Latitude",
                    string.IsNullOrEmpty(latitude) ? (object)DBNull.Value : latitude);

                    cmd.Parameters.AddWithValue("@Longitude",
                    string.IsNullOrEmpty(longitude) ? (object)DBNull.Value : longitude);
                    cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Registration Successful');window.location='Login.aspx';", true);

                    clear();

                }

                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('" + ex.Message + "')", true);

                }

            }

        }

        void clear()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtPincode.Text = "";
            txtLatitude.Text = "";
            txtLongitude.Text = "";
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
        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (Session["UserOTP"] == null)
            {
                lblmsg.Text = "Please request OTP first.";
                return;
            }

            if (txtOTP.Text.Trim() == Session["UserOTP"].ToString())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "otpVerified",
                "Swal.fire({icon:'success',title:'OTP Verified!',text:'OTP Verified Successfully!',confirmButtonColor:'#18c1d6'});", true); Session["OTPVerified"] = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "otpInvalid",
                "Swal.fire({icon:'error',title:'Invalid OTP!',text:'Please enter correct OTP.',confirmButtonColor:'#d33'});", true);
            }
        }
        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lblmsg.Text = "Please enter email first.";
                return;
            }

            string otp = GenerateOTP();
            Session["UserOTP"] = otp;   // Store OTP in session

            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&FoodRestaurtant");

                mail.To.Add(txtEmail.Text.Trim());
                mail.Subject = "Your Registration OTP";
                mail.Body = "Your OTP is: " + otp;
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

                ScriptManager.RegisterStartupScript(this, GetType(), "otpSuccess",
     "Swal.fire({icon:'success',title:'OTP Sent!',text:'OTP sent to your email successfully.',confirmButtonColor:'#18c1d6'});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "otpError",
                "Swal.fire({icon:'error',title:'Error!',text:'Failed to send OTP. Please try again.',confirmButtonColor:'#d33'});", true);
            }
        }
        // Generate Random 6 Digit OTP
        private string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

    }
}
