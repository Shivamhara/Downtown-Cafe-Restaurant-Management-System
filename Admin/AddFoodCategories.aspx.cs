using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class AddFoodCategories : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
            }
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditCat")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string q = "SELECT * FROM Categories WHERE CategoryID=@id";
                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hfCategoryID.Value = dr["CategoryID"].ToString();
                        txtCategoryName.Text = dr["CategoryName"].ToString();
                        txtDescription.Text = dr["Description"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        string imgPath = dr["ImagePath"]?.ToString();
                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            imgCurrent.ImageUrl = "~/CategoryImg/" + imgPath;
                            imgCurrent.Visible = true;
                        }
                        else
                        {
                            imgCurrent.ImageUrl = "";
                            imgCurrent.Visible = false;
                        }
                        btnAdd.Text = "Update Category";
                    }
                }
            }
            else if (e.CommandName == "clear")
            {
                Response.Redirect("~/Admin/AddFoodCategories.aspx");
            }
            else if (e.CommandName == "DeleteCat")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string check = "SELECT COUNT(*) FROM Menu WHERE Category=@cat";
                    SqlCommand cmdCheck = new SqlCommand(check, con);

                    string catName = new SqlCommand("SELECT CategoryName FROM Categories WHERE CategoryID=@id", con)
                    {
                        Parameters = { new SqlParameter("@id", id) }
                    }.ExecuteScalar().ToString();

                    cmdCheck.Parameters.AddWithValue("@cat", catName);

                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "alert",
                            "alert('⚠️ Cannot delete! Items exist in this category.');",
                            true);
                        return;
                    }

                    // DELETE IMAGE ALSO
                    string oldImage = new SqlCommand("SELECT ImagePath FROM Categories WHERE CategoryID=@id", con)
                    {
                        Parameters = { new SqlParameter("@id", id) }
                    }.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(oldImage))
                    {
                        string oldPath = Server.MapPath("~/" + oldImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE CategoryID=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    lblMsg.Text = "✅ Category deleted successfully!";
                    BindCategories();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text.Trim() == "")
            {
                lblMsg.Text = "⚠️ Category name required!";
                return;
            }

            string imagePath = "";
            string folderPath = Server.MapPath("~/CategoryImg/");

            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // DUPLICATE CHECK
                string checkQuery = "SELECT COUNT(*) FROM Categories WHERE CategoryName=@name AND CategoryID!=@id";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@name", txtCategoryName.Text.Trim());
                checkCmd.Parameters.AddWithValue("@id", hfCategoryID.Value == "" ? 0 : Convert.ToInt32(hfCategoryID.Value));

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMsg.Text = "⚠️ Category already exists!";
                    return;
                }

                // ➕ INSERT
                if (hfCategoryID.Value == "")
                {
                    string query = @"INSERT INTO Categories 
                    (CategoryName, Description, IsActive)
                    VALUES (@name, @desc, @active);
                    SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", txtCategoryName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@active", chkActive.Checked);

                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    // SAVE IMAGE
                    if (fuCategoryImage.HasFile)
                    {
                        string cleanName = txtCategoryName.Text.Trim().Replace(" ", "");
                        string ext = System.IO.Path.GetExtension(fuCategoryImage.FileName);

                        string fileName = newId + "_" + cleanName + ext;

                        string fullPath = folderPath + fileName;
                        fuCategoryImage.SaveAs(fullPath);

                        imagePath = "CategoryImg/" + fileName;

                        SqlCommand updateImg = new SqlCommand("UPDATE Categories SET ImagePath=@img WHERE CategoryID=@id", con);
                        updateImg.Parameters.AddWithValue("@img", imagePath);
                        updateImg.Parameters.AddWithValue("@id", newId);
                        updateImg.ExecuteNonQuery();
                    }

                    lblMsg.Text = "✅ Category added successfully!";
                }
                else // ✏️ UPDATE
                {
                    int id = Convert.ToInt32(hfCategoryID.Value);

                    // GET OLD IMAGE
                    string oldImage = new SqlCommand("SELECT ImagePath FROM Categories WHERE CategoryID=@id", con)
                    {
                        Parameters = { new SqlParameter("@id", id) }
                    }.ExecuteScalar()?.ToString();

                    if (fuCategoryImage.HasFile)
                    {
                        // DELETE OLD IMAGE
                        if (!string.IsNullOrEmpty(oldImage))
                        {
                            string oldPath = Server.MapPath("~/" + oldImage);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        // SAVE NEW IMAGE
                        string cleanName = txtCategoryName.Text.Trim().Replace(" ", "");
                        string ext = System.IO.Path.GetExtension(fuCategoryImage.FileName);

                        string fileName = id + "_" + cleanName + ext;

                        string fullPath = folderPath + fileName;
                        fuCategoryImage.SaveAs(fullPath);

                        imagePath = "CategoryImg/" + fileName;
                    }

                    string query = @"UPDATE Categories 
                    SET CategoryName=@name, Description=@desc, IsActive=@active,
                        ImagePath = CASE WHEN @img='' THEN ImagePath ELSE @img END
                    WHERE CategoryID=@id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", txtCategoryName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@active", chkActive.Checked);
                    cmd.Parameters.AddWithValue("@img", imagePath);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();

                    lblMsg.Text = "✅ Category updated successfully!";
                }

                ClearForm();
                BindCategories();
            }
        }

        void BindCategories()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Categories ORDER BY CategoryID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

        void ClearForm()
        {
            txtCategoryName.Text = "";
            txtDescription.Text = "";
            chkActive.Checked = true;
            hfCategoryID.Value = "";
            btnAdd.Text = "Add Category";
        }
    }
}