using System;
using System.Data;
using System.Web.Services;
using Business_layer;

namespace Gadget_Hub
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService] // Optional: enables AJAX calls
    public class MainPageService : WebService
    {
        private readonly Main_Page mainPage = new Main_Page();

        // ✅ Get product list
        [WebMethod]
        public DataSet GetProducts(string search = "")
        {
            return mainPage.GetProducts(search);
        }

        // ✅ Add product to cart
        [WebMethod]
        public bool AddToCart(int userId, int productId, int quantity = 1)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0)
                throw new ArgumentException("Invalid parameters for adding product to cart.");

            try
            {
                return mainPage.AddToCart(userId, productId, quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product to cart: " + ex.Message);
            }
        }
    }
}
