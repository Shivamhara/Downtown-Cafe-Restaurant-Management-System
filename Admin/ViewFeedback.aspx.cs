using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewFeedback : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFeedback();
            }
        }

        void LoadFeedback()
        {
            if (Request.QueryString["menuid"] == null) return;

            int menuid = Convert.ToInt32(Request.QueryString["menuid"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT c.FullName AS FullName,
                        f.Rating,
                        f.Comment,
                        f.RatingDate
                 FROM Ratings f
                 INNER JOIN Customers c ON f.UserID = c.CustomerID
                 WHERE f.MenuID = @mid
                 ORDER BY f.RatingDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mid", menuid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvFeedback.DataSource = dt;
                gvFeedback.DataBind();
            }
        }
    }
}