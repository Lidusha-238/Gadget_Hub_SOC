using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class GadgetHub_Cart : System.Web.UI.Page
    {
        private readonly localhost4.CartService service = new localhost4.CartService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCart();
        }

        // Get logged-in user ID 
        private int GetCurrentUserId()
        {
            if (Session["ID"] != null && int.TryParse(Session["ID"].ToString(), out int userId))
                return userId;

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest(); 
            return 0;
        }

        // Load cart items for the current user
        private void LoadCart()
        {
            try
            {
                int userId = GetCurrentUserId();
                if (userId == 0) return;

                DataSet ds = service.GetCartItems(userId);

                bool hasItems = ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;

                rptCart.DataSource = hasItems ? ds : null;
                rptCart.DataBind();

                pnlEmptyCart.Visible = !hasItems;
                rptCart.Visible = hasItems;

                if (hasItems)
                {
                    decimal total = service.GetCartTotal(userId);
                    lblTotal.Text = $"Total: Rs. {total:N2}";
                    lblMessage.Text = string.Empty;
                }
                else
                {
                    lblTotal.Text = string.Empty;
                    lblMessage.Text = "🛒 Your cart is empty!";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading cart: " + ex.Message;
            }
        }

        // Remove product from cart
        protected void btnRemove_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int cartId = Convert.ToInt32(e.CommandArgument);
                bool removed = service.RemoveFromCart(cartId);

                lblMessage.Text = removed
                    ? "Item removed successfully!"
                    : "Error removing item.";

                LoadCart();
            }
            catch (Exception ex)
            {
                lblMessage.Text = " Error removing item: " + ex.Message;
            }
        }

        // Update product quantity
        protected void btnUpdate_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int cartId = Convert.ToInt32(e.CommandArgument);
                RepeaterItem item = ((Button)sender).NamingContainer as RepeaterItem;
                TextBox txtQuantity = item?.FindControl("txtQuantity") as TextBox;

                if (txtQuantity == null || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    lblMessage.Text = "Please enter a valid quantity.";
                    return;
                }

                bool updated = service.UpdateQuantity(cartId, quantity);

                lblMessage.Text = updated
                    ? "Quantity updated successfully!"
                    : "Error updating quantity.";

                LoadCart();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "⚠️ Error updating quantity: " + ex.Message;
            }
        }

        // Checkout and save order
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = GetCurrentUserId();
                if (userId == 0) return;

                bool success = service.CheckoutAndSaveOrders(userId);

                lblMessage.Text = success
                    ? "Checkout successful! Your order has been saved."
                    : "Your cart is empty.";

                LoadCart();
            }
            catch (Exception ex)
            {
                lblMessage.Text = " Error during checkout: " + ex.Message;
            }
        }
    }
}
