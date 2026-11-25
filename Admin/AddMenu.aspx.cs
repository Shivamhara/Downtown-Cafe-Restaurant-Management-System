using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CafeandfoodRestaurant.Admin
{
    public partial class AddMenu : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                //  BindMenu();
            }
        }
        void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive=1";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName"; // what user sees
                ddlCategory.DataValueField = "CategoryName"; // what gets saved
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
            }
        }

        // LOAD MENU
        //protected void BindMenu(string search = "", string category = "")
        //{
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {

        //        string q = "SELECT * FROM Menu WHERE 1=1";

        //        if (search != "")
        //            q += " AND MenuName LIKE '%'+@search+'%'";

        //        if (category != "")
        //            q += " AND Category=@cat";


        //        SqlCommand cmd = new SqlCommand(q, con);

        //        cmd.Parameters.AddWithValue("@search", search);
        //        cmd.Parameters.AddWithValue("@cat", category);


        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        DataTable dt = new DataTable();

        //        da.Fill(dt);


        //        rptMenu.DataSource = dt;
        //        rptMenu.DataBind();
        //    }
        //}



        // ADD MENU
        protected void btnAddMenu_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue == "")
            {
                lblMessage.Text = "⚠️ Please select category!";
                return;
            }
            string name = txtMenuName.Text.Trim();
            string cost = txtCost.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string details = txtdetails.Text.Trim();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Insert menu first
                string insertQuery = @"INSERT INTO Menu(MenuName,Cost,Category,Description)
                               VALUES(@name,@cost,@cat,@Description);
                               SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(insertQuery, con);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@cost", cost);
                cmd.Parameters.AddWithValue("@cat", category);
                cmd.Parameters.AddWithValue("@Description", details);
                int menuID = Convert.ToInt32(cmd.ExecuteScalar());

                string imagePath = "";

                // Save Image
                if (fuImage.HasFile)
                {
                    string fileName = menuID + "_" + System.IO.Path.GetFileName(fuImage.FileName);

                    string folder = Server.MapPath("~/Menusimg/");

                    fuImage.SaveAs(folder + fileName);

                    imagePath = "Menusimg/" + fileName;

                    // Update DB
                    string updateQuery = "UPDATE Menu SET ImagePath=@img WHERE MenuID=@id";

                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, con);

                    cmdUpdate.Parameters.AddWithValue("@img", imagePath);
                    cmdUpdate.Parameters.AddWithValue("@id", menuID);

                    cmdUpdate.ExecuteNonQuery();
                }

                con.Close();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup",
            "alert('Menu Added Successfully'); window.location='ViewMenu.aspx';", true);
            //BindMenu();
        }
        // DELETE MENU
        //protected void DeleteMenu(object sender, CommandEventArgs e)
        //{

        //    int id = Convert.ToInt32(e.CommandArgument);


        //    using (SqlConnection con = new SqlConnection(cs))
        //    {

        //        string q = "DELETE FROM Menu WHERE MenuID=@id";

        //        SqlCommand cmd = new SqlCommand(q, con);

        //        cmd.Parameters.AddWithValue("@id", id);

        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }

        //    lblMessage.Text = "Menu deleted successfully";

        //    BindMenu();
        //}



        // EDIT MENU
        protected void EditMenu(object sender, CommandEventArgs e)
        {

            int id = Convert.ToInt32(e.CommandArgument);

            Response.Redirect("EditMenu.aspx?MenuID=" + id);
        }



        // SEARCH
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    BindMenu(txtSearch.Text.Trim(), ddlFilterCategory.SelectedValue);
        //}


    }
}