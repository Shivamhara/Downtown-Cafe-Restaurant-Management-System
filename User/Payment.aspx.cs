using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace CafeandfoodRestaurant.User
{
    public partial class Payment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["OrderID"] != null && Session["FinalAmount"] != null)
                {
                    lblOrderID.Text = Session["OrderID"].ToString();
                    lblAmount.Text = Session["FinalAmount"].ToString();
                }
                else
                {
                    Response.Redirect("ViewCart.aspx");
                }
            }
        }
        void SendEmail(string toEmail, string orderID, string transactionID)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("mycollegeproject02@gmail.com", "Cafe&Food");
                mail.To.Add(toEmail);

                mail.Subject = "Order Confirmation - Cafe&Food";

                mail.Body =
                "Dear Customer,\n\n" +
                "Your order has been placed successfully.\n\n" +

                "Order ID: " + orderID + "\n" +
                "Transaction ID: " + transactionID + "\n\n" +

                "Your order is now confirmed and will be prepared shortly.\n\n" +

                "Thank you for ordering with Cafe&Food.\n\n" +
                "Regards,\nCafe&Food Team";

                SmtpClient smtp = new SmtpClient(); // Uses Web.config SMTP
                smtp.Send(mail);
            }
            catch (Exception)
            {
                // optional logging
            }
        }

        // RANDOM TRANSACTION CODE
        string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = chars[rnd.Next(chars.Length)];
            }

            return new string(code);
        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            string paymentMode = "";

            //if (rdoCash.Checked)
            //    paymentMode = "Cash";

             if (rdoUPI.Checked)
                paymentMode = "UPI";

            else if (rdoCard.Checked)
                paymentMode = "Card";

            else
            {
                lblMessage.Text = "Please select payment mode";
                return;
            }

            int orderID = Convert.ToInt32(lblOrderID.Text);
            int customerID = Convert.ToInt32(Session["CustomerID"]);
            decimal amount = Convert.ToDecimal(lblAmount.Text);

            int menuID = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // GET MENU ITEM ID
                string getMenu = "SELECT TOP 1 MenuID FROM OrderDetails WHERE OrderID=@oid";

                SqlCommand cmdMenu = new SqlCommand(getMenu, con);
                cmdMenu.Parameters.AddWithValue("@oid", orderID);

                object result = cmdMenu.ExecuteScalar();

                if (result != null)
                    menuID = Convert.ToInt32(result);

                // GENERATE TRANSACTION ID
                string randomCode = GenerateRandomCode(5);

                string transactionID =
                    "TXN-U" + customerID +
                    "-O" + orderID +
                    "-M" + menuID +
                    "-" + randomCode;

                // INSERT PAYMENT
                string query = @"INSERT INTO Payments
                (OrderID,CustomerID,PaymentMode,TransactionID,Amount,PaymentDate,PaymentStatus)

                VALUES
                (@orderid,@cid,@mode,@tid,@amount,GETDATE(),'Success')";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@orderid", orderID);
                cmd.Parameters.AddWithValue("@cid", customerID);
                cmd.Parameters.AddWithValue("@mode", paymentMode);
                cmd.Parameters.AddWithValue("@tid", transactionID);
                cmd.Parameters.AddWithValue("@amount", amount);

                cmd.ExecuteNonQuery();

                // UPDATE ORDER STATUS
                string updateOrder = @"UPDATE Orders 
                                       SET PaymentStatus='Paid', OrderStatus='Confirmed'
                                       WHERE OrderID=@oid";

                SqlCommand cmdUpdate = new SqlCommand(updateOrder, con);
                cmdUpdate.Parameters.AddWithValue("@oid", orderID);

                cmdUpdate.ExecuteNonQuery();
                string customerEmail = "";

                string getEmail = "SELECT Email FROM Customers WHERE CustomerID=@cid";

                SqlCommand cmdEmail = new SqlCommand(getEmail, con);
                cmdEmail.Parameters.AddWithValue("@cid", customerID);

                object emailResult = cmdEmail.ExecuteScalar();

                if (emailResult != null)
                {
                    customerEmail = emailResult.ToString();

                    // SEND EMAIL
                    SendEmail(customerEmail, orderID.ToString(), transactionID);
                }

                con.Close();


                lblMessage.Text = "✅ Payment Successful! Transaction ID: " + transactionID;

                // CLEAR SESSION
                Session.Remove("OrderID");
                Session.Remove("FinalAmount");
            }
        }
    }
}