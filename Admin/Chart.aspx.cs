using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CafeandfoodRestaurant.Admin
{
    public partial class Chart : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["CafeSfood"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRevenue();
                LoadStatus();
                LoadProducts();
                LoadCustomers();
            }
        }

        void LoadRevenue()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"
SELECT 
    DATENAME(MONTH, DateVal) AS Month,
    MONTH(DateVal) AS MonthNo,
    SUM(Amount) AS Revenue
FROM
(
    -- Orders Revenue
    SELECT 
        CAST(OrderDate AS DATETIME) AS DateVal,
        FinalAmount AS Amount
    FROM Orders
    WHERE PaymentStatus = 'Paid'

    UNION ALL

    -- Table Booking Revenue
    SELECT 
        BookingDate AS DateVal,
        TotalAmount AS Amount
    FROM TableBookings
    WHERE PaymentStatus = 'Paid'

) AS CombinedData

GROUP BY DATENAME(MONTH, DateVal), MONTH(DateVal)
ORDER BY MonthNo
";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string m = "", r = "";

                foreach (DataRow row in dt.Rows)
                {
                    m += row["Month"] + ",";
                    r += row["Revenue"] + ",";
                }

                hfMonths.Value = m.TrimEnd(',');
                hfRevenue.Value = r.TrimEnd(',');
            }
        }

        void LoadStatus()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT OrderStatus, COUNT(*) Total FROM Orders GROUP BY OrderStatus";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string s = "", c = "";

                foreach (DataRow row in dt.Rows)
                {
                    s += row["OrderStatus"] + ",";
                    c += row["Total"] + ",";
                }

                hfStatus.Value = s.TrimEnd(',');
                hfStatusCount.Value = c.TrimEnd(',');
            }
        }

        void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"SELECT TOP 5 m.MenuName, SUM(od.Quantity) TotalSold
                             FROM OrderDetails od
                             INNER JOIN Menu m ON od.MenuID = m.MenuID
                             GROUP BY m.MenuName
                             ORDER BY TotalSold DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string n = "", s = "";

                foreach (DataRow row in dt.Rows)
                {
                    n += row["MenuName"] + ",";
                    s += row["TotalSold"] + ",";
                }

                hfProductNames.Value = n.TrimEnd(',');
                hfProductSales.Value = s.TrimEnd(',');
            }
        }

        void LoadCustomers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string q = @"
SELECT TOP 5 
    c.FullName,
    SUM(Amount) AS TotalSpent
FROM
(
    -- Orders
    SELECT 
        CustomerID,
        FinalAmount AS Amount
    FROM Orders
    WHERE PaymentStatus = 'Paid'

    UNION ALL

    -- Table Bookings
    SELECT 
        CustomerID,
        TotalAmount AS Amount
    FROM TableBookings
    WHERE PaymentStatus = 'Paid'

) AS Combined

INNER JOIN Customers c ON Combined.CustomerID = c.CustomerID

GROUP BY c.FullName
ORDER BY TotalSpent DESC
";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string n = "", s = "";

                foreach (DataRow row in dt.Rows)
                {
                    n += row["FullName"] + ",";
                    s += row["TotalSpent"] + ",";
                }

                hfCustomerNames.Value = n.TrimEnd(',');
                hfCustomerSpending.Value = s.TrimEnd(',');
            }
        }
    }
}