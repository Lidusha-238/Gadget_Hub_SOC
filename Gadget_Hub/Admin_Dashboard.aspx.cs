using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class Admin_Dashboard : System.Web.UI.Page
    {
        private localhost3.AdminService adminService = new localhost3.AdminService();

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                LoadCustomerOrders();
                LoadReceivedQuotations();
            }
        }

        #region Load Data

        private void LoadCustomerOrders()
        {
            try
            {
                DataSet ds = adminService.GetAllOrdersWithDetails();
                gvOrders.DataSource = ds?.Tables[0];
                gvOrders.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error loading orders: {ex.Message}');</script>");
            }
        }

        private void LoadReceivedQuotations()
        {
            try
            {
                DataSet ds = adminService.GetReceivedQuotations();
                gvQuotations.DataSource = ds?.Tables[0];
                gvQuotations.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error loading quotations: {ex.Message}');</script>");
            }
        }

        #endregion

        #region Sidebar Buttons

        protected void btnOrders_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 0;
            LoadCustomerOrders();
        }

        protected void btnSendQuotation_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 1;
        }

        protected void btnReceiveQuotation_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 2;
            LoadReceivedQuotations();
        }

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 3;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        #endregion

        #region Send Quotation

        protected void btnSubmitQuotation_Click(object sender, EventArgs e)
        {
            try
            {
                string productId = txtProductID.Text.Trim();
                string productName = txtProductName.Text.Trim();
                int quantity = 0;

                if (!int.TryParse(txtQuantity.Text.Trim(), out quantity))
                {
                    lblMessage.Text = "Invalid quantity.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                int result = adminService.RequestQuotation(productId, productName, quantity);

                if (result > 0)
                {
                    lblMessage.Text = "Quotation request sent successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    txtProductID.Text = "";
                    txtProductName.Text = "";
                    txtQuantity.Text = "";
                }
                else
                {
                    lblMessage.Text = "Failed to send quotation.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        #endregion

        #region Received Quotations

        protected void gvQuotations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int quotationId = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                if (e.CommandName == "Approve")
                    newStatus = "Approved";
                else if (e.CommandName == "Reject")
                    newStatus = "Rejected";
                else
                    return;

                int result = adminService.UpdateReceivedQuotationStatus(quotationId, newStatus);

                if (result > 0)
                    LoadReceivedQuotations();
                else
                    Response.Write("<script>alert('Failed to update status');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error updating quotation: {ex.Message}');</script>");
            }
        }

        #endregion

        #region Orders

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int orderId = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
                DropDownList ddlStatus = row.FindControl("ddlStatus") as DropDownList;
                TextBox txtMessage = row.FindControl("txtMessage") as TextBox;

                string newStatus = ddlStatus?.SelectedValue;
                string message = txtMessage?.Text.Trim();

                if (!string.IsNullOrEmpty(newStatus))
                {
                    adminService.UpdateOrderStatus(orderId, newStatus, message);
                    LoadCustomerOrders();
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error updating order: {ex.Message}');</script>");
            }
        }

        #endregion

        #region Profile

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                int result = adminService.UpdateProfile(email, password);

                if (result > 0)
                    Response.Write("<script>alert('Profile updated successfully!');</script>");
                else
                    Response.Write("<script>alert('Failed to update profile.');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error updating profile: {ex.Message}');</script>");
            }
        }

        #endregion
    }
}
