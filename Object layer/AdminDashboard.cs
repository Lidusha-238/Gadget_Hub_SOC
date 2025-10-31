using Data_Access;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Business_layer
{
    public class AdminDashboard
    {
        private Database db = new Database();

        //  Send quotation request Admin to distributor
        public int RequestQuotation(string productId, string productName, int quantity)
        {
            string sql = @"
                INSERT INTO SendQuotations 
                (ProductID, ProductName, Quantity, RequestDate) 
                VALUES (@ProductID, @ProductName, @Quantity, GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@ProductName", productName),
                new SqlParameter("@Quantity", quantity)
            };

            return db.exeQuery(sql, parameters);
        }

        // Get all sent quotations by Admin
        public DataSet GetAllSentQuotations()
        {
            string sql = "SELECT * FROM SendQuotations ORDER BY RequestDate DESC";
            return db.exeSelectQuery(sql);
        }

        // Get all received quotations from distributors
        public DataSet GetReceivedQuotations()
        {
            string sql = @"
                SELECT 
                    QuotationID,          
                    RequestID,
                    DistributorID,
                    ProductID,
                    ProductName,
                    Quantity,
                    Price,
                    DeliveryDate,
                    Status,
DistributorOrderID
                FROM ReceivedQuotations
                ORDER BY QuotationID DESC";

            return db.exeSelectQuery(sql);
        }

        // Update status of received quotation 
        public int UpdateQuotationStatus(int quotationId, string status)
        {
            string sql = @"
                UPDATE ReceivedQuotations
                SET Status=@Status,
                    ResponseDate=GETDATE()
                WHERE QuotationID=@QuotationID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@QuotationID", quotationId)
            };

            return db.exeQuery(sql, parameters);
        }

        // Get details of a single received quotation
        public DataSet GetReceivedQuotationDetails(int quotationId)
        {
            string sql = @"
                SELECT 
                    QuotationID,
                    RequestID,
                    DistributorID,
                    ProductID,
                    ProductName,
                    Quantity,
                    Price,
                    DeliveryDate,
                    Status,
                    ResponseDate
                FROM ReceivedQuotations
                WHERE QuotationID=@QuotationID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@QuotationID", quotationId)
            };

            return db.exeSelectQuery(sql, parameters);
        }

        // Get all orders with product details 
        public DataSet GetAllOrdersWithDetails()
        {
            string sql = @"
                SELECT 
                    o.OrderID,
                    o.ID,
                    o.TotalAmount,
                    o.OrderDate,
                    od.ProductID,
                    od.ProductName,
                    od.Quantity,
                    o.Status,
                    o.Message
                FROM Orders o
                INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
                ORDER BY o.OrderDate DESC";

            return db.exeSelectQuery(sql);
        }

        // Get order details by Order ID
        public DataSet GetOrderDetails(int orderId)
        {
            string sql = @"
                SELECT 
                    od.OrderDetailsID,
                    od.ProductID,
                    od.ProductName,
                    od.Quantity,
                    od.PricePerUnit,
                    od.Total
                FROM OrderDetails od
                WHERE od.OrderID = @OrderID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderID", orderId)
            };

            return db.exeSelectQuery(sql, parameters);
        }

        // Accept/Reject order with message
        public int UpdateOrderStatus(int orderId, string status, string message)
        {
            string sql = @"
                UPDATE Orders 
                SET Status = @Status, Message = @Message
                WHERE OrderID = @OrderID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@Message", message ?? (object)DBNull.Value),
                new SqlParameter("@OrderID", orderId)
            };

            return db.exeQuery(sql, parameters);
        }

        // Update admin profile
        public int UpdateProfile(string email, string password)
        {
            string sql = @"
                UPDATE Admin
                SET Email=@Email, Passwords=@Passwords
                WHERE AdminID=1";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Passwords", password)
            };

            return db.exeQuery(sql, parameters);
        }
    }
}
