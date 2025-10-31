using Data_Access;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Business_layer
{
    public class Cart
    {
        private Database db = new Database();

        // Get all cart items
        public DataSet GetCartItems(int userId)
        {
            string sql = @"
                SELECT 
                    c.CartID,
                    c.ProductID,
                    c.ProductName,
                    c.Quantity,
                    c.MarketPrice AS PricePerUnit,
                    (c.Quantity * c.MarketPrice) AS Total
                FROM Cart c
                WHERE c.ID = @ID";

            return db.exeSelectQuery(sql, new SqlParameter("@ID", userId));
        }

        // Get total price of all items in the cart
        public decimal GetCartTotal(int userId)
        {
            string sql = @"
                SELECT SUM(c.Quantity * c.MarketPrice)
                FROM Cart c
                WHERE c.ID = @ID";

            DataSet ds = db.exeSelectQuery(sql, new SqlParameter("@ID", userId));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
                return Convert.ToDecimal(ds.Tables[0].Rows[0][0]);

            return 0;
        }

        // Add product to cart
        public bool AddToCart(int userId, int productId, string productName, int quantity, decimal pricePerUnit)
        {
            // Check if product already exists
            string checkSql = "SELECT COUNT(*) FROM Cart WHERE ID=@ID AND ProductID=@ProductID";
            SqlParameter[] checkParams = {
                new SqlParameter("@ID", userId),
                new SqlParameter("@ProductID", productId)
            };
            DataSet dsCheck = db.exeSelectQuery(checkSql, checkParams);

            if (dsCheck.Tables[0].Rows[0][0] != DBNull.Value && Convert.ToInt32(dsCheck.Tables[0].Rows[0][0]) > 0)
            {
                // Update quantity and price if already exists
                string updateSql = @"
                    UPDATE Cart 
                    SET Quantity = Quantity + @Quantity,
                        MarketPrice = @MarketPrice
                    WHERE ID=@ID AND ProductID=@ProductID";
                SqlParameter[] updateParams = {
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@MarketPrice", pricePerUnit),
                    new SqlParameter("@ID", userId),
                    new SqlParameter("@ProductID", productId)
                };
                return db.exeQuery(updateSql, updateParams) > 0;
            }
            else
            {
                // Insert new cart item
                string insertSql = @"
                    INSERT INTO Cart (ID, ProductID, ProductName, Quantity, MarketPrice)
                    VALUES (@ID, @ProductID, @ProductName, @Quantity, @MarketPrice)";
                SqlParameter[] insertParams = {
                    new SqlParameter("@ID", userId),
                    new SqlParameter("@ProductID", productId),
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@MarketPrice", pricePerUnit)
                };
                return db.exeQuery(insertSql, insertParams) > 0;
            }
        }

        // Remove item from cart
        public bool RemoveFromCart(int cartId)
        {
            string sql = "DELETE FROM Cart WHERE CartID = @CartID";
            SqlParameter[] parameters = { new SqlParameter("@CartID", cartId) };
            return db.exeQuery(sql, parameters) > 0;
        }

        // Update quantity
        public bool UpdateQuantity(int cartId, int quantity)
        {
            string sql = "UPDATE Cart SET Quantity = @Quantity WHERE CartID = @CartID";
            SqlParameter[] parameters = {
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@CartID", cartId)
            };
            return db.exeQuery(sql, parameters) > 0;
        }

        // Checkout
        public bool CheckoutAndSaveOrders(int userId)
        {
            DataSet ds = GetCartItems(userId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;

            decimal totalAmount = GetCartTotal(userId);

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-T0DGFL0;Initial Catalog=Gadget_Hub;Integrated Security=True;"))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // Insert into Orders
                    string orderSql = @"
                        INSERT INTO Orders (ID, TotalAmount, OrderDate)
                        VALUES (@ID, @TotalAmount, @OrderDate);
                        SELECT SCOPE_IDENTITY();";
                    SqlCommand orderCmd = new SqlCommand(orderSql, conn, tran);
                    orderCmd.Parameters.AddWithValue("@ID", userId);
                    orderCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                    // Insert into UserOrders
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string detailSql = @"
                            INSERT INTO OrderDetails (OrderID, ID, ProductID, ProductName, Quantity, PricePerUnit, Total)
                            VALUES (@OrderID, @ID, @ProductID, @ProductName, @Quantity, @PricePerUnit, @Total)";
                        SqlCommand detailCmd = new SqlCommand(detailSql, conn, tran);
                        detailCmd.Parameters.AddWithValue("@OrderID", orderId);
                        detailCmd.Parameters.AddWithValue("@ID", userId);
                        detailCmd.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                        detailCmd.Parameters.AddWithValue("@ProductName", row["ProductName"]);
                        detailCmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                        detailCmd.Parameters.AddWithValue("@PricePerUnit", row["PricePerUnit"]);
                        detailCmd.Parameters.AddWithValue("@Total", row["Total"]);
                        detailCmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    return true;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
