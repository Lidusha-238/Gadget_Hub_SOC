using System;
using System.Web.UI;

namespace Gadget_Hub
{
    public partial class Login : System.Web.UI.Page
    {
        localhost.UserService service = new localhost.UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string result = service.LoginUser(email, password);

            string[] parts = result.Split('|');

            if (parts[0] == "SUCCESS" && parts.Length >= 3)
            {
                if (int.TryParse(parts[1], out int userId))
                {
                    // Save session consistently
                    Session["ID"] = userId;
                    Session["Email"] = email;

                    // Login Success Message
                    Session["LoginMessage"] = "Login successful!";

                    // Redirect to page
                    Response.Redirect(parts[2], false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = parts.Length > 1 ? parts[1] : "Login failed!";
            }
        }
    }
}

