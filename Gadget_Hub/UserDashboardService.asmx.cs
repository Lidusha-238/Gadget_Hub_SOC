using System;
using System.Data;
using System.Web.Services;
using Business_layer;

namespace Gadget_Hub
{
    /// <summary>
    /// Web service for user dashboard operations — handles orders and profile management.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // [System.Web.Script.Services.ScriptService] // Uncomment if using with AJAX
    public class UserDashboardService : WebService
    {
        private readonly User_Dashboard userDashboard = new User_Dashboard();

        // Get all orders for a specific user
        [WebMethod]
        public DataSet GetUserOrders(int userId)
        {
            try
            {
                return userDashboard.GetUserOrders(userId);
            }
            catch (Exception ex)
            {
                return CreateErrorDataSet(ex.Message);
            }
        }

        // Update user profile
        [WebMethod]
        public int UpdateUserProfile(int userId, string fullName, string phone, string password)
        {
            try
            {
                return userDashboard.UpdateProfile(userId, fullName, phone, password);
            }
            catch
            {
                return 0; // failure
            }
        }

        // Get full user profile details for display
        [WebMethod]
        public DataSet GetUserProfile(int userId)
        {
            try
            {
                return userDashboard.GetUserProfile(userId);
            }
            catch (Exception ex)
            {
                return CreateErrorDataSet(ex.Message);
            }
        }

        // Get user's full name for welcome label
        [WebMethod]
        public string GetUserName(int userId)
        {
            try
            {
                string fullName = userDashboard.GetUserNameById(userId);
                return string.IsNullOrEmpty(fullName) ? "User" : fullName;
            }
            catch
            {
                return "User";
            }
        }

        //  Cancel an order
        [WebMethod]
        public int CancelOrder(int orderId)
        {
            try
            {
                return userDashboard.CancelOrder(orderId);
            }
            catch
            {
                return 0; 
            }
        }

        // Helper: Create a simple error DataSet
        private DataSet CreateErrorDataSet(string message)
        {
            DataSet ds = new DataSet("Error");
            ds.Tables.Add("ErrorTable");
            ds.Tables[0].Columns.Add("Message");
            ds.Tables[0].Rows.Add(message);
            return ds;
        }
    }
}
