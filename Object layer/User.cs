using System;
using System.Data;
using Data_Access;

namespace Business_layer
{
    public class User
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Passwords { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string RedirectPage { get; set; }  

        private Database db = new Database();

        public bool Register()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullName) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Passwords) ||
                    string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    throw new Exception("All fields are required!");
                }

                // Check if email exists
                string checkSql = $"SELECT * FROM Users WHERE Email = '{Email}'";
                DataSet ds = db.exeSelectQuery(checkSql);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Email already registered!");
                }

                // Register user
                string insertSql = $"INSERT INTO Users (FullName, Email, Passwords, PhoneNumber) " +
                                   $"VALUES ('{FullName}', '{Email}', '{Passwords}', '{PhoneNumber}')";
                int result = db.exeQuery(insertSql);
                return result > 0;
            }
            catch
            {
                throw;
            }
        }

        public bool Login()
        {
            try
            {
                // 1. Admin table
                string sqlAdmin = $"SELECT * FROM Admin WHERE Email = '{Email}' AND Passwords = '{Passwords}'";
                DataSet dsAdmin = db.exeSelectQuery(sqlAdmin);

                if (dsAdmin.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsAdmin.Tables[0].Rows[0];
                    ID = Convert.ToInt32(row["AdminID"]);
                    Role = "Admin";
                    RedirectPage = "Admin_Dashboard.aspx";
                    return true;
                }

                // 2. Users table
                string sqlUser = $"SELECT * FROM Users WHERE Email = '{Email}' AND Passwords = '{Passwords}'";
                DataSet dsUser = db.exeSelectQuery(sqlUser);

                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsUser.Tables[0].Rows[0];
                    ID = Convert.ToInt32(row["ID"]);
                    FullName = row["FullName"].ToString();
                    PhoneNumber = row["PhoneNumber"].ToString();
                    Role = "User";
                    RedirectPage = "MainPage.aspx";
                    return true;
                }

                // 3. Distributor table
                string sqlDist = $"SELECT * FROM Distributor WHERE Email = '{Email}' AND Passwords = '{Passwords}'";
                DataSet dsDist = db.exeSelectQuery(sqlDist);

                if (dsDist.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsDist.Tables[0].Rows[0];
                    ID = Convert.ToInt32(row["DistributorID"]);
                    FullName = row["DistributorName"].ToString();
                    Role = "Distributor";

                    switch (ID)
                    {
                        case 1:
                            RedirectPage = "TechWorld.aspx";
                            break;
                        case 2:
                            RedirectPage = "ElectroCom.aspx";
                            break;
                        case 3:
                            RedirectPage = "Gadget_Central.aspx";
                            break;
                        default:
                            RedirectPage = "DistributorDashboard.aspx"; 
                            break;
                    }

                    return true;
                }

                // Not found in any table
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
