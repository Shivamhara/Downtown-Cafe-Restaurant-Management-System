using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class UserDetails : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    LoadUserDetails(Request.QueryString["id"]);
                }
            }
        }

        void LoadUserDetails(string id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT c.FullName, c.Email, c.IsActive, c.RegistrationDate,
                                 ISNULL(m.IsActive, 0) AS MembershipStatus,
                                 (SELECT COUNT(*) FROM Orders o WHERE o.CustomerID = c.CustomerID) AS TotalOrders
                                 FROM Customers c
                                 LEFT JOIN Membership m ON c.CustomerID = m.CustomerID
                                 WHERE c.CustomerID=@id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblName.Text = dr["FullName"].ToString();
                    lblEmail.Text = dr["Email"].ToString();
                    lblDate.Text = Convert.ToDateTime(dr["RegistrationDate"]).ToString("dd MMM yyyy");

                    bool isActive = Convert.ToBoolean(dr["IsActive"]);
                    lblStatus.Text = isActive ? "<span class='badge-active'>Active</span>"
                                              : "<span class='badge-inactive'>Inactive</span>";

                    lblMembership.Text = dr["MembershipStatus"].ToString() == "1" ? "Active" : "Inactive";
                    lblOrders.Text = dr["TotalOrders"].ToString();
                }

                dr.Close();
            }
        }
    }
}