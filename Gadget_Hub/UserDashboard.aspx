<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="Gadget_Hub.UserDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="css/UserDashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row flex-nowrap">

                <!-- Sidebar -->
                <div class="col-auto sidebar">
                    <h3>Welcome, <asp:Label ID="lblName" runat="server" Text="User"></asp:Label></h3>
                    <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" CssClass="btn btn-link mb-2" OnClick="btnEditProfile_Click" />
                    <asp:Button ID="btnOrders" runat="server" Text="My Orders" CssClass="btn btn-link mb-2" OnClick="btnOrders_Click" />
                    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn logout-btn mt-auto" OnClick="btnLogout_Click" />
                </div>

                <!-- Main Content -->
                <div class="col main-content">
                    <asp:MultiView ID="mvUser" runat="server" ActiveViewIndex="0">

                        <!-- Edit Profile View -->
                        <asp:View ID="vwEditProfile" runat="server">
                            <h3>Edit Profile</h3>
                            <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-2 d-none"></asp:Label>
                            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger mt-2 d-none"></asp:Label>

                            <div class="mb-3">
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Placeholder="Full Name"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Email" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Placeholder="Phone"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Password"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnUpdateProfile" runat="server" Text="Save Changes" CssClass="btn btn-success" OnClick="btnUpdateProfile_Click" />
                        </asp:View>

                        <!-- Orders View -->
                        <asp:View ID="vwOrders" runat="server">
                            <h3>My Orders</h3>
                            <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped table-hover"
                                AutoGenerateColumns="False" EmptyDataText="No orders found." DataKeyNames="OrderID" OnRowCommand="gvOrders_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Message" HeaderText="Message" />
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:Button ID="btnCancelOrder" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm grid-btn"
                                                CommandName="CancelOrder" CommandArgument='<%# Eval("OrderID") %>' 
                                                OnClientClick="return confirm('Are you sure you want to cancel this order?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:View>

                    </asp:MultiView>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
