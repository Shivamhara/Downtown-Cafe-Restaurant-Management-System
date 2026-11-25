using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CafeandfoodRestaurant.Admin
{
    public partial class TakeActionTableOrder : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        // Load all bookings
        void LoadBookings()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    TB.TransactionID, 
    C.FullName AS CustomerName,
    TB.BookingDate, 
    TB.BookingTime,
    TB.BookingStatus,TB.BookingTime,
    TB.RemainingAmount,
    TB.PaymentStatus
FROM TableBookings TB
INNER JOIN Customers C ON TB.CustomerID = C.CustomerID
WHERE TB.VisibleAdmin = 0";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    TB.TransactionID, 
    C.FullName AS CustomerName,
    TB.BookingDate, 
    TB.BookingTime,
    TB.BookingStatus,
    TB.RemainingAmount,
    TB.PaymentStatus
FROM TableBookings TB
INNER JOIN Customers C ON TB.CustomerID = C.CustomerID
WHERE TB.VisibleAdmin = 0
AND (
    CAST(TB.TransactionID AS NVARCHAR) LIKE @kw OR
    C.FullName LIKE @kw OR
    CAST(TB.BookingDate AS NVARCHAR) LIKE @kw OR
    TB.BookingTime LIKE @kw OR
    TB.BookingStatus LIKE @kw OR
    CAST(TB.RemainingAmount AS NVARCHAR) LIKE @kw OR
    TB.PaymentStatus LIKE @kw
)
ORDER BY TB.BookingDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadBookings(); // reload all bookings
        }
        // Show/Hide action buttons based on status
        protected void gvBookings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "BookingStatus").ToString();
                decimal remaining = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RemainingAmount"));
                string paymentStatus = DataBinder.Eval(e.Row.DataItem, "PaymentStatus").ToString();

                Button btnApprove = (Button)e.Row.FindControl("btnApprove");
                Button btnReject = (Button)e.Row.FindControl("btnReject");
                Button btnPayRemaining = (Button)e.Row.FindControl("btnPayRemaining");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                // Hide all action buttons if payment is fully paid or booking is confirmed
                if (paymentStatus == "Paid" || status == "Confirmed" || status == "Rejected")
                {
                    if (btnApprove != null) btnApprove.Visible = false;
                    if (btnReject != null) btnReject.Visible = false;
                    if (btnPayRemaining != null) btnPayRemaining.Visible = false;
                }
                else
                {
                    // Pending bookings: Approve/Reject visible, Pay Remaining hidden
                    if (status == "Pending")
                    {
                        if (btnApprove != null) btnApprove.Visible = true;
                        if (btnReject != null) btnReject.Visible = true;
                        if (btnPayRemaining != null) btnPayRemaining.Visible = false;
                    }
                    // Approved bookings: Pay Remaining only if partial
                    else if (status == "Approved")
                    {
                        if (btnApprove != null) btnApprove.Visible = false;
                        if (btnReject != null) btnReject.Visible = false;

                        if (btnPayRemaining != null)
                            btnPayRemaining.Visible = (paymentStatus == "Partial" && remaining > 0);
                    }
                }
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string txn = gvBookings.DataKeys[row.RowIndex].Value.ToString();

            UpdateStatus(txn, "Approved");
            SendBookingEmail(txn, "Approved");
            LoadBookings();
            ShowAlert("Booking Approved and Email Sent!");
        }

        // Reject booking
        protected void btnReject_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string txn = gvBookings.DataKeys[row.RowIndex].Value.ToString();

            UpdateStatus(txn, "Rejected");
            SendBookingEmail(txn, "Rejected");
            LoadBookings();
            ShowAlert("Booking Rejected and Email Sent!");
        }

        // Pay remaining
        protected void btnPayRemaining_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string txn = gvBookings.DataKeys[row.RowIndex].Value.ToString();

            PayRemainingAmount(txn);
            SendFullPaymentEmail(txn);
            LoadBookings();
            ShowAlert("Remaining Amount Paid and Confirmation Email Sent!");
            
        }

        // Delete booking
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string txn = gvBookings.DataKeys[row.RowIndex].Value.ToString();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string checkQuery = @"SELECT BookingStatus, PaymentStatus 
                              FROM TableBookings 
                              WHERE TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(checkQuery, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string status = dr["BookingStatus"].ToString();
                    string payment = dr["PaymentStatus"].ToString();

                    // ❌ BLOCK DELETE
                    if (status == "Approved" || status == "Confirmed" || payment == "Partial" || payment == "Paid")
                    {
                        ShowAlert("❌ Cannot delete this booking (Already Approved or Payment Started)");
                        return;
                    }
                }
                con.Close();
            }

            // ✅ ALLOW DELETE
            DeleteBooking(txn);
            LoadBookings();
            ShowAlert("✅ Booking Deleted!");
        }
        void UpdateStatus(string txn, string status)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE TableBookings SET BookingStatus=@status WHERE TransactionID=@txn";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void DeleteBooking(string txn)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM TableBookings WHERE TransactionID=@txn";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void PayRemainingAmount(string txn)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
