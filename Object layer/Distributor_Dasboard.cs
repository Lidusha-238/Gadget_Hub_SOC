using Data_Access;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Business_layer
{
    public class Distributor_Dashboard
    {
        private readonly Database db = new Database();


        public DataSet GetInventory(int distributorId)
        {
            string sql = @"
                SELECT ProductID, ProductName, Quantity, PricePerUnit, DeliveryDate 
                FROM Inventory 
                WHERE DistributorID = @distributorId";

            return db.exeSelectQuery(sql, new SqlParameter("@distributorId", distributorId));
        }

        public int AddProduct(int distributorId, int productId, string productName, int quantity, decimal price, DateTime deliveryDate)
        {
            string sql = @"
                INSERT INTO Inventory (ProductID, DistributorID, ProductName, Quantity, PricePerUnit, DeliveryDate) 
                VALUES (@productId, @distributorId, @productName, @quantity, @price, @deliveryDate)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@productId", productId),
                new SqlParameter("@distributorId", distributorId),
                new SqlParameter("@productName", productName),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@price", price),
                new SqlParameter("@deliveryDate", deliveryDate)
            };

            return db.exeQuery(sql, parameters);
        }

        public int UpdateProduct(int distributorId, int productId, string productName, int quantity, decimal price, DateTime deliveryDate)
        {
            string sql = @"
                UPDATE Inventory 
                SET ProductName=@productName, Quantity=@quantity, PricePerUnit=@price, DeliveryDate=@deliveryDate 
                WHERE ProductID=@productId AND DistributorID=@distributorId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@productName", productName),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@price", price),
                new SqlParameter("@deliveryDate", deliveryDate),
                new SqlParameter("@productId", productId),
                new SqlParameter("@distributorId", distributorId)
            };

            return db.exeQuery(sql, parameters);
        }

        public int DeleteProduct(int distributorId, int productId)
        {
            string sql = "DELETE FROM Inventory WHERE ProductID=@productId AND DistributorID=@distributorId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@productId", productId),
                new SqlParameter("@distributorId", distributorId)
            };

            return db.exeQuery(sql, parameters);
        }


        public DataSet GetQuotationRequests()
        {
            string sql = @"
                SELECT RequestID, ProductID, ProductName, Quantity, RequestDate 
                FROM SendQuotations 
                ORDER BY RequestDate DESC";

            return db.exeSelectQuery(sql);
        }

        public int SendQuotationResponse(int requestId, int distributorId, int productId, string productName, int quantity, decimal price, DateTime deliveryDate)
        {
            string sql = @"
                INSERT INTO ReceivedQuotations
                (RequestID, DistributorID, ProductID, ProductName, Quantity, Price, DeliveryDate, ResponseDate, Status)
                VALUES
                (@requestId, @distributorId, @productId, @productName, @quantity, @price, @deliveryDate, GETDATE(), 'Pending')";

            SqlParameter[] parameters =
            {
                new SqlParameter("@requestId", requestId),
                new SqlParameter("@distributorId", distributorId),
                new SqlParameter("@productId", productId),
                new SqlParameter("@productName", productName),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@price", price),
                new SqlParameter("@deliveryDate", deliveryDate)
            };

            return db.exeQuery(sql, parameters);
        }

        public DataSet GetSentQuotations(int distributorId)
        {
            string sql = @"
                SELECT QuotationID, RequestID, ProductID, ProductName, Quantity, Price, DeliveryDate, ResponseDate, Status, DistributorOrderID
                FROM ReceivedQuotations
                WHERE DistributorID=@distributorId
                ORDER BY ResponseDate DESC";

            return db.exeSelectQuery(sql, new SqlParameter("@distributorId", distributorId));
        }

        public int SendOrder(int quotationId, string orderId)
        {
            using (SqlConnection conn = new SqlConnection(db.conn_string))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get quotation details
                        string selectSql = @"SELECT ProductID, Quantity, DistributorID 
                                     FROM ReceivedQuotations 
                                     WHERE QuotationID=@quotationId";
                        using (SqlCommand selectCmd = new SqlCommand(selectSql, conn, transaction))
                        {
                            selectCmd.Parameters.AddWithValue("@quotationId", quotationId);
                            DataTable dt = new DataTable();
                            using (SqlDataAdapter da = new SqlDataAdapter(selectCmd))
                            {
                                da.Fill(dt);
                            }

                            if (dt.Rows.Count == 0)
                            {
                                transaction.Rollback();
                                throw new Exception("Quotation not found or not accepted by admin.");
                            }

                            int productId = Convert.ToInt32(dt.Rows[0]["ProductID"]);
                            int quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
                            int distributorId = Convert.ToInt32(dt.Rows[0]["DistributorID"]);

                            // Reduce inventory
                            string updateInventorySql = @"UPDATE Inventory 
                                                  SET Quantity = Quantity - @quantity 
                                                  WHERE ProductID=@productId AND DistributorID=@distributorId";
                            using (SqlCommand updateInventoryCmd = new SqlCommand(updateInventorySql, conn, transaction))
                            {
                                updateInventoryCmd.Parameters.AddWithValue("@quantity", quantity);
                                updateInventoryCmd.Parameters.AddWithValue("@productId", productId);
                                updateInventoryCmd.Parameters.AddWithValue("@distributorId", distributorId);
                                updateInventoryCmd.ExecuteNonQuery();
                            }

                            // Update quotation with order ID and status
                            string updateQuotationSql = @"UPDATE ReceivedQuotations 
                                                  SET DistributorOrderID=@orderId, Status='Order Sent'
                                                  WHERE QuotationID=@quotationId";
                            using (SqlCommand updateQuotationCmd = new SqlCommand(updateQuotationSql, conn, transaction))
                            {
                                updateQuotationCmd.Parameters.AddWithValue("@orderId", orderId);
                                updateQuotationCmd.Parameters.AddWithValue("@quotationId", quotationId);
                                updateQuotationCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return 1;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public DataSet GetDistributorProfile(int distributorId)
        {
            string sql = @"
                SELECT DistributorID, DistributorName, Email, Passwords 
                FROM Distributor 
                WHERE DistributorID=@distributorId";

            return db.exeSelectQuery(sql, new SqlParameter("@distributorId", distributorId));
        }

        public int UpdateDistributorProfile(int distributorId, string name, string email, string password)
        {
            string sql = @"
                UPDATE Distributor 
                SET DistributorName=@distributorname, Email=@email, Passwords=@passwords 
                WHERE DistributorID=@distributorId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@distributorname", name),
                new SqlParameter("@email", email),
                new SqlParameter("@passwords", password),
                new SqlParameter("@distributorId", distributorId)
            };

            int rowsAffected = db.exeQuery(sql, parameters);

            if (rowsAffected == 0)
                throw new Exception("No rows were updated. Check if the DistributorID exists.");

            return rowsAffected;
        }
    }
}
