using System;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class EditMenu : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                int id = Convert.ToInt32(Request.QueryString["id"]);
                LoadMenu(id);
            }
        }
        void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT CategoryName FROM Categories WHERE IsActive=1";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryName";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Category --", ""));
            }
        }

        void LoadMenu(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "SELECT * FROM Menu WHERE MenuID=@id";

                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hfMenuID.Value = dr["MenuID"].ToString();
                    txtMenuName.Text = dr["MenuName"].ToString();
                    txtCost.Text = dr["Cost"].ToString();

                    string cat = dr["Category"].ToString();
                    if (ddlCategory.Items.FindByValue(cat) != null)
                    {
                        ddlCategory.SelectedValue = cat;
                    }

                    txtdetails.Text = dr["Description"].ToString();
                    imgPreview.Src = ResolveUrl("~/" + dr["ImagePath"].ToString());

                    // ✅ SET AVAILABILITY
                    string isAvailable = dr["IsAvailable"].ToString();
                    if (rblIsAvailable.Items.FindByValue(isAvailable) != null)
                    {
                        rblIsAvailable.SelectedValue = isAvailable;
                    }
                }
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hfMenuID.Value);

            string imagePath = "";

            if (fuImage.HasFile)
            {
                string fileName = id + "_" + fuImage.FileName;

                string folder = Server.MapPath("~/Menusimg/");

                fuImage.SaveAs(folder + fileName);

                imagePath = "Menusimg/" + fileName;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = "UPDATE Menu SET MenuName=@name,Cost=@cost,Category=@cat,Description=@Description,IsAvailable=@avail" +
                           (imagePath != "" ? ",ImagePath=@img" : "") +
                           " WHERE MenuID=@id";
                SqlCommand cmd = new SqlCommand(q, con);

                cmd.Parameters.AddWithValue("@name", txtMenuName.Text);
                cmd.Parameters.AddWithValue("@cost", txtCost.Text);
                cmd.Parameters.AddWithValue("@Description", txtdetails.Text);
                cmd.Parameters.AddWithValue("@cat", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@avail", rblIsAvailable.SelectedValue);
                if (ddlCategory.SelectedValue == "")
                {
                    return;
                }
                if (imagePath != "")
                    cmd.Parameters.AddWithValue("@img", imagePath);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            Response.Redirect("ViewMenu.aspx");
        }
    }
}