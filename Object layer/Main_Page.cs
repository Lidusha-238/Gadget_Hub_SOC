using System;
using System.Data;
using Data_Access;
using System.Data.SqlClient;

namespace Business_layer
{
    public class Main_Page
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal MarketPrice { get; set; }
        public string ImageUrl { get; set; }

        private Database db = new Database();

        // ✅ Get products (with market price calculated)
        public DataSet GetProducts(string search = "")
        {
            string sql = @"
                SELECT ProductID, ProductName, 
                       MIN(PricePerUnit) * 2 AS MarketPrice,
                       '/images/' + REPLACE(ProductName, ' ', '') + '.jpg' AS ImageUrl
                FROM Inventory
                WHERE (@search = '' OR ProductName LIKE '%' + @search + '%')
                GROUP BY ProductID, ProductName";

            return db.exeSelectQuery(sql, new SqlParameter("@search", search));
        }

        // ✅ Add product to cart with dynamic price and name
        public bool AddToCart(int userID, int productID, int quantity = 1)
        {
            // 1️⃣ Get product details from Inventory
            string sqlProduct = @"
                SELECT ProductName, MIN(PricePerUnit) * 2 AS MarketPrice
                FROM Inventory
                WHERE ProductID = @ProductID
                GROUP BY ProductName";

            DataSet ds = db.exeSelectQuery(sqlProduct, new SqlParameter("@ProductID", productID));

            if (ds.Tables[0].Rows.Count == 0)
                throw new Exception("Product not found.");

            DataRow row = ds.Tables[0].Rows[0];
            string productName = row["ProductName"].ToString();
            decimal marketPrice = Convert.ToDecimal(row["MarketPrice"]);

            // 2️⃣ Insert into Cart table
            string sqlCart = @"
                INSERT INTO Cart (ID, ProductID, ProductName, MarketPrice, Quantity)
                VALUES (@ID, @ProductID, @ProductName, @MarketPrice, @Quantity)";

            SqlParameter[] parameters = {
                new SqlParameter("@ID", userID),
                new SqlParameter("@ProductID", productID),
                new SqlParameter("@ProductName", productName),
                new SqlParameter("@MarketPrice", marketPrice),
                new SqlParameter("@Quantity", quantity)
            };

            return db.exeQuery(sqlCart, parameters) > 0;
        }
    }
}
