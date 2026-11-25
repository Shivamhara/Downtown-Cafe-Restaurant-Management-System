using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.User
{
    public partial class ViewMenu : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadMenu("All");
            }
        }

        // ✅ LOAD CATEGORY DROPDOWN
        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT DISTINCT Category FROM Menu WHERE IsAvailable=1";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, "All");
            }
        }

        // ✅ LOAD MENU BASED ON CATEGORY
        private void LoadMenu(string category)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query;

                if (category == "All")
                {
                    query = "SELECT MenuID, MenuName, Cost, Category, Description, ImagePath FROM Menu WHERE IsAvailable=1";
                }
                else
                {
                    query = "SELECT MenuID, MenuName, Cost, Category, Description, ImagePath FROM Menu WHERE Category=@Category AND IsAvailable=1";
                }

                SqlCommand cmd = new SqlCommand(query, con);

                if (category != "All")
                {
                    cmd.Parameters.AddWithValue("@Category", category);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtMenu = new DataTable();
                da.Fill(dtMenu);

                rptMenu.DataSource = dtMenu;
                rptMenu.DataBind();

                // ✅ Bind feedback
                foreach (RepeaterItem item in rptMenu.Items)
                {
                    int menuId = Convert.ToInt32(dtMenu.Rows[item.ItemIndex]["MenuID"]);

                    Repeater rptFeedback = (Repeater)item.FindControl("rptFeedback");
                    Label lblNoFeedback = (Label)item.FindControl("lblNoFeedback");

                    BindFeedback(rptFeedback, menuId, lblNoFeedback);
                }
            }
        }

        // ✅ DROPDOWN CHANGE EVENT
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = ddlCategory.SelectedValue;
            LoadMenu(selectedCategory);
        }

        // ✅ FEEDBACK BIND
        private void BindFeedback(Repeater rpt, int menuId, Label lblNoFeedback)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TOP 1 R.Comment, R.Rating, U.FullName AS UserName
                         FROM Ratings R
                         INNER JOIN Customers U ON R.UserID = U.CustomerID
                         WHERE R.MenuID = @MenuID
                         ORDER BY R.RatingID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = menuId;

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
            }
        }
        // ✅ ADD TO CART (FIXED QUERY ALSO)
        protected void rptMenu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "addcart")
            {
                int menuID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT MenuID, MenuName, Cost FROM Menu WHERE MenuID=@id";
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
                }

                Response.Redirect("ViewCart.aspx");
            }
        }
    }
}