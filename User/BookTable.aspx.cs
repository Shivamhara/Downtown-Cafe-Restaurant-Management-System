using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace CafeandfoodRestaurant.User
{
    public partial class BookTable : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;
        decimal selectedAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTableTypes();
                //LoadExistingBooking();
            }
        }

        void LoadTableTypes()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT TableID, TableType FROM TableMaster";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DropDownList1.DataSource = dt;
                DropDownList1.DataTextField = "TableType";
                DropDownList1.DataValueField = "TableID";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Table Type --", "0"));
            }
        }

        //void LoadExistingBooking()
        //{
        //    if (Session["CustomerID"] == null) return;

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        string query = @"SELECT TOP 1 BookingID, TableID, BookingTime, ArrivalTime, BookingStatus, PaymentStatus
        //                         FROM TableBookings
        //                         WHERE CustomerID=@cid 
        //                         AND BookingStatus='Pending'
        //                         ORDER BY CreatedDate DESC";

        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        if (dr.Read())
        //        {
        //            int bookingID = Convert.ToInt32(dr["BookingID"]);
        //            hfBookingID.Value = bookingID.ToString(); // persist BookingID
        //            DropDownList1.SelectedValue = dr["TableID"].ToString();
        //            txttime.Text = dr["BookingTime"].ToString();
        //            TextBox2.Text = Convert.ToDateTime(dr["ArrivalTime"]).ToString("yyyy-MM-ddTHH:mm");
        //            SetCost();

        //            Button1.Visible = false; // hide book button
        //            Button2.Visible = true;  // show update
        //            Button3.Visible = true;  // show cancel
        //            Panel1.Visible = false;  // hide payment
        //        }
        //        else
        //        {
        //            ResetForm();
        //        }
        //    }
        //}

        protected void ddlTableType_SelectedIndexChanged(object sender, EventArgs e) => SetCost();
        protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e) => SetCost();

        void SetCost()
        {
            Label2.Text = "";
            if (DropDownList1.SelectedValue == "0") return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT AmountPerTable FROM TableMaster WHERE TableID=@tid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@tid", DropDownList1.SelectedValue);
                con.Open();
                decimal amount = Convert.ToDecimal(cmd.ExecuteScalar());
                selectedAmount = amount;

                if (RadioButtonList1.SelectedValue == "Full")
                    Label2.Text = "Total Cost: ₹" + amount + " (Full Payment)";
                else
                    Label2.Text = "Advance (50%): ₹" + (amount / 2) + " | Remaining: ₹" + (amount / 2);
            }
        }

        protected void btnBookTable_Click(object sender, EventArgs e) => SaveBooking(true);
        protected void btnUpdate_Click(object sender, EventArgs e) => SaveBooking(false);

        void SaveBooking(bool isNew)
        {
            Label1.Text = "";
            if (Session["CustomerID"] == null) { Label1.Text = "⚠️ Please login first!"; return; }

            int customerID = Convert.ToInt32(Session["CustomerID"]);
            if (DropDownList1.SelectedValue == "0") { Label1.Text = "⚠️ Select table type!"; return; }

            int tableID = Convert.ToInt32(DropDownList1.SelectedValue);
           
            if (!DateTime.TryParse(TextBox2.Text.Trim(), out DateTime arrivalTime)) { Label1.Text = "⚠️ Enter valid arrival date."; return; }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                decimal totalAmount = Convert.ToDecimal(new SqlCommand("SELECT AmountPerTable FROM TableMaster WHERE TableID=@tid", con) { Parameters = { new SqlParameter("@tid", tableID) } }.ExecuteScalar());

                if (isNew)
                {
                    string txnID = customerID + tableID + Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
                    string insertQuery = @"INSERT INTO TableBookings
(CustomerID,TableID,BookingDate,BookingTime,
 TotalAmount,AdvanceAmount,RemainingAmount,
 BookingStatus,PaymentStatus,TransactionID,CreatedDate)
VALUES
(@cid,@tid,@bdate,@nop,
 @total,@adv,@rem,
 'Pending','Pending',@txn,@date)";

                    SqlCommand cmd = new SqlCommand(insertQuery, con);

                    cmd.Parameters.AddWithValue("@cid", customerID);
                    cmd.Parameters.AddWithValue("@tid", tableID);
                    cmd.Parameters.AddWithValue("@bdate", TextBox2.Text);
                   // cmd.Parameters.AddWithValue("@arrival", arrivalTime);
                    cmd.Parameters.AddWithValue("@nop", txttime.Text);
                    cmd.Parameters.AddWithValue("@total", totalAmount);

                    // ✅ FIXED PAYMENT LOGIC
                    cmd.Parameters.AddWithValue("@adv", 0);
                    cmd.Parameters.AddWithValue("@rem", totalAmount);

                    cmd.Parameters.AddWithValue("@txn", txnID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    string email = new SqlCommand("SELECT Email FROM Customers WHERE CustomerID=@cid", con) { Parameters = { new SqlParameter("@cid", customerID) } }.ExecuteScalar()?.ToString();
                    SendPaymentEmail(email, txnID);
                    Label1.Text = "✅ Booking request sent successfully!";
                }
                else
                {
                    if (!int.TryParse(hfBookingID.Value, out int bookingID)) { Label1.Text = "⚠️ Invalid booking!"; return; }

                    string status = new SqlCommand("SELECT BookingStatus FROM TableBookings WHERE BookingID=@bid", con) { Parameters = { new SqlParameter("@bid", bookingID) } }.ExecuteScalar()?.ToString();
                    if (status != "Pending") { Label1.Text = "⚠️ Cannot update approved booking!"; return; }

                    string updateQuery = @"UPDATE TableBookings SET TableID=@tid, ArrivalTime=@arrival, BookingTime=@nop, TotalAmount=@total WHERE BookingID=@bid";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@tid", tableID);
                    cmd.Parameters.AddWithValue("@arrival", arrivalTime);
                    cmd.Parameters.AddWithValue("@nop", txttime.Text);
                    cmd.Parameters.AddWithValue("@total", totalAmount);
                    cmd.Parameters.AddWithValue("@bid", bookingID);
                    cmd.ExecuteNonQuery();

                    Label1.Text = "✅ Booking updated successfully!";
                }

                //LoadExistingBooking();
            }
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (!int.TryParse(hfBookingID.Value, out int bookingID)) return;

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand cmdCheck = new SqlCommand("SELECT BookingStatus, PaymentStatus FROM TableBookings WHERE BookingID=@bid", con);
        //        cmdCheck.Parameters.AddWithValue("@bid", bookingID);
        //        SqlDataReader dr = cmdCheck.ExecuteReader();

        //        if (dr.Read())
        //        {
        //            string status = dr["BookingStatus"].ToString();
        //            string payment = dr["PaymentStatus"].ToString();
        //            dr.Close();

        //            if (status == "Approved" || payment == "Paid")
        //            {
        //                Label1.Text = "⚠️ Cannot cancel approved/paid booking!";
        //                return;
        //            }

        //            new SqlCommand("DELETE FROM TableBookings WHERE BookingID=@bid", con) { Parameters = { new SqlParameter("@bid", bookingID) } }.ExecuteNonQuery();
        //            Label1.Text = "✅ Booking canceled successfully!";
        //            ResetForm();
        //        }
        //    }
        //}

        void ResetForm()
        {
            DropDownList1.SelectedIndex = 0;
            txttime.Text = "";
            TextBox2.Text = "";
            Label2.Text = "";
            Button1.Visible = true;
            //Button2.Visible = false;
          //  Button3.Visible = false;
            hfBookingID.Value = "0";
        }
        protected void btnViewBookings_Click(object sender, EventArgs e)
        {
            // Redirect to the page that displays all bookings
            Response.Redirect("~/User/ViewBookingDetails.aspx");
        }
        void SendPaymentEmail(string email, string txnID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT 
                            t.TransactionID,
                            t.TotalAmount,
                            t.AdvanceAmount,
                            t.RemainingAmount,
                            t.PaymentMethod,
                            t.BookingTime,
                            tm.TableType
                         FROM TableBookings t
                         INNER JOIN TableMaster tm ON t.TableID = tm.TableID
                         WHERE t.TransactionID = @txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txnID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string tableType = dr["TableType"].ToString();
                    string bookingTime = dr["BookingTime"] == DBNull.Value
                        ? "Not Provided"
                        : dr["BookingTime"].ToString(); // since BookingTime is text, no need to convert
                    string total = dr["TotalAmount"].ToString();
                    string advance = dr["AdvanceAmount"].ToString();
                    string remaining = dr["RemainingAmount"].ToString();
                    string paymentMethod = dr["PaymentMethod"].ToString();

                    MailMessage mail = new MailMessage();
                    mail.To.Add(email);
                    mail.Subject = "Payment Confirmation - Cafe & Food Restaurant";

                    mail.Body = $@"
<div style='font-family:Segoe UI, Arial; color:#333; line-height:1.6;'>

<h2 style='color:#28a745;'>Payment Successful 🎉</h2>

<p>Dear Customer,</p>

<p>Your payment has been successfully completed. Below are your booking details:</p>

<table style='border-collapse:collapse; width:100%;'>
<tr><td><b>Transaction ID:</b></td><td>{txnID}</td></tr>
<tr><td><b>Table Type:</b></td><td>{tableType}</td></tr>
<tr><td><b>Booking Time:</b></td><td>{bookingTime}</td></tr>
<tr><td><b>Total Amount:</b></td><td>₹{total}</td></tr>
<tr><td><b>Paid Amount:</b></td><td>₹{advance}</td></tr>
<tr><td><b>Remaining Amount:</b></td><td>₹{remaining}</td></tr>
<tr><td><b>Payment Method:</b></td><td>{paymentMethod}</td></tr>
</table>

<br/>

<p>We look forward to serving you. Please carry this confirmation when you visit.</p>

<br/>

<p>Warm Regards,<br/>
<b>Cafe & Food Restaurant Team</b></p>

<hr/>
<p style='font-size:12px;color:gray;'>This is an automated email. Please do not reply.</p>

</div>";

                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("mycollegeproject02@gmail.com", "rzkh ekar ngzy vghw");
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }
            }
        }
    }
}