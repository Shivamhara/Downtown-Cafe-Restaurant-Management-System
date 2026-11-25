using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders(); // ✅ Load today's orders
            }
        }

        void LoadOrders()
        {
            string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                // ✅ Works with nvarchar date like "Mar 17 2026 11:45AM"
                string query = @"
                SELECT 
                    O.OrderID,
                    C.FullName,
                    O.FinalAmount,
                    O.OrderStatus,
                    O.PaymentStatus,
                    O.OrderDate
                FROM Orders O
                INNER JOIN Customers C 
                    ON O.CustomerID = C.CustomerID
                WHERE 
                    O.OrderDate LIKE '%' + FORMAT(GETDATE(), 'dd MMM yyyy') + '%'
                ORDER BY O.OrderID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
    }
}