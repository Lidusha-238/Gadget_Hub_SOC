using System.Web.Services;
using System;
using Business_layer;

namespace Gadget_Hub
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class UserService : System.Web.Services.WebService
    {
        [WebMethod]
        public string RegisterUser(string fullName, string email, string password, string phoneNumber)
        {
            try
            {
                User user = new User
                {
                    FullName = fullName,
                    Email = email,
                    Passwords = password,
                    PhoneNumber = phoneNumber
                };

                bool result = user.Register();
                return result ? "Registration successful" : "Registration failed";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public string LoginUser(string email, string password)
        {
            try
            {
                User user = new User
                {
                    Email = email,
                    Passwords = password
                };

                bool success = user.Login();

                if (success)
                {
                    
                    int userId = user.ID;         
                    string redirectPage = user.RedirectPage; 


                    return $"SUCCESS|{userId}|{redirectPage}";
                }
                else
                {
                    return "ERROR|Invalid email or password";
                }
            }
            catch (Exception ex)
            {
                return "ERROR|" + ex.Message;
            }
        }
    }
}
