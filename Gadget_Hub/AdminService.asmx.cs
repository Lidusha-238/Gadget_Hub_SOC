using System;
using System.Data;
using System.Web.Services;
using Business_layer;

namespace Gadget_Hub
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AdminService : WebService
    {
        private AdminDashboard admin = new AdminDashboard();

        [WebMethod]
        public int RequestQuotation(string productId, string productName, int quantity)
        {
            return admin.RequestQuotation(productId, productName, quantity);
        }

        [WebMethod]
        public DataSet GetAllSentQuotations()
        {
            return admin.GetAllSentQuotations();
        }

        [WebMethod]
        public DataSet GetReceivedQuotations()
        {
            return admin.GetReceivedQuotations();
        }

        [WebMethod]
        public int UpdateReceivedQuotationStatus(int quotationId, string status)
        {
            return admin.UpdateQuotationStatus(quotationId, status);
        }

        [WebMethod]
        public DataSet GetAllOrdersWithDetails()
        {
            return admin.GetAllOrdersWithDetails();
        }

        [WebMethod]
        public DataSet GetOrderDetails(int orderId)
        {
            return admin.GetOrderDetails(orderId);
        }

        [WebMethod]
        public int UpdateOrderStatus(int orderId, string status, string message)
        {
            return admin.UpdateOrderStatus(orderId, status, message);
        }

        [WebMethod]
        public int UpdateProfile(string email, string password)
        {
            return admin.UpdateProfile(email, password);
        }
    }
}
