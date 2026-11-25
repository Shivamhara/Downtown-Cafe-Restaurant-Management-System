using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace CafeandfoodRestaurant.User
{
    public partial class MembershipPayment : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        int planID;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["plan"] != null)
            {
                planID = Convert.ToInt32(Request.QueryString["plan"]);
            }

            if (!IsPostBack)
            {
                LoadPlanAmount();
            }
        }
        void SendMembershipActivationEmail(int customerID, decimal amount, DateTime start, DateTime end)
        {
            string email = "";
            string fullName = "";

            // Get user email & name
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT FullName, Email FROM Customers WHERE CustomerID=@cid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cid", customerID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    fullName = dr["FullName"].ToString();
                    email = dr["Email"].ToString();
                }
                con.Close();
            }

            if (!string.IsNullOrEmpty(email))
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("myrestaurant@gmail.com", "Cafe & Food Restaurant");
                mail.To.Add(email);
                mail.Subject = "🎉 Your Membership is Activated!";

                mail.IsBodyHtml = true;
                mail.Body = $@"
            <p>Dear {fullName},</p>
            <p>Thank you for purchasing our <b>Premium Membership Plan</b>.</p>
            <p><b>Amount Paid:</b> ₹{amount}<br/>
               <b>Start Date:</b> {start:dd MMM yyyy}<br/>
               <b>End Date:</b> {end:dd MMM yyyy}</p>
            <p>You are eligible for discounts on up to your plan's maximum orders.</p>
            <p>Enjoy your membership!</p>
            <p>Best Regards,<br/>Cafe & Food Restaurant Team</p>
        ";

                SmtpClient smtp = new SmtpClient(); // Reads from Web.config
                smtp.Send(mail);
            }
        }
        void LoadPlanAmount()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = "SELECT Amount FROM MembershipPlans WHERE PlanID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", planID);

                con.Open();

                object amount = cmd.ExecuteScalar();

                lblAmount.Text = "Amount to Pay: ₹ " + amount;
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {

            if (Session["CustomerID"] == null)
            {
                lblMsg.Text = "Please login first.";
                return;
            }

            int customerID = Convert.ToInt32(Session["CustomerID"]);

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"INSERT INTO Membership
                                (CustomerID,MembershipAmount,StartDate,EndDate,
                                OrdersUsed,MaxOrders,DiscountPercent,IsActive)

                                SELECT
                                @cid,
                                Amount,
                                GETDATE(),
                                DATEADD(MONTH,DurationMonths,GETDATE()),
                                0,
                                MaxOrders,
                                DiscountPercent,
                                1
                                FROM MembershipPlans
                                WHERE PlanID=@pid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@cid", customerID);
                cmd.Parameters.AddWithValue("@pid", planID);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMsg.Text = "Payment Successful! Membership Activated.";
                SendMembershipActivationEmail(customerID, amount: Convert.ToDecimal(lblAmount.Text.Split('₹')[1].Trim()), start: DateTime.Now, end: DateTime.Now.AddMonths(2));
            }
        }
    }
}