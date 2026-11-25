using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewCart : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        // INCREASE / DECREASE / REMOVE ITEM
        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable cart = (DataTable)Session["cart"];

            if (cart == null) return;

            int menuID = Convert.ToInt32(e.CommandArgument);

            foreach (DataRow row in cart.Rows)
            {
                if (Convert.ToInt32(row["MenuID"]) == menuID)
                {
                    int qty = Convert.ToInt32(row["Qty"]);

                    if (e.CommandName == "IncreaseQty")
                    {
                        row["Qty"] = qty + 1;
                    }

                    else if (e.CommandName == "DecreaseQty")
                    {
                        if (qty > 1)
                            row["Qty"] = qty - 1;
                    }

                    else if (e.CommandName == "RemoveItem")
                    {
                        cart.Rows.Remove(row);
                    }

                    break;
                }
            }

            Session["cart"] = cart;

            BindCart();
        }

        // BIND CART
        void BindCart()
        {
            DataTable cart = (DataTable)Session["cart"];

            if (cart != null)
            {
                gvCart.DataSource = cart;
                gvCart.DataBind();

                decimal total = 0;

                foreach (DataRow row in cart.Rows)
                {
                    total += Convert.ToDecimal(row["Cost"]) * Convert.ToInt32(row["Qty"]);
                }

                lblTotal.Text = total.ToString();
            }
        }
        decimal GetMembershipDiscount(int customerID, out int totalRemainingOrders)
        {
            decimal totalDiscount = 0;
            totalRemainingOrders = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT DiscountPercent, MaxOrders, OrdersUsed, MembershipID
                         FROM Membership
                         WHERE CustomerID=@cid 
                         AND IsActive=1 
                         AND EndDate >= GETDATE()";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cid", customerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int maxOrders = Convert.ToInt32(dr["MaxOrders"]);
                    int usedOrders = Convert.ToInt32(dr["OrdersUsed"]);
                    int remaining = maxOrders - usedOrders;

                    if (remaining > 0)
                    {
                        totalRemainingOrders += remaining;

                        // 🔥 ADD ALL DISCOUNTS
                        totalDiscount += Convert.ToDecimal(dr["DiscountPercent"]);
                    }
                }

                con.Close();
            }

            // Optional: Limit max discount (recommended)
            if (totalDiscount > 50)  // cap at 50%
                totalDiscount = 50;

            return totalDiscount;
        }
        protected void btnPayment_Click(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            DataTable cart = (DataTable)Session["cart"];
            if (cart == null || cart.Rows.Count == 0)
            {
                Response.Write("<script>alert('Cart is empty');</script>");
                return;
            }

            int customerID = Convert.ToInt32(Session["CustomerID"]);
            decimal subTotal = 0;

            foreach (DataRow row in cart.Rows)
                subTotal += Convert.ToDecimal(row["Cost"]) * Convert.ToInt32(row["Qty"]);

            // Get membership discount
            int remainingOrders;
            decimal discountPercent = GetMembershipDiscount(customerID, out remainingOrders);

            // If no membership discount, apply loyalty discount for repeat customers
            if (discountPercent == 0)
            {
                int previousOrders = 0;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"SELECT COUNT(*) FROM Orders WHERE CustomerID=@cid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@cid", customerID);
                    con.Open();
                    previousOrders = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }

                if (previousOrders > 0) // repeat customer
                {
                    // Loyalty discount 5% to 15% based on number of previous orders
                    discountPercent = Math.Min(15, 5 + previousOrders); // max 15%
                }
            }

            decimal discountAmount = (subTotal * discountPercent) / 100;
            decimal finalAmount = subTotal - discountAmount;

            // Update OrdersUsed in Membership table if applicable
            if (discountPercent > 0 && remainingOrders > 0)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string updateQuery = @"UPDATE Membership 
                                   SET OrdersUsed = OrdersUsed + 1 
                                   WHERE CustomerID=@cid AND IsActive=1 AND EndDate >= GETDATE()";
                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);
                    cmdUpdate.Parameters.AddWithValue("@cid", customerID);
                    con.Open();
                    cmdUpdate.ExecuteNonQuery();
                    con.Close();
                }
            }

            // Insert Order and OrderDetails
            int orderID = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string orderQuery = @"INSERT INTO Orders
        (CustomerID,OrderType,OrderDate,SubTotal,DiscountAmount,FinalAmount,
        DeliveryAddress,DistanceKM,OrderStatus,PaymentStatus)
        VALUES
        (@cid,'Delivery',GETDATE(),@sub,@discount,@final,'User Address',5,'Pending','Pending');
        SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(orderQuery, con);
                cmd.Parameters.AddWithValue("@cid", customerID);
                cmd.Parameters.AddWithValue("@sub", subTotal);
                cmd.Parameters.AddWithValue("@discount", discountAmount);
                cmd.Parameters.AddWithValue("@final", finalAmount);

                orderID = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (DataRow row in cart.Rows)
                {
                    int menuID = Convert.ToInt32(row["MenuID"]);
                    string detailQuery = @"INSERT INTO OrderDetails
            (OrderID,MenuID,Quantity,Price,TotalPrice)
            VALUES
            (@oid,@mid,@qty,@price,@total)";

                    SqlCommand cmd2 = new SqlCommand(detailQuery, con);
                    cmd2.Parameters.AddWithValue("@oid", orderID);
                    cmd2.Parameters.AddWithValue("@mid", menuID);
                    cmd2.Parameters.AddWithValue("@qty", row["Qty"]);
                    cmd2.Parameters.AddWithValue("@price", row["Cost"]);
                    cmd2.Parameters.AddWithValue("@total", Convert.ToDecimal(row["Cost"]) * Convert.ToInt32(row["Qty"]));
                    cmd2.ExecuteNonQuery();
                }

                con.Close();
            }

            // Send data to Payment Page
            Session["OrderID"] = orderID;
            Session["FinalAmount"] = finalAmount;
            Session["cart"] = null;

            Response.Redirect("Payment.aspx");
        }
    }
}