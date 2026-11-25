using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewMenu : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindCategories();

                BindMenu();

            }
        }
        void BindCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT DISTINCT Category FROM Menu";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category";
                ddlCategory.DataBind();

                // ✅ Add All option
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Categories", "All"));
            }
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenu();
        }
        void BindMenu()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd;

                if (ddlCategory.SelectedValue == "All")
                {
                    // ✅ Show all
                    cmd = new SqlCommand("SELECT * FROM Menu", con);
                }
                else
                {
                    // ✅ Filter by category TEXT
                    cmd = new SqlCommand("SELECT * FROM Menu WHERE Category = @Category", con);
                    cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMenu.DataSource = dt;
                rptMenu.DataBind();
            }
        }
    }
}