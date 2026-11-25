using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Net.Mail;
using System.Net;

namespace CafeandfoodRestaurant.User
{
    public partial class TablePayment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["txn"] == null)
                    Response.Redirect("BookTable.aspx");

                string txn = Request.QueryString["txn"];
                lblTxnID.Text = txn;
                LoadAmount(txn);
            }
        }
        protected void PaymentType_Changed(object sender, EventArgs e)
        {
            decimal total = Convert.ToDecimal(ViewState["TotalAmount"]);

            if (rdoAdvance.Checked)
            {
                lblAmount.Text = (total / 2).ToString();
            }
            else if (rdoFull.Checked)
            {
                lblAmount.Text = total.ToString();
            }
        }
        void LoadAmount(string txn)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT TotalAmount, AdvanceAmount, RemainingAmount FROM TableBookings WHERE TransactionID=@txn";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    decimal total = Convert.ToDecimal(dr["TotalAmount"]);
                    decimal remaining = Convert.ToDecimal(dr["RemainingAmount"]);

                    ViewState["TotalAmount"] = total;

                    if (remaining == 0)
                    {
                        lblAmount.Text = "0";
                        btnPayNow.Enabled = false;
                    }
                    else
                    {
                        lblAmount.Text = remaining.ToString();
                    }
                }
            }
        }
        void SendEmail(string toEmail, string txnID, string paidAmount, string remainingAmount)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
                mail.To.Add(toEmail);

                mail.Subject = "Table Booking Payment Confirmation";

                mail.IsBodyHtml = true;

                mail.Body = @"
<h2 style='color:green;'>Payment Successful</h2>

<p>Dear Customer,</p>

<p>Your table booking payment was successful.</p>

<p><b>Transaction ID:</b> " + txnID + @"</p>

<p><b>Paid Amount:</b> ₹" + paidAmount + @"</p>

<p><b>Remaining Amount:</b> ₹" + remainingAmount + @"</p>" +

                (remainingAmount == "0"
                ? "<p>Your table booking is <b>confirmed</b>.</p>"
                : "<p>Your booking is partially paid. Please pay remaining amount before arrival.</p>") +

                @"<br/>

<p>Thank you for choosing <b>Cafe&Food</b>.</p>
<p>Regards,<br/>Cafe&Food Team</p>";
                SmtpClient smtp = new SmtpClient(); // Uses Web.config SMTP
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void btnPayNow_Click(object sender, EventArgs e)
        {
            string txn = lblTxnID.Text;

            string paymentMethod = "";

            if (rdoUPI.Checked) paymentMethod = "UPI";
            else if (rdoCard.Checked) paymentMethod = "Card";

            decimal totalAmount = 0;
            decimal advanceAmount = 0;
            decimal remainingAmount = 0;
            string customerEmail = "";

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TotalAmount, AdvanceAmount,
                        (SELECT Email FROM Customers WHERE CustomerID = TableBookings.CustomerID) AS Email
                        FROM TableBookings
                        WHERE TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    totalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    advanceAmount = Convert.ToDecimal(dr["AdvanceAmount"]);
                    customerEmail = dr["Email"].ToString();
                }
            }

            // CALCULATE REMAINING
            remainingAmount = totalAmount - advanceAmount;

            // UPDATE DATABASE AFTER PAYMENT
            using (SqlConnection con = new SqlConnection(cs))
            {
                string updateBooking = "";
                SqlCommand cmd;

                if (rdoAdvance.Checked)
                {
                    updateBooking = @"UPDATE TableBookings
        SET 
            AdvanceAmount = @pay,
            RemainingAmount = TotalAmount - @pay,
            PaymentStatus = 'Partial',
            BookingStatus = 'Approved',
            PaymentMethod = @pm
        WHERE TransactionID=@txn";

                    cmd = new SqlCommand(updateBooking, con);

                    // ✅ IMPORTANT: PASS @pay VALUE
                    decimal payAmount = Convert.ToDecimal(lblAmount.Text);
                    cmd.Parameters.AddWithValue("@pay", payAmount);
                }
                else
                {
                    updateBooking = @"UPDATE TableBookings
        SET 
            AdvanceAmount = TotalAmount,
            RemainingAmount = 0,
            PaymentStatus = 'Paid',
            BookingStatus = 'Confirmed',
            PaymentMethod = @pm
        WHERE TransactionID=@txn";

                    cmd = new SqlCommand(updateBooking, con);
                }

                cmd.Parameters.AddWithValue("@pm", paymentMethod);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            // SEND EMAIL
            if (!string.IsNullOrEmpty(customerEmail))
            {
                decimal paidAmount = Convert.ToDecimal(lblAmount.Text);

                // calculate NEW remaining after payment
                decimal newRemaining;

                if (rdoAdvance.Checked)
                {
                    newRemaining = totalAmount - paidAmount;
                }
                else
                {
                    newRemaining = 0;
                }

                SendEmail(customerEmail, txn, paidAmount.ToString(), newRemaining.ToString());
            }

            ScriptManager.RegisterStartupScript(this, GetType(),
                "popup", "alert('✅ Payment successful! Confirmation email sent.');", true);
        }
    }
}