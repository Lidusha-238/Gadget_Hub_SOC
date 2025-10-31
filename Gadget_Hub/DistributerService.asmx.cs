using Business_layer;
using System;
using System.Data;
using System.Web.Services;

namespace Gadget_Hub
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DistributerService : WebService
    {
        private readonly Distributor_Dashboard dashboard = new Distributor_Dashboard();


        [WebMethod]
        public DataSet GetInventory(int distributorId)
        {
            return dashboard.GetInventory(distributorId);
        }

        [WebMethod]
        public string AddProduct(int distributorId, int productId, string productName, int quantity, decimal price, string deliveryDate)
        {
            try
            {
                DateTime parsedDate = DateTime.Parse(deliveryDate);
                return dashboard.AddProduct(distributorId, productId, productName, quantity, price, parsedDate) > 0
                    ? "Product added successfully."
                    : "Failed to add product.";
            }
            catch
            {
                return "Invalid date format.";
            }
        }

        [WebMethod]
        public string UpdateProduct(int distributorId, int productId, string productName, int quantity, decimal price, string deliveryDate)
        {
            try
            {
                DateTime parsedDate = DateTime.Parse(deliveryDate);
                return dashboard.UpdateProduct(distributorId, productId, productName, quantity, price, parsedDate) > 0
                    ? "Product updated successfully."
                    : "Failed to update product.";
            }
            catch
            {
                return "Invalid date format.";
            }
        }

        [WebMethod]
        public string DeleteProduct(int distributorId, int productId)
        {
            return dashboard.DeleteProduct(distributorId, productId) > 0
                ? "Product deleted successfully."
                : "Failed to delete product.";
        }

   
        [WebMethod]
        public DataSet GetQuotationRequests()
        {
            return dashboard.GetQuotationRequests();
        }

        [WebMethod]
        public DataSet GetSentQuotations(int distributorId)
        {
            return dashboard.GetSentQuotations(distributorId);
        }

        [WebMethod]
        public string SendQuotationResponse(int requestId, int distributorId, int productId, string productName, int quantity, decimal price, string deliveryDate)
        {
            try
            {
                DateTime parsedDate = DateTime.Parse(deliveryDate);
                return dashboard.SendQuotationResponse(requestId, distributorId, productId, productName, quantity, price, parsedDate) > 0
                    ? "Quotation sent successfully."
                    : "Failed to send quotation.";
            }
            catch
            {
                return "Invalid date format.";
            }
        }


        [WebMethod]
        public string SendOrder(int quotationId, string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return "Error: Order ID cannot be empty.";

            try
            {
                int result = dashboard.SendOrder(quotationId, orderId);
                if (result > 0)
                    return "Order sent successfully and inventory updated.";
                else
                    return "Failed to send order. Check quotation status.";
            }
            catch (Exception ex)
            {
                return $"Error sending order: {ex.Message}";
            }
        }

        [WebMethod]
        public DataSet GetDistributorProfile(int distributorId)
        {
            return dashboard.GetDistributorProfile(distributorId);
        }

        [WebMethod]
        public string UpdateDistributorProfile(int distributorId, string name, string email, string password)
        {
            try
            {
                return dashboard.UpdateDistributorProfile(distributorId, name, email, password) > 0
                    ? "Profile updated successfully."
                    : "Failed to update profile.";
            }
            catch (Exception ex)
            {
                return $"Error updating profile: {ex.Message}";
            }
        }
    }
}
