using Business_layer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class ElectroCom : System.Web.UI.Page
    {
        private readonly int distributorId = 2; 
        private readonly Distributor_Dashboard dashboard = new Distributor_Dashboard();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotationRequests();
                LoadSentQuotations();
                LoadInventory();
                LoadProfile();
            }
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        private void LoadQuotationRequests()
        {
            try
            {
                DataSet ds = dashboard.GetQuotationRequests();
                gvQuotationRequests.DataSource = ds;
                gvQuotationRequests.DataBind();
            }
            catch (Exception ex)
            {
                lblQuotationsMessage.Text = "Error loading quotation requests: " + ex.Message;
                lblQuotationsMessage.CssClass = "alert alert-danger";
                lblQuotationsMessage.Visible = true;
            }
        }

        protected void btnSendQuotation_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int requestId = int.Parse(txtRequestID.Text.Trim());
                int productId = int.Parse(txtProductID_SQ.Text.Trim());
                string productName = txtProductName_SQ.Text.Trim();
                int quantity = int.Parse(txtQuantity_SQ.Text.Trim());
                decimal price = decimal.Parse(txtPrice_SQ.Text.Trim());
                DateTime deliveryDate = DateTime.Parse(txtDeliveryDate_SQ.Text.Trim());

                int rows = dashboard.SendQuotationResponse(requestId, distributorId, productId, productName, quantity, price, deliveryDate);

                lblQuotationsMessage.Text = rows > 0 ? "Quotation sent successfully." : "Failed to send quotation.";
                lblQuotationsMessage.CssClass = rows > 0 ? "alert alert-success" : "alert alert-danger";
                lblQuotationsMessage.Visible = true;

                LoadSentQuotations();
            }
            catch (Exception ex)
            {
                lblQuotationsMessage.Text = "Error sending quotation: " + ex.Message;
                lblQuotationsMessage.CssClass = "alert alert-danger";
                lblQuotationsMessage.Visible = true;
            }
        }

        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                // Quotation ID and Order ID
                int quotationId = int.Parse(txtQuotationID_Order.Text.Trim());
                string orderId = txtOrderID.Text.Trim();

                // Call business layer method
                int result = dashboard.SendOrder(quotationId, orderId);

                if (result > 0)
                {
                    lblSendOrderMessage.Text = $"Order {orderId} sent successfully for Quotation {quotationId}!";
                    lblSendOrderMessage.CssClass = "alert alert-success";
                }
                else
                {
                    lblSendOrderMessage.Text = "Failed to send order. Please check the quotation and inventory.";
                    lblSendOrderMessage.CssClass = "alert alert-warning";
                }

                lblSendOrderMessage.Visible = true;

                // Refresh grids
                LoadQuotationRequests();
                LoadSentQuotations();
                LoadInventory();
            }
            catch (FormatException)
            {
                lblSendOrderMessage.Text = "Please enter valid numeric values for Quotation ID and Order ID.";
                lblSendOrderMessage.CssClass = "alert alert-danger";
                lblSendOrderMessage.Visible = true;
            }
            catch (Exception ex)
            {
                // Detect specific inventory errors
                if (ex.Message.Contains("Quantity"))
                {
                    lblSendOrderMessage.Text = "Cannot send order: Insufficient inventory for this product.";
                    lblSendOrderMessage.CssClass = "alert alert-danger";
                }
                else
                {
                    lblSendOrderMessage.Text = "Error sending order: " + ex.Message;
                    lblSendOrderMessage.CssClass = "alert alert-danger";
                }
                lblSendOrderMessage.Visible = true;
            }
        }

        protected void btnViewQuotationRequests_Click(object sender, EventArgs e) => mvDashboard.ActiveViewIndex = 0;

        protected void btnViewSentQuotations_Click(object sender, EventArgs e)
        {
            LoadSentQuotations();
            mvDashboard.ActiveViewIndex = 1;
        }
        private void LoadSentQuotations()
        {
            try
            {
                DataSet ds = dashboard.GetSentQuotations(distributorId);
                gvSentQuotations.DataSource = ds;
                gvSentQuotations.DataBind();
            }
            catch (Exception ex)
            {
                lblSentQuotationsMessage.Text = "Error loading sent quotations: " + ex.Message;
                lblSentQuotationsMessage.CssClass = "alert alert-danger";
                lblSentQuotationsMessage.Visible = true;
            }
        }

        private void LoadInventory()
        {
            try
            {
                DataSet ds = dashboard.GetInventory(distributorId);
                gvInventory.DataSource = ds;
                gvInventory.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading inventory: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnInventory_Click(object sender, EventArgs e) { LoadInventory(); mvDashboard.ActiveViewIndex = 2; }

        protected void gvInventory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInventory.EditIndex = e.NewEditIndex;
            LoadInventory();
        }

        protected void gvInventory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInventory.EditIndex = -1;
            LoadInventory();
        }

        protected void gvInventory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvInventory.Rows[e.RowIndex];

                string productName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
                int quantity = int.Parse(((TextBox)row.Cells[2].Controls[0]).Text.Trim());
                decimal price = decimal.Parse(((TextBox)row.Cells[3].Controls[0]).Text.Trim());
                DateTime deliveryDate = DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text.Trim());

                dashboard.UpdateProduct(distributorId, productId, productName, quantity, price, deliveryDate);

                gvInventory.EditIndex = -1;
                LoadInventory();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating product: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Value);
                dashboard.DeleteProduct(distributorId, productId);
                LoadInventory();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting product: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = int.Parse(txtProductID.Text.Trim());
                string productName = txtProductName.Text.Trim();
                int quantity = int.Parse(txtQuantity.Text.Trim());
                decimal price = decimal.Parse(txtPrice.Text.Trim());
                DateTime deliveryDate = DateTime.Parse(txtDeliveryDate.Text.Trim());

                dashboard.AddProduct(distributorId, productId, productName, quantity, price, deliveryDate);
                LoadInventory();

                lblMessage.Text = "Product added successfully.";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error adding product: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
            mvDashboard.ActiveViewIndex = 3;
        }

        private void LoadProfile()
        {
            try
            {
                DataSet ds = dashboard.GetDistributorProfile(distributorId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtFullName.Text = row["DistributorName"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                    txtPassword.Text = row["Passwords"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblProfileMessage.Text = "Error loading profile: " + ex.Message;
                lblProfileMessage.CssClass = "alert alert-danger";
                lblProfileMessage.Visible = true;
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                int result = dashboard.UpdateDistributorProfile(distributorId, name, email, password);

                if (result > 0)
                {
                    lblProfileMessage.Text = "Profile updated successfully!";
                    lblProfileMessage.CssClass = "alert alert-success";
                    lblProfileMessage.Visible = true;
                }
                else
                {
                    lblProfileMessage.Text = "Profile update failed. Please check your data.";
                    lblProfileMessage.CssClass = "alert alert-warning";
                    lblProfileMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblProfileMessage.Text = "Error: " + ex.Message;
                lblProfileMessage.CssClass = "alert alert-danger";
                lblProfileMessage.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}
