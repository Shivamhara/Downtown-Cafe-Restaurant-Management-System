using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant
{
    public partial class Menu : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMenu();
            }
        }

        void LoadMenu()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT MenuName, Cost, Category,Description,ImagePath FROM Menu";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMenu.DataSource = dt;
                rptMenu.DataBind();
            }
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}