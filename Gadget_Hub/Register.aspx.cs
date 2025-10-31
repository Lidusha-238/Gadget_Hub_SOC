using System;
using System.Web.UI;

namespace Gadget_Hub
{
    public partial class Register : System.Web.UI.Page
    {
        localhost.UserService service = new localhost.UserService();
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string phone = txtPhone.Text.Trim();

            try
            {

                // Register User
                string result = service.RegisterUser(fullName, email, password, phone);

                if (result == "Registration successful")
                {
                    string message = "Registration successful!";
                    string redirectUrl = "MainPage.aspx";
                    string script = $"alert('{message}'); window.location='{redirectUrl}';";
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                }
                else
                {
                    lblMessage.Text = result;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }
    }
}
