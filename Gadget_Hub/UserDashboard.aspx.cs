using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gadget_Hub
{
    public partial class UserDashboard : System.Web.UI.Page
    {
        // Web service reference
        localhost5.UserDashboardService service = new localhost5.UserDashboardService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is logged in
                if (Session["ID"] != null)
                {
                    int userId = Convert.ToInt32(Session["ID"]);
                    string fullName = service.GetUserProfile(userId)?.Tables[0].Rows.Count > 0
                        ? service.GetUserProfile(userId).Tables[0].Rows[0]["FullName"].ToString()
                        : Session["FullName"]?.ToString() ?? "User";

                    lblName.Text = fullName;
                    Session["FullName"] = fullName;

                    mvUser.ActiveViewIndex = 0; // Default view: Dashboard
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }



        // Edit Profile Button
        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            mvUser.ActiveViewIndex = 0;
            LoadUserProfile();
        }

        // My Orders Button
        protected void btnOrders_Click(object sender, EventArgs e)
        {
            mvUser.ActiveViewIndex = 1;
            LoadUserOrders();
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        // Load user profile from Web Service
        private void LoadUserProfile()
        {
            try
            {
                int userId = Convert.ToInt32(Session["ID"]);
                DataSet ds = service.GetUserProfile(userId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtFullName.Text = row["FullName"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                    txtPhone.Text = row["PhoneNumber"].ToString();
                    txtPassword.Text = row["Passwords"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "";
                lblError.Text = "Error loading profile: " + ex.Message;
            }
        }

        // Load user orders via Web Service
        private void LoadUserOrders()
        {
            try
            {
                int userId = Convert.ToInt32(Session["ID"]);
                DataSet ds = service.GetUserOrders(userId);

                gvOrders.DataSource = (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
                gvOrders.DataBind();
            }
            catch (Exception ex)
            {
                gvOrders.DataSource = null;
                gvOrders.DataBind();
                lblMessage.Text = "";
                lblError.Text = "Error loading orders: " + ex.Message;
            }
        }

        // Update profile 
        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["ID"]);
                string fullName = txtFullName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string password = txtPassword.Text.Trim();

                int result = service.UpdateUserProfile(userId, fullName, phone, password);

                if (result > 0)
                {
                    lblMessage.Text = "Profile updated successfully!";
                    lblError.Text = "";
                    Session["FullName"] = fullName;
                    lblName.Text = fullName;
                }
                else
                {
                    lblMessage.Text = "";
                    lblError.Text = "Failed to update profile.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "";
                lblError.Text = "Error updating profile: " + ex.Message;
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelOrder")
            {
                try
                {
                    int orderId = Convert.ToInt32(e.CommandArgument);
                    int userId = Convert.ToInt32(Session["ID"]);

                    // Call web service to cancel the order
                    int result = service.CancelOrder(orderId); 

                    if (result > 0)
                    {
                        lblMessage.Text = "Order cancelled successfully!";
                        lblError.Text = "";
                        LoadUserOrders();
                    }
                    else
                    {
                        lblMessage.Text = "";
                        lblError.Text = "Failed to cancel order.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "";
                    lblError.Text = "Error cancelling order: " + ex.Message;
                }
            }
        }
    }
}
