using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewMembershipUsers : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMembershipUsers();
        }

        void LoadMembershipUsers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT M.MembershipID, C.FullName, C.Email, M.StartDate, M.EndDate, M.DiscountPercent
                                 FROM Membership M
                                 INNER JOIN Customers C ON M.CustomerID = C.CustomerID
                                 WHERE M.IsActive = 1";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMembershipUsers.DataSource = dt;
                gvMembershipUsers.DataBind();
            }
        }

        protected void gvMembershipUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SendExpiryMail")
            {
                int membershipID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"SELECT C.FullName, C.Email, M.EndDate
                                     FROM Membership M
                                     INNER JOIN Customers C ON M.CustomerID = C.CustomerID
                                     WHERE M.MembershipID=@mid";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@mid", membershipID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string fullName = dr["FullName"].ToString();
                        string email = dr["Email"].ToString();
                        DateTime endDate = Convert.ToDateTime(dr["EndDate"]);

                        // Send Email
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
                        mail.To.Add(email);
                        mail.Subject = "⚠ Your Membership is About to Expire";
                        mail.IsBodyHtml = true;
                        mail.Body = $@"
                            <p>Dear {fullName},</p>
                            <p>Your membership will expire on <b>{endDate:dd MMM yyyy}</b>.</p>
                            <p>We invite you to renew your membership to continue enjoying exclusive discounts and benefits.</p>
                            <p>Click here to <a href='https://yourwebsite.com/User/ViewMembershipPlans.aspx'>renew your membership</a>.</p>
                            <p>Best Regards,<br/>Cafe & Food Restaurant Team</p>
                        ";

                        SmtpClient smtp = new SmtpClient();
                        smtp.Send(mail);

                        lblMsg.Text = $"Expiry email sent successfully to {fullName}!";
                    }
                    con.Close();
                }
            }
        }
    }
}