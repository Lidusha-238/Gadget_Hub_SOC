using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class MainPage : System.Web.UI.Page
    {
        // Reference the updated web service
        private readonly localhost2.MainPageService main = new localhost2.MainPageService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Show login success message if available
                if (Session["LoginMessage"] != null)
                {
                    lblMessage.Text = Session["LoginMessage"].ToString();
                    lblMessage.CssClass = "text-success mb-3 d-block";
                    Session.Remove("LoginMessage"); // show only once

                    // Register JS to fade out after 3 seconds
                    string script = $@"
                        setTimeout(function() {{
                            var lbl = document.getElementById('{lblMessage.ClientID}');
                            if(lbl) {{ lbl.style.transition = 'opacity 1s'; lbl.style.opacity = '0'; }}
                        }}, 3000);";
                    ClientScript.RegisterStartupScript(this.GetType(), "FadeMessage", script, true);
                }

                BindProducts();
            }
        }

        // -------------------- Bind Product List --------------------
        private void BindProducts(string search = "")
        {
            DataSet ds = main.GetProducts(search);

            if (ds != null && ds.Tables.Count > 0)
            {
                rptProducts.DataSource = ds;
                rptProducts.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts(txtSearch.Text.Trim());
        }

        // -------------------- Add Product to Cart --------------------
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (!int.TryParse(btn.CommandArgument, out int productId))
            {
                lblMessage.Text = "Invalid product selection.";
                return;
            }

            // Get logged-in user ID from session
            if (Session["ID"] != null && int.TryParse(Session["ID"].ToString(), out int userId))
            {
                int quantity = 1; // default quantity

                try
                {
                    // ✅ AddToCart now returns bool and uses ProductName & MarketPrice from Inventory
                    bool added = main.AddToCart(userId, productId, quantity);

                    lblMessage.Text = added
                        ? "Product added to cart successfully!"
                        : "Failed to add product to cart.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error adding product: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Please log in to add products to the cart.";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("Login.aspx");
        }
    }
}
