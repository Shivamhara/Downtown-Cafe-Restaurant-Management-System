using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class UserList : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        void LoadUsers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT c.CustomerID, c.FullName, c.Email, c.IsActive, c.RegistrationDate,
                                 ISNULL(m.IsActive, 0) AS MembershipStatus,
                                 (SELECT COUNT(*) FROM Orders o WHERE o.CustomerID = c.CustomerID) AS TotalOrders
                                 FROM Customers c
                                 LEFT JOIN Membership m ON c.CustomerID = m.CustomerID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptUsers.DataSource = dt;
                    rptUsers.DataBind();
                }
                else
                {
                    lblMsg.Text = "No users found.";
                }
            }
        }
    }
}