using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace CafeandfoodRestaurant.Admin
{
    public partial class ViewOrders : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        void LoadOrders()
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"SELECT 
o.OrderID,
o.CustomerID,
c.FullName CustomerName,
o.OrderDate,
o.FinalAmount,
o.OrderStatus,
o.PaymentStatus
FROM Orders o
INNER JOIN Customers c
ON o.CustomerID = c.CustomerID
WHERE o.VisibleAdmin = 0 ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT 
                            o.OrderID,
                            o.CustomerID,
                            c.FullName AS CustomerName,
                            o.OrderDate,
                            o.FinalAmount,
                            o.OrderStatus,
                            o.PaymentStatus
                         FROM Orders o
                         INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                         WHERE o.VisibleAdmin = 0
                         AND (
                            CAST(o.OrderID AS NVARCHAR) LIKE @kw OR
                            c.FullName LIKE @kw OR
                            c.Email LIKE @kw OR
                            o.OrderStatus LIKE @kw OR
                            o.PaymentStatus LIKE @kw OR
                            CAST(o.FinalAmount AS NVARCHAR) LIKE @kw
                         )
                         ORDER BY o.OrderDate DESC"; // latest first

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadOrders();
        }


        protected void gvOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "TrackOrder")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                // Redirect to TrackOrder.aspx with OrderID in query string
                Response.Redirect("TakeAction.aspx?oid=" + orderID);
            }
            if (e.CommandName == "DeleteOrder")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "UPDATE Orders SET VisibleAdmin = 1 WHERE OrderID=@oid";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@oid", orderID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadOrders();
            }
            if (e.CommandName == "UserDetails")
            {

                int customerID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {

                    string query = @"SELECT FullName,Email,Phone,Address 
                                    FROM Customers 
                                    WHERE CustomerID=@cid";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@cid", customerID);

                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {

                        lblUserDetails.Text = "<b>Name:</b> " + dr["FullName"] + "<br/>"
                            + "<b>Email:</b> " + dr["Email"] + "<br/>"
                            + "<b>Mobile:</b> " + dr["Phone"] + "<br/>"
                            + "<b>Address:</b> " + dr["Address"];
                    }

                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop",
                "var myModal = new bootstrap.Modal(document.getElementById('userModal')); myModal.show();", true);
            }



            if (e.CommandName == "OrderDetails")
            {

                int orderID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {

                    string query = @"SELECT 
                                    m.MenuName,
                                    od.Quantity,
                                    od.Price,
                                    od.TotalPrice
                                    FROM OrderDetails od
                                    INNER JOIN Menu m
                                    ON od.MenuID = m.MenuID
                                    WHERE od.OrderID=@oid";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    da.SelectCommand.Parameters.AddWithValue("@oid", orderID);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    gvOrderDetails.DataSource = dt;
                    gvOrderDetails.DataBind();

                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2",
                "var myModal = new bootstrap.Modal(document.getElementById('orderModal')); myModal.show();", true);
            }

        }

    }
}