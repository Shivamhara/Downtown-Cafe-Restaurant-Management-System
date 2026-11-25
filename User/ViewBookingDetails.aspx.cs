using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewBookingDetails : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBookings();
            }
            // 🔥 HANDLE DELETE POSTBACK
         

        }
        private void DeleteBookingPermanently(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string checkQuery = "SELECT BookingStatus FROM TableBookings WHERE BookingID=@BookingID";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, con);
                cmdCheck.Parameters.AddWithValue("@BookingID", bookingId);

                con.Open();
                string status = cmdCheck.ExecuteScalar()?.ToString();

                // ❌ BLOCK DELETE IF NOT REJECTED OR CANCELLED
                if (status != "Rejected" && status != "Cancelled")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "Swal.fire('Not Allowed','Only Rejected or Cancelled bookings can be deleted.','warning');", true);
                    con.Close();
                    return;
                }

                // ✅ DELETE
                string deleteQuery = "DELETE FROM TableBookings WHERE BookingID=@BookingID";
                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindBookings();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "Swal.fire('Deleted','Booking deleted successfully.','success');", true);
        }
        protected void btnUpdateBooking_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE TableBookings 
                         SET BookingDate = @Date
                         WHERE BookingID = @BookingID";

                SqlCommand cmd = new SqlCommand(query, con);

                // ✅ Proper date conversion
                DateTime arrivalDate = DateTime.Parse(txtArrivalDate.Text);

                cmd.Parameters.Add("@Date", SqlDbType.Date).Value = arrivalDate;
                cmd.Parameters.AddWithValue("@BookingID", hfBookingID.Value);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindBookings();

            txtArrivalDate.Text = "";
        }
        private void LoadBookingForEdit(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT b.BookingID, b.TableID, b.BookingTime, b.BookingDate, t.TableType
                 FROM TableBookings b
                 INNER JOIN TableMaster t ON b.TableID = t.TableID
                 WHERE b.BookingID=@BookingID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hfBookingID.Value = dr["BookingID"].ToString();
                    txtBookingTime.Text = dr["BookingTime"].ToString();

                    // ✅ FIX: SET DATE
                    if (dr["BookingDate"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(dr["BookingDate"]);
                        txtArrivalDate.Text = dt.ToString("yyyy-MM-dd");
                    }
                   // txtArrivalDate.Text = dt.ToString("yyyy-MM-dd");

                    string currentTableID = dr["TableID"].ToString();
                    dr.Close(); // <-- CLOSE the reader before running another command

                    // Load all table types
                    ddlTableType.Items.Clear();
                    using (SqlCommand cmdTables = new SqlCommand("SELECT TableID, TableType FROM TableMaster", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmdTables);
                        DataTable dtTables = new DataTable();
                        da.Fill(dtTables);

                        ddlTableType.DataSource = dtTables;
                        ddlTableType.DataTextField = "TableType";
                        ddlTableType.DataValueField = "TableID";
                        ddlTableType.DataBind();
                    }

                    ddlTableType.SelectedValue = currentTableID; // select current type
                }
                con.Close();
            }
        }
        private void BindBookings()
        {
            string userId = Session["CustomerID"]?.ToString();
            if (string.IsNullOrEmpty(userId)) return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    b.BookingID,
    t.TableType, 
    b.BookingDate,
    CONVERT(varchar, b.BookingTime, 100) AS BookingTime,
    b.BookingStatus
FROM TableBookings b
INNER JOIN TableMaster t ON b.TableID = t.TableID
WHERE b.CustomerID=@CustomerID";
SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@CustomerID", userId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }
        private void CancelBooking(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string checkQuery = "SELECT BookingStatus FROM TableBookings WHERE BookingID=@BookingID";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, con);
                cmdCheck.Parameters.AddWithValue("@BookingID", bookingId);

                con.Open();
                string status = cmdCheck.ExecuteScalar()?.ToString();

                // ❌ BLOCK IF ACCEPTED
                if (status == "Accepted")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "Swal.fire('Not Allowed','Booking already accepted. You cannot cancel now.','warning');", true);
                    con.Close();
                    return;
                }

                // 🔥 IF REJECTED → DELETE OPTION
                if (status == "Rejected")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmDelete",
        $@"Swal.fire({{
    title:'Delete Booking?',
    text:'Rejected booking can be deleted. Continue?',
    icon:'warning',
    showCancelButton:true,
    confirmButtonText:'Yes, Delete'
}}).then((result)=>{{
    if(result.isConfirmed){{
        document.getElementById('hfDeleteBookingID').value = '{bookingId}';
        document.getElementById('btnDeleteBooking').click();
    }}
}});", true);

                    con.Close();
                    return;
                }

                // ✅ OTHERWISE → CANCEL (Pending only)
                string updateQuery = "UPDATE TableBookings SET BookingStatus='Cancelled' WHERE BookingID=@BookingID";
                SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);
                cmdUpdate.Parameters.AddWithValue("@BookingID", bookingId);

                cmdUpdate.ExecuteNonQuery();
                con.Close();

                BindBookings();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "Swal.fire('Cancelled','Your booking has been cancelled.','success');", true);
            }
        }
        protected void btnDeleteBooking_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfDeleteBookingID.Value))
            {
                int bookingId = Convert.ToInt32(hfDeleteBookingID.Value);
                DeleteBookingPermanently(bookingId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "err",
                    "Swal.fire('Error','Booking ID missing!','error');", true);
            }
        }
        protected void btnResetForm_Click(object sender, EventArgs e)
        {
            hfBookingID.Value = "";
            txtBookingTime.Text = "";
            txtArrivalDate.Text = "";
            ddlTableType.SelectedIndex = 0;
        }
        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                LoadBookingForEdit(bookingId);
            }
            else if (e.CommandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                CancelBooking(bookingId);
            }
            else if (e.CommandName == "DeleteBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmDelete",
                $@"Swal.fire({{
            title:'Delete Booking?',
            text:'Are you sure you want to delete?',
            icon:'warning',
            showCancelButton:true,
            confirmButtonText:'Yes, Delete'
        }}).then((result)=>{{
            if(result.isConfirmed){{
                document.getElementById('hfDeleteBookingID').value = '{bookingId}';
                document.getElementById('btnDeleteBooking').click();
            }}
        }});", true);
            }
        }
    }
}