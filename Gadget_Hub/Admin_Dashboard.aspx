<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Dashboard.aspx.cs" Inherits="Gadget_Hub.Admin_Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/AdminDashboard.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
    <div class="container-fluid">
        <div class="row flex-nowrap">
            
            <!-- Sidebar -->
            <div class="col-auto col-md-3 col-xl-2 px-sm-2 px-0">
                <div class="d-flex flex-column align-items-sm-start px-3 pt-3 min-vh-100">
                    <h3 class="mb-4">Admin Panel</h3>
                    <asp:Button ID="btnOrders" runat="server" Text="View Orders" CssClass="btn btn-link mb-2" OnClick="btnOrders_Click" CausesValidation="False" />
                    <asp:Button ID="btnSendQuotation" runat="server" Text="Send Quotation" CssClass="btn btn-link mb-2" OnClick="btnSendQuotation_Click" CausesValidation="False" />
                    <asp:Button ID="btnReceiveQuotation" runat="server" Text="Received Quotations" CssClass="btn btn-link mb-2" OnClick="btnReceiveQuotation_Click" CausesValidation="False" />
                    <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" CssClass="btn btn-link mb-2" OnClick="btnEditProfile_Click" CausesValidation="False" />
                    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-danger mt-3" OnClick="btnLogout_Click" CausesValidation="False" />
                </div>
            </div>

            <!-- Main Content -->
            <div class="col py-3">
                <asp:MultiView ID="mvDashboard" runat="server" ActiveViewIndex="0">

                    <!-- Orders View -->
                    <asp:View ID="vwOrders" runat="server">
                        <h3>Customer Orders</h3>
                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered"
                            DataKeyNames="OrderID" OnRowCommand="gvOrders_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                                <asp:BoundField DataField="ID" HeaderText="User ID" />
                                <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Message" HeaderText="Message" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm mt-1">
                                            <asp:ListItem Text="Accepted" Value="Accepted" />
                                            <asp:ListItem Text="Rejected" Value="Rejected" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control form-control-sm mt-1" Placeholder="Optional message" />
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary btn-sm mt-1" CommandName="UpdateStatus" CommandArgument='<%# Eval("OrderID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:View>

                    <!-- Send Quotation View -->
                    <asp:View ID="vwSendQuotation" runat="server">
                        <h3>Send Quotation</h3>
                        <div class="mb-3">
                            <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control" Placeholder="Product ID"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductID" runat="server" ControlToValidate="txtProductID"
                                ErrorMessage="Product ID is required" CssClass="validation-message" Display="Dynamic" />
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" Placeholder="Product Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                                ErrorMessage="Product Name is required" CssClass="validation-message" Display="Dynamic" />
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Placeholder="Quantity" TextMode="Number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                ErrorMessage="Quantity is required" CssClass="validation-message" Display="Dynamic" />
                        </div>
                        <asp:Button ID="btnSubmitQuotation" runat="server" Text="Send" CssClass="btn btn-primary" OnClick="btnSubmitQuotation_Click" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2"></asp:Label>
                    </asp:View>

                    <!-- Received Quotations View -->
                    <asp:View ID="vwReceiveQuotation" runat="server">
                        <h3>Received Quotations</h3>
                        <asp:GridView ID="gvQuotations" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="QuotationID" OnRowCommand="gvQuotations_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="QuotationID" HeaderText="Quotation ID" />
                                <asp:BoundField DataField="DistributorID" HeaderText="Distributor ID" />
                                <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success btn-sm me-1"
                                            CommandName="Approve" CommandArgument='<%# Eval("QuotationID") %>' />
                                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm"
                                            CommandName="Reject" CommandArgument='<%# Eval("QuotationID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DistributorOrderID" HeaderText="Distributor Order ID" />
                            </Columns>
                        </asp:GridView>
                    </asp:View>

                    <!-- Edit Profile View -->
                    <asp:View ID="vwEditProfile" runat="server">
                        <h3>Edit Profile</h3>
                        <div class="mb-3">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Email"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is required" CssClass="validation-message" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address"
                                CssClass="validation-message" Display="Dynamic" />
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="New Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="Password is required" CssClass="validation-message" Display="Dynamic" />
                        </div>
                        <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" CssClass="btn btn-success" OnClick="btnSaveProfile_Click" />
                    </asp:View>

                </asp:MultiView>
            </div>
        </div>
    </div>
</form>
</body>
</html>
