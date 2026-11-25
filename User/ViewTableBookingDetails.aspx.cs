using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewTableBookingDetails : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMyBookings();
            }
        }
        protected void gvMyBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Pay")
            {
                string txn = e.CommandArgument.ToString();

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"SELECT BookingStatus, PaymentStatus 
                             FROM TableBookings 
                             WHERE TransactionID=@txn AND CustomerID=@cid";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@txn", txn);
                    cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string status = dr["BookingStatus"].ToString();
                        string payment = dr["PaymentStatus"].ToString();

                        // ❌ Already Paid
                        if (payment == "Paid")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "popup",
                                "alert('⚠️ Payment already completed for this booking.');",
                                true);
                            return;
                        }

                        // ❌ Not approved
                        if (status != "Approved")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "popup",
                                "alert('⚠️ Booking is not approved yet.');",
                                true);
                            return;
                        }

                        // ✅ Allowed
                        Response.Redirect("~/User/TablePayment.aspx?txn=" + txn);
                    }
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string txn = txtSearchTxn.Text.Trim();
            LoadMyBookings(txn);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchTxn.Text = "";
            LoadMyBookings();
        }
        void LoadMyBookings(string txnID = "")
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TransactionID, BookingDate, BookingTime,
                         BookingStatus, PaymentStatus
                         FROM TableBookings
                         WHERE CustomerID=@cid";

                // ✅ FILTER BY TRANSACTION ID
                if (!string.IsNullOrEmpty(txnID))
                {
                    query += " AND TransactionID LIKE @txn";
                }

                // ✅ LATEST FIRST
                query += " ORDER BY BookingDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cid", Session["CustomerID"]);

                if (!string.IsNullOrEmpty(txnID))
                {
                    cmd.Parameters.AddWithValue("@txn", "%" + txnID + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMyBookings.DataSource = dt;
                gvMyBookings.DataBind();
            }
        }

    
    }
}