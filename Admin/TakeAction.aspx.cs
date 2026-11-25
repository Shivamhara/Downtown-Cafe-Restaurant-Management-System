using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace CafeandfoodRestaurant.Admin
{
    public partial class TakeAction : System.Web.UI.Page
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
                string query = @"SELECT o.OrderID, o.CustomerID, c.FullName CustomerName, o.OrderDate, c.Email, o.FinalAmount, o.OrderStatus, o.PaymentStatus
                         FROM Orders o
                         INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                         ORDER BY o.OrderDate DESC"; // latest first
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
        protected void UpdateStatus_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.Button)sender;
            string newStatus = btn.CommandArgument;

            if (ViewState["SelectedOrderID"] == null)
            {
                lblOrderInfo.Text = "⚠️ Please select an order first by clicking 📍 Track.";
                return;
            }

            int orderID = (int)ViewState["SelectedOrderID"];

            using (SqlConnection con = new SqlConnection(cs))
            {
                // Update order status
                string query = "UPDATE Orders SET OrderStatus=@status WHERE OrderID=@oid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@oid", orderID);
                con.Open();
                cmd.ExecuteNonQuery();

                // Get customer email and name
                string emailQuery = @"SELECT c.Email, c.FullName 
                              FROM Customers c
                              INNER JOIN Orders o ON o.CustomerID = c.CustomerID
                              WHERE o.OrderID=@oid";
                SqlCommand emailCmd = new SqlCommand(emailQuery, con);
                emailCmd.Parameters.AddWithValue("@oid", orderID);
                SqlDataReader dr = emailCmd.ExecuteReader();
                if (dr.Read())
                {
                    string customerEmail = dr["Email"].ToString();
                    string customerName = dr["FullName"].ToString();

                    // Send email with customer name
                    SendEmail(customerEmail, customerName, orderID.ToString(), newStatus);
                }
            }

            lblOrderInfo.Text = $"✅ Order #{orderID} status updated to '{newStatus}'";

            LoadOrders();
        }
        protected void gvOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserDetails")
            {
                int customerID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"SELECT FullName, Email, Phone, Address 
                         FROM Customers
                         WHERE CustomerID=@cid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@cid", customerID);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblOrderInfo.Text = $"👤 Customer Info: {dr["FullName"]} | Email: {dr["Email"]} | Phone: {dr["Phone"]} | Address: {dr["Address"]}";
                    }
                }
            }
            if (e.CommandName == "OrderDetails")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
SELECT M.MenuName, OD.Quantity, OD.Price, O.FinalAmount
FROM Orders O
INNER JOIN OrderDetails OD ON O.OrderID = OD.OrderID
INNER JOIN Menu M ON OD.MenuID = M.MenuID
WHERE O.OrderID = @oid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@oid", orderID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        lblOrderInfo.Text = $"🍔 Order #{orderID} Details:<br/>" +
                                            $"{dr["Items"]} <br/> Total Amount: ₹{dr["FinalAmount"]}";
                    }
                }
            }
            if (e.CommandName == "TrackOrder")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);
                ViewState["SelectedOrderID"] = orderID; // store selected order

                // Get order info (optional)
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"SELECT o.OrderID, c.FullName, o.OrderStatus 
                             FROM Orders o
                             INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                             WHERE o.OrderID=@oid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@oid", orderID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblOrderInfo.Text = $"📍 Selected Order #{dr["OrderID"]} | Customer: {dr["FullName"]} | Current Status: {dr["OrderStatus"]}";
                    }
                }
            }

            if (e.CommandName == "UpdateStatus")
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                int orderID = Convert.ToInt32(args[0]);
                string status = args[1];

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "UPDATE Orders SET OrderStatus=@status WHERE OrderID=@oid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@oid", orderID);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Get customer email and name
                    string emailQuery = @"SELECT c.Email, c.FullName 
                              FROM Customers c
                              INNER JOIN Orders o ON o.CustomerID = c.CustomerID
                              WHERE o.OrderID=@oid";
                    SqlCommand emailCmd = new SqlCommand(emailQuery, con);
                    emailCmd.Parameters.AddWithValue("@oid", orderID);
                    SqlDataReader dr = emailCmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string customerEmail = dr["Email"].ToString();
                        string customerName = dr["FullName"].ToString();
                
                        // Send email with name
                        SendEmail(customerEmail, orderID.ToString(), customerName, status);
                    }
                }

                // Reload Grid
                LoadOrders();
            }
        }

        void SendEmail(string toEmail, string customerName, string orderID, string status)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
            mail.To.Add(toEmail);
            mail.Subject = "Order Status Update";

            mail.Body = $"Dear {customerName},\n\n" +
                        $"Your order '#{orderID}' is now '{status}'.\n\n" +
                        $"Thank you for ordering with us!\n\n" +
                        $"Regards,\nCafe&Food";

            SmtpClient smtp = new SmtpClient(); // Reads configuration from Web.config

            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {
                // Optional: log error
            }
        }
    }
}