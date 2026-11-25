using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class TablePayment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        void LoadBookings()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT b.BookingID, b.CustomerID, t.TableType, b.NoOfPersons,
                           b.TotalAmount, b.AdvanceAmount, b.RemainingAmount, b.BookingStatus
                    FROM TableBookings b
                    INNER JOIN TableMaster t ON b.TableID = t.TableID
                    ORDER BY b.BookingDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PayRemaining")
            {
                int bookingID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    // Fetch remaining amount
                    string selectQuery = "SELECT RemainingAmount FROM TableBookings WHERE BookingID=@bid";
                    SqlCommand cmdSelect = new SqlCommand(selectQuery, con);
                    cmdSelect.Parameters.AddWithValue("@bid", bookingID);
                    con.Open();
                    decimal remaining = Convert.ToDecimal(cmdSelect.ExecuteScalar());

                    if (remaining <= 0)
                    {
                        lblMessage.Text = "⚠️ No remaining payment for this booking.";
                        return;
                    }

                    // Update booking as fully paid
                    string updateQuery = @"
                        UPDATE TableBookings
                        SET AdvanceAmount = AdvanceAmount + @rem,
                            RemainingAmount = 0,
                            BookingStatus = 'Paid'
                        WHERE BookingID = @bid";

                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);
                    cmdUpdate.Parameters.AddWithValue("@rem", remaining);
                    cmdUpdate.Parameters.AddWithValue("@bid", bookingID);
                    cmdUpdate.ExecuteNonQuery();

                    lblMessage.Text = $"✅ Remaining payment of ₹{remaining} marked as paid for Booking #{bookingID}";
                    LoadBookings();
                }
            }
        }
    }
}