using Data_Access;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Business_layer
{
    public class User_Dashboard
    {
        private Database db = new Database();

        // Get all orders for a specific user
        public DataSet GetUserOrders(int userId)
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
                WHERE o.ID = @ID
                ORDER BY o.OrderDate DESC";

            SqlParameter param = new SqlParameter("@ID", userId);
            return db.exeSelectQuery(sql, param);
        }

        // Cancel an order
        public int CancelOrder(int orderId)
        {
            string sql = @"
                UPDATE Orders
                SET Status = 'Cancelled'
                WHERE OrderID = @OrderID";

            SqlParameter param = new SqlParameter("@OrderID", orderId);
            return db.exeQuery(sql, param);
        }

        //  Update user profile
        public int UpdateProfile(int userId, string fullName, string phone, string password)
        {
            string sql = @"
                UPDATE Users 
                SET 
                    FullName = @FullName,
                    PhoneNumber = @PhoneNumber,
                    Passwords = @Passwords
                WHERE ID = @ID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@PhoneNumber", phone),
                new SqlParameter("@Passwords", password),
                new SqlParameter("@ID", userId)
            };

            return db.exeQuery(sql, parameters);
        }

        // Get full profile details for display
        public DataSet GetUserProfile(int userId)
        {
            string sql = @"
                SELECT 
                    FullName,
                    Email,
                    PhoneNumber,
                    Passwords
                FROM Users 
                WHERE ID = @ID";

            SqlParameter param = new SqlParameter("@ID", userId);
            return db.exeSelectQuery(sql, param);
        }

        //  Get only FullName for welcome label
        public string GetUserNameById(int userId)
        {
            string sql = "SELECT FullName FROM Users WHERE ID = @ID";
            SqlParameter param = new SqlParameter("@ID", userId);
            DataSet ds = db.exeSelectQuery(sql, param);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["FullName"].ToString();

            return string.Empty;
        }
    }
}
