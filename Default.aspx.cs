using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CafeandfoodRestaurant
{
    public partial class Default : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
            ClassCheck cs = new ClassCheck();
            cs.lis(Server.MapPath("HomemasterPage.master"), Server.MapPath("HomemasterPage.master.cs"), Server.MapPath("web.config"));



        }

        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = @"SELECT Top(9) CategoryID, CategoryName, ImagePath
                         FROM Categories 
                         WHERE IsActive=1";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
        }
    }
}