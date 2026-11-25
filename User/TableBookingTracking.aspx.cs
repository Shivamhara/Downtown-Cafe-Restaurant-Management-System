using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class TableBookingTrack : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            lblMessage.Text = ""; // ✅ clear message

            string txn = txtTxnID.Text.Trim();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT BookingDate, ArrivalTime, NoOfPersons,
                        TotalAmount, AdvanceAmount, BookingStatus
                        FROM TableBookings
                        WHERE TransactionID=@txn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@txn", txn);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pnlBookingDetails.Visible = true;

                    decimal total = Convert.ToDecimal(dr["TotalAmount"]);
                    decimal advance = Convert.ToDecimal(dr["AdvanceAmount"]);
                    decimal remaining = total - advance;

                    litBookingDetails.Text =
                    "<b>Booking Date:</b> " + Convert.ToDateTime(dr["BookingDate"]).ToShortDateString() + "<br/>" +
                    "<b>Arrival Time:</b> " + dr["ArrivalTime"] + "<br/>" +
                    "<b>No Of Persons:</b> " + dr["NoOfPersons"] + "<br/>" +
                    "<b>Total Amount:</b> ₹" + total + "<br/>" +
                    "<b>Advance Paid:</b> ₹" + advance + "<br/>" +
                    "<b>Remaining:</b> <span style='color:red;font-weight:bold;'>₹" + remaining + "</span><br/>";

                    //ResetSteps();

                    string status = dr["BookingStatus"].ToString();


                    // Display status with color
                    if (status == "Pending")
                    {
                        lblStatus.Text = "⏳ Status: Pending Approval";
                        lblStatus.CssClass = "text-warning fw-bold";
                    }
                    else if (status == "Approved")
                    {
                        lblStatus.Text = "✅ Status: Approved by Admin";
                        lblStatus.CssClass = "text-primary fw-bold";
                    }
                    else if (status == "Confirmed")
                    {
                        lblStatus.Text = "🎉 Status: Booking Confirmed";
                        lblStatus.CssClass = "text-success fw-bold";
                    }
                    else if (status == "Rejected")
                    {
                        lblStatus.Text = "❌ Status: Rejected";
                        lblStatus.CssClass = "text-danger fw-bold";
                    }
                    // REJECTED
                    if (status == "Rejected")
                    {
                        lblMessage.Text = "❌ Your booking was rejected by admin.";
                    }
                }
                else
                {
                    pnlBookingDetails.Visible = false;
                    lblMessage.Text = "❌ Booking not found!";
                }
            }
        }
        //void ResetSteps()
        //{
        //    stepRequested.Attributes["class"] = "tracking-step requested";
        //    stepAdvance.Attributes["class"] = "tracking-step advance";
        //    stepConfirmed.Attributes["class"] = "tracking-step confirmed";
        //    stepArrival.Attributes["class"] = "tracking-step arrival";
        //}
    }
}