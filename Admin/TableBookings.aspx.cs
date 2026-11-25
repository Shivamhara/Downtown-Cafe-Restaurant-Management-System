using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class TableBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookings(); // 🔥 important
            }
        }
        void LoadBookings()
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
      SELECT 
    B.BookingID,
    C.FullName,
    B.BookingDate,
    B.BookingStatus,
    B.TotalAmount,
    CASE 
        WHEN CAST(B.BookingDate AS DATE) = CAST(GETDATE() AS DATE) 
        THEN 'Today'
        ELSE 'Upcoming'
    END AS BookingType
FROM TableBookings B
INNER JOIN Customers C 
    ON B.CustomerID = C.CustomerID
ORDER BY B.BookingDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }
    }
}