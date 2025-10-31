using System;
using System.Data;
using System.Web.Services;
using Business_layer;

namespace Gadget_Hub
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CartService : WebService
    {
        private readonly Cart cart = new Cart();

        // Get all cart items for a specific user
        [WebMethod]
        public DataSet GetCartItems(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            try
            {
                return cart.GetCartItems(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving cart items: " + ex.Message);
            }
        }

        // Get total of the cart
        [WebMethod]
        public decimal GetCartTotal(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            try
            {
                return cart.GetCartTotal(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating cart total: " + ex.Message);
            }
        }

        // Add product to cart 
        [WebMethod]
        public bool AddToCart(int userId, int productId, string productName, int quantity, decimal marketPrice)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0 || string.IsNullOrWhiteSpace(productName) || marketPrice <= 0)
                throw new ArgumentException("Invalid parameters for adding product to cart.");

            try
            {
                return cart.AddToCart(userId, productId, productName, quantity, marketPrice);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product to cart: " + ex.Message);
            }
        }

        // Remove product from cart
        [WebMethod]
        public bool RemoveFromCart(int cartId)
        {
            if (cartId <= 0)
                throw new ArgumentException("Invalid cart ID.");

            try
            {
                return cart.RemoveFromCart(cartId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing item from cart: " + ex.Message);
            }
        }

        // Update quantity of product in cart
        [WebMethod]
        public bool UpdateQuantity(int cartId, int quantity)
        {
            if (cartId <= 0 || quantity <= 0)
                throw new ArgumentException("Invalid cart ID or quantity.");

            try
            {
                return cart.UpdateQuantity(cartId, quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating quantity: " + ex.Message);
            }
        }

        // Checkout 
        [WebMethod]
        public bool CheckoutAndSaveOrders(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            try
            {
                return cart.CheckoutAndSaveOrders(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Checkout failed: " + ex.Message);
            }
        }
    }
}
