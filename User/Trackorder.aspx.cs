using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace CafeandfoodRestaurant.User
{
    public partial class Trackorder : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            int orderID;
            if (!int.TryParse(txtOrderID.Text.Trim(), out orderID))
            {
                lblMessage.Text = "⚠️ Please enter a valid Order ID.";
                pnlOrderDetails.Visible = false;
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT o.OrderID, o.OrderDate, o.FinalAmount, o.OrderStatus, o.PaymentStatus,
                   c.FullName AS CustomerName, c.Email
            FROM Orders o
            INNER JOIN Customers c ON o.CustomerID = c.CustomerID
            WHERE o.OrderID=@oid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@oid", orderID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (!dr.HasRows)
                {
                    lblMessage.Text = "⚠️ Order not found. Please check your Order ID.";
                    pnlOrderDetails.Visible = false;
                    return;
                }

                dr.Read();

                // ✅ ORDER DETAILS
                litOrderDetails.Text = $@"
            <ul>
                <li><strong>Order ID:</strong> {dr["OrderID"]}</li>
                <li><strong>Customer Name:</strong> {dr["CustomerName"]}</li>
                <li><strong>Order Date:</strong> {dr["OrderDate"]}</li>
                <li><strong>Amount:</strong> ₹{dr["FinalAmount"]}</li>
                <li><strong>Payment Status:</strong> {dr["PaymentStatus"]}</li>
                <li><strong>Current Status:</strong> {dr["OrderStatus"]}</li>
            </ul>";

                string status = dr["OrderStatus"].ToString();

                // 🔴 IMPORTANT: CLOSE FIRST READER
                dr.Close();

                // =====================================================
                // 🔥 FETCH MENU DETAILS (NEW CODE)
                // =====================================================

                string menuQuery = @"
        SELECT M.MenuName, M.Description, M.Category, M.ImagePath,
               OD.Quantity, OD.Price
        FROM OrderDetails OD
        INNER JOIN Menu M ON OD.MenuID = M.MenuID
        WHERE OD.OrderID = @oid";

                SqlCommand cmdMenu = new SqlCommand(menuQuery, con);
                cmdMenu.Parameters.AddWithValue("@oid", orderID);

                SqlDataReader drMenu = cmdMenu.ExecuteReader();

                string html = "<h4>🍽️ Ordered Items</h4><div class='row'>";

                while (drMenu.Read())
                {
                    html += $@"
            <div class='col-md-4'>
                <div style='border:1px solid #ddd; padding:10px; margin:10px; border-radius:10px;'>

                    <img src='{ResolveUrl("~/" + drMenu["ImagePath"])}'
                         style='width:100%; height:150px; object-fit:cover; border-radius:8px;' />

                    <h5>{drMenu["MenuName"]}</h5>

                    <p><b>Category:</b> {drMenu["Category"]}</p>
                    <p><b>Description:</b> {drMenu["Description"]}</p>

                    <p><b>Qty:</b> {drMenu["Quantity"]}</p>
                    <p><b>Price:</b> ₹{drMenu["Price"]}</p>

                </div>
            </div>";
                }

                html += "</div>";

                litMenuDetails.Text = html;

                drMenu.Close();

                // =====================================================

                pnlOrderDetails.Visible = true;
                trackingDiv.Visible = true;

                // TRACKING UI
                stepReceived.Attributes["class"] = "tracking-step received";
                stepProcess.Attributes["class"] = "tracking-step process";
                stepReady.Attributes["class"] = "tracking-step ready";
                stepReached.Attributes["class"] = "tracking-step reached";

                switch (status)
                {
                    case "Confirmed":
                    case "Received":
                        stepReceived.Attributes["class"] += " current-step";
                        break;

                    case "Process Started":
                        stepReceived.Attributes["class"] += " current-step";
                        stepProcess.Attributes["class"] += " current-step";
                        break;

                    case "Ready to Send":
                        stepReceived.Attributes["class"] += " current-step";
                        stepProcess.Attributes["class"] += " current-step";
                        stepReady.Attributes["class"] += " current-step";
                        break;

                    case "Reached":
                        stepReceived.Attributes["class"] += " current-step";
                        stepProcess.Attributes["class"] += " current-step";
                        stepReady.Attributes["class"] += " current-step";
                        stepReached.Attributes["class"] += " current-step";
                        break;
                }
            }
        }
      
    }
}