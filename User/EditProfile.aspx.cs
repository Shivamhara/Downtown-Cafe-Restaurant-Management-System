using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;

namespace CafeandfoodRestaurant.User
{
    public partial class EditProfile : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        void LoadUserProfile()
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT FullName, Email,PasswordHash, Phone, City, Address
                                 FROM Customers
                                 WHERE CustomerID=@cid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtName.Text = dr["FullName"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtPhone.Text = dr["Phone"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtAddress.Text = dr["Address"].ToString();
                  // txtpassword.Text = dr["PasswordHash"].ToString();
                }
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
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
          //  Response.Write("Password value: " + txtpassword.Text + "<br>");
            string oldEmail = "";

            using (SqlConnection con = new SqlConnection(cs))
            {
                string getEmailQuery = "SELECT Email FROM Customers WHERE CustomerID=@cid";
                SqlCommand getCmd = new SqlCommand(getEmailQuery, con);
                getCmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                con.Open();
                oldEmail = getCmd.ExecuteScalar().ToString();
                con.Close();
            }

            // Email verification check
            if (txtEmail.Text.Trim() != oldEmail)
            {
                if (Session["OTPVerified"] == null || !(bool)Session["OTPVerified"])
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "otpRequired",
                    "Swal.fire({icon:'warning',title:'Verify Email!',text:'Please verify OTP before updating email.',confirmButtonColor:'#d33'});", true);
                    return;
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // ✅ ALWAYS update password if entered
                if (!string.IsNullOrWhiteSpace(txtpassword.Text))
                {
                    string hashedPassword = GetMD5Hash(txtpassword.Text.Trim());

                    string query = @"UPDATE Customers
                             SET FullName=@name,
                                 Email=@email,
                                 Phone=@phone,
                                 City=@city,
                                 Address=@address,
                                 PasswordHash=@pass
                             WHERE CustomerID=@cid";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@city", txtCity.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword);
                    cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // Update without password
                    string query = @"UPDATE Customers
                             SET FullName=@name,
                                 Email=@email,
                                 Phone=@phone,
                                 City=@city,
                                 Address=@address
                             WHERE CustomerID=@cid";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@city", txtCity.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                    cmd.ExecuteNonQuery();
                }
            }

            Session["OTPVerified"] = null;

            ScriptManager.RegisterStartupScript(this, this.GetType(),
            "alert", "Swal.fire({icon:'success',title:'Updated!',text:'Profile updated successfully!',confirmButtonColor:'#18c1d6'});", true);
        }
    }
}