UPDATE TableBookings
SET 
    RemainingAmount = 0,
    PaymentStatus = 'Paid',
    BookingStatus = 'Confirmed'
WHERE TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void SendBookingEmail(string txn, string status)
        {
            string email = "", name = "", date = "", time = "", tableType = "";
            decimal amount = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    C.Email,
    C.FullName,
    TB.BookingDate,
    TB.BookingTime,
    TM.TableType,
    TM.AmountPerTable
FROM TableBookings TB
INNER JOIN Customers C ON TB.CustomerID = C.CustomerID
INNER JOIN TableMaster TM ON TB.TableID = TM.TableID
WHERE TB.TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    email = dr["Email"].ToString();
                    name = dr["FullName"].ToString();
                    date = Convert.ToDateTime(dr["BookingDate"]).ToString("dd MMM yyyy");
                    time = dr["BookingTime"].ToString();
                    tableType = dr["TableType"].ToString();
                    amount = Convert.ToDecimal(dr["AmountPerTable"]);
                }
            }

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
            mail.To.Add(email);
            mail.Subject = "Table Booking " + status;
            mail.IsBodyHtml = true;

            string color = status == "Approved" ? "green" : "red";
            string message = status == "Approved"
                ? "Your table booking has been successfully approved. Please visit us on the booking date."
                : "We regret to inform you that your table booking has been rejected. You may contact us for further details.";

            mail.Body = $@"
<h2 style='color:{color};'>Booking {status}</h2>
<p>Dear <b>{name}</b>,</p>
<p>{message}</p>
<hr/>
<h4>📋 Booking Details:</h4>
<p><b>Transaction ID:</b> {txn}</p>
<p><b>Date:</b> {date}</p>
<p><b>Time:</b> {time}</p>
<p><b>Table Type:</b> {tableType}</p>
<p><b>Amount:</b> ₹{amount}</p>
<br/>
<p>Thank you for choosing <b>Cafe&Food</b>.</p>
<p>Regards,<br/>Cafe&Food Team</p>";

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }

        void SendFullPaymentEmail(string txn)
        {
            string email = "";
            decimal totalAmount = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    C.Email,
    TB.TotalAmount
FROM TableBookings TB
INNER JOIN Customers C ON TB.CustomerID = C.CustomerID
WHERE TB.TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    email = dr["Email"].ToString();
                    totalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                }
            }

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
            mail.To.Add(email);
            mail.Subject = "Full Payment Confirmation";
            mail.IsBodyHtml = true;

            mail.Body = $@"
<h2 style='color:green;'>✅ Full Payment Received</h2>
<p>Dear Customer,</p>
<p>Your table booking payment has been completed successfully.</p>
<p><b>Transaction ID:</b> {txn}</p>
<p><b>Total Paid:</b> ₹{totalAmount}</p>
<p><b>Status:</b> Confirmed</p>
<br/>
<p>Thank you for choosing <b>Cafe&Food</b>.</p>
<p>Regards,<br/>Cafe&Food Team</p>";

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }

        void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{msg}');", true);
        }
    }
}