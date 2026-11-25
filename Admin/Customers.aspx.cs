using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers(); // 🔥 important
            }
        }

        void LoadCustomers()
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT FullName, Email, Phone, City FROM Customers";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                gvCustomers.DataSource = dt;
                gvCustomers.DataBind();
            }
        }
    }
}