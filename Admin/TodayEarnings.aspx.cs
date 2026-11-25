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
    public partial class TodayEarnings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTodayEarnings();
            }
        }
        void GetTodayEarnings()
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ISNULL(SUM(Amount),0) FROM Payments WHERE CAST(PaymentDate AS DATE)=CAST(GETDATE() AS DATE) AND PaymentStatus='Success'";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                lblToday.Text = "₹ " + cmd.ExecuteScalar().ToString();
            }
        }
    }
}