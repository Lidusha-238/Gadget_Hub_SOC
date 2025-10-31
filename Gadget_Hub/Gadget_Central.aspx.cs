using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class Gadget_Central : System.Web.UI.Page
    {
        private readonly localhost1.DistributerService service = new localhost1.DistributerService();
        private readonly int distributorId = 3; // Replace with session-based ID later

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                LoadQuotationRequests();
                LoadSentQuotations();
                LoadInventory();
                LoadProfile();
                mvDashboard.ActiveViewIndex = 0;
            }
        }

        // ===================== QUOTATION REQUESTS =====================
        private void LoadQuotationRequests()
        {
            try
            {
                DataSet ds = service.GetQuotationRequests();
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
                string deliveryDate = txtDeliveryDate_SQ.Text.Trim();

                string result = service.SendQuotationResponse(requestId, distributorId, productId, productName, quantity, price, deliveryDate);

                lblQuotationsMessage.Text = result;
                lblQuotationsMessage.CssClass = "alert alert-success";
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

        // ===================== SEND ORDER =====================
        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int quotationId = int.Parse(txtQuotationID_Order.Text.Trim());
                string orderId = txtOrderID.Text.Trim();

                string result = service.SendOrder(quotationId, orderId);

                lblSendOrderMessage.Text = result.Contains("success") ?
                    $"Order {orderId} sent successfully for Quotation {quotationId}!" : result;
                lblSendOrderMessage.CssClass = result.Contains("success") ? "alert alert-success" : "alert alert-danger";
                lblSendOrderMessage.Visible = true;

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
                lblSendOrderMessage.Text = "Error sending order: " + ex.Message;
                lblSendOrderMessage.CssClass = "alert alert-danger";
                lblSendOrderMessage.Visible = true;
            }
        }

        // ===================== SENT QUOTATIONS =====================
        private void LoadSentQuotations()
        {
            try
            {
                DataSet ds = service.GetSentQuotations(distributorId);
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

        // ===================== INVENTORY =====================
        private void LoadInventory()
        {
            try
            {
                DataSet ds = service.GetInventory(distributorId);
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

        protected void btnInventory_Click(object sender, EventArgs e)
        {
            LoadInventory();
            mvDashboard.ActiveViewIndex = 2;
        }

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
                string deliveryDate = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

                string result = service.UpdateProduct(distributorId, productId, productName, quantity, price, deliveryDate);
                lblMessage.Text = result;
                lblMessage.CssClass = "alert alert-info";
                lblMessage.Visible = true;

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
                string result = service.DeleteProduct(distributorId, productId);

                lblMessage.Text = result;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;

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
                string deliveryDate = txtDeliveryDate.Text.Trim();

                string result = service.AddProduct(distributorId, productId, productName, quantity, price, deliveryDate);

                lblMessage.Text = result;
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;

                LoadInventory();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error adding product: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        // ===================== PROFILE =====================
        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
            mvDashboard.ActiveViewIndex = 3;
        }

        private void LoadProfile()
        {
            try
            {
                DataSet ds = service.GetDistributorProfile(distributorId);
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

                string result = service.UpdateDistributorProfile(distributorId, name, email, password);

                lblProfileMessage.Text = result;
                lblProfileMessage.CssClass = "alert alert-success";
                lblProfileMessage.Visible = true;

                LoadProfile();
            }
            catch (Exception ex)
            {
                lblProfileMessage.Text = "Error updating profile: " + ex.Message;
                lblProfileMessage.CssClass = "alert alert-danger";
                lblProfileMessage.Visible = true;
            }
        }

        // ===================== NAVIGATION =====================
        protected void btnViewQuotationRequests_Click(object sender, EventArgs e)
        {
            LoadQuotationRequests();
            mvDashboard.ActiveViewIndex = 0;
        }

        protected void btnViewSentQuotations_Click(object sender, EventArgs e)
        {
            LoadSentQuotations();
            mvDashboard.ActiveViewIndex = 1;
        }


        // ===================== LOGOUT =====================
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}
