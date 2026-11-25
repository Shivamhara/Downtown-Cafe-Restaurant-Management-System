using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.User
{
    public partial class StudentSpecial : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMenu("Student Special"); // Category name
        }

        private void LoadMenu(string categoryName)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT MenuID, MenuName, Cost AS Price, Description, ImagePath AS Image " +
                    "FROM Menu WHERE Category=@Category AND IsAvailable=1", con);
                cmd.Parameters.AddWithValue("@Category", categoryName);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    DataTable dtMenu = new DataTable();
                    dtMenu.Load(dr);

                    repeaterMenu.DataSource = dtMenu;
                    repeaterMenu.DataBind();

                    // Bind feedback for each menu item
                    foreach (RepeaterItem item in repeaterMenu.Items)
                    {
                        int menuId = Convert.ToInt32(dtMenu.Rows[item.ItemIndex]["MenuID"]);
                        Repeater rptFeedback = (Repeater)item.FindControl("rptFeedback");
                        Label lblNoFeedback = (Label)item.FindControl("lblNoFeedback");
                        BindFeedback(rptFeedback, menuId, lblNoFeedback);
                    }

                    lblNoItems.Text = "";
                }
                else
                {
                    lblNoItems.Text = "No items found in this category.";
                }

                dr.Close();
            }
        }

        private void BindFeedback(Repeater rpt, int menuId, Label lblNoFeedback)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT R.Comment, R.Rating, U.FullName AS UserName " +
                               "FROM Ratings R INNER JOIN Customers U ON R.UserID=U.CustomerID " +
                               "WHERE R.MenuID=@MenuID AND R.UserID <> @CurrentUserID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MenuID", menuId);
                cmd.Parameters.AddWithValue("@CurrentUserID", Session["CustomerID"].ToString());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);

                if (dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    lblNoFeedback.Visible = false;
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                    lblNoFeedback.Visible = true;
                }

                dr.Close();
            }
        }
        protected void rptMenu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "addcart")
            {
                int menuID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT MenuID, MenuName, Cost, Category, Description, ImagePath FROM Menu WHERE IsAvailable = 1";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", menuID);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        DataTable cart;

                        if (Session["cart"] == null)
                        {
                            cart = new DataTable();

                            cart.Columns.Add("MenuID", typeof(int));
                            cart.Columns.Add("MenuName");
                            cart.Columns.Add("Cost", typeof(decimal));
                            cart.Columns.Add("Qty", typeof(int));
                        }
                        else
                        {
                            cart = (DataTable)Session["cart"];
                        }

                        bool exists = false;

                        foreach (DataRow row in cart.Rows)
                        {
                            if (Convert.ToInt32(row["MenuID"]) == menuID)
                            {
                                row["Qty"] = Convert.ToInt32(row["Qty"]) + 1;
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            DataRow row = cart.NewRow();

                            row["MenuID"] = dr["MenuID"];
                            row["MenuName"] = dr["MenuName"];
                            row["Cost"] = dr["Cost"];
                            row["Qty"] = 1;

                            cart.Rows.Add(row);
                        }

                        Session["cart"] = cart;
                    }

                    con.Close();
                }

                Response.Redirect("ViewCart.aspx");
            }
        }

      
    }
}