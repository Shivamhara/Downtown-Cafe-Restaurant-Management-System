using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CafeandfoodRestaurant.Admin
{
    public partial class TableInfo : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTables();
            }
        }

        void LoadTables()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT TableID, TableType, TotalTables, BookedTables, AmountPerTable
                                 FROM TableMaster";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptTables.DataSource = dt;
                rptTables.DataBind();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            int tableID = Convert.ToInt32(btn.CommandArgument);
            int totalTables = Convert.ToInt32(((TextBox)item.FindControl("txtTotal")).Text);
            int bookedTables = Convert.ToInt32(((TextBox)item.FindControl("txtBooked")).Text);
            decimal amountPerTable = Convert.ToDecimal(((TextBox)item.FindControl("txtAmount")).Text);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE TableMaster 
                                 SET TotalTables=@total, BookedTables=@booked, AmountPerTable=@amount
                                 WHERE TableID=@tid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@total", totalTables);
                cmd.Parameters.AddWithValue("@booked", bookedTables);
                cmd.Parameters.AddWithValue("@amount", amountPerTable);
                cmd.Parameters.AddWithValue("@tid", tableID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = $"✅ Table #{tableID} updated successfully!";
            LoadTables();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            string type = txtNewType.Text.Trim();
            int total = Convert.ToInt32(txtNewTotal.Text);
            int booked = Convert.ToInt32(txtNewBooked.Text);
            decimal amount = Convert.ToDecimal(txtNewAmount.Text);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO TableMaster (TableType, TotalTables, BookedTables, AmountPerTable, CreatedDate)
                                 VALUES (@type, @total, @booked, @amount, @created)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@booked", booked);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@created", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = $"✅ Table type '{type}' added successfully!";
            txtNewType.Text = txtNewTotal.Text = txtNewBooked.Text = txtNewAmount.Text = "";
            LoadTables();
        }
    }
}