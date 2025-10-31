<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gadget_Central.aspx.cs" Inherits="Gadget_Hub.Gadget_Central" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ElectroCom Distributor Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/DistributorDashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row flex-nowrap">

                <!-- Sidebar -->
                <div class="col-auto col-md-3 col-xl-2 px-3 sidebar">
                    <h3>ElectroCom</h3>
                    <asp:Button ID="btnViewQuotationRequests" runat="server" Text="Quotation Requests"
                        CssClass="btn w-100" OnClick="btnViewQuotationRequests_Click" CausesValidation="false" />
                    <asp:Button ID="btnViewSentQuotations" runat="server" Text="Sent Quotations"
                        CssClass="btn w-100" OnClick="btnViewSentQuotations_Click" CausesValidation="false" />
                    <asp:Button ID="btnInventory" runat="server" Text="Manage Inventory"
                        CssClass="btn w-100" OnClick="btnInventory_Click" CausesValidation="false" />
                    <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile"
                        CssClass="btn w-100" OnClick="btnEditProfile_Click" CausesValidation="false" />
                    <asp:Button ID="btnLogout" runat="server" Text="Logout"
                        CssClass="btn btn-danger w-100 mt-4" OnClick="btnLogout_Click" CausesValidation="false" />
                </div>

                <!-- Main Content -->
                <div class="col py-4">
                    <asp:MultiView ID="mvDashboard" runat="server" ActiveViewIndex="0">

                        <!-- Quotation Requests -->
                        <asp:View ID="vwQuotationRequests" runat="server">
                            <div class="card shadow-sm p-3">
                                <h3 class="mb-3">Quotation Requests</h3>
                                <asp:Label ID="lblQuotationsMessage" runat="server" CssClass="alert d-none"></asp:Label>
                                <asp:GridView ID="gvQuotationRequests" runat="server" CssClass="table table-striped table-bordered"
                                    AutoGenerateColumns="false" DataKeyNames="RequestID">
                                    <Columns>
                                        <asp:BoundField DataField="RequestID" HeaderText="Request ID" ReadOnly="true" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                                    </Columns>
                                </asp:GridView>

                                <h4 class="mt-4">Send Quotation Response</h4>
                                <div class="row g-2">
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtRequestID" runat="server" CssClass="form-control" Placeholder="Request ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvRequestID" runat="server" ControlToValidate="txtRequestID"
                                            ErrorMessage="Request ID is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtProductID_SQ" runat="server" CssClass="form-control" Placeholder="Product ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProductID_SQ" runat="server" ControlToValidate="txtProductID_SQ"
                                            ErrorMessage="Product ID is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtProductName_SQ" runat="server" CssClass="form-control" Placeholder="Product Name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProductName_SQ" runat="server" ControlToValidate="txtProductName_SQ"
                                            ErrorMessage="Product Name is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtQuantity_SQ" runat="server" CssClass="form-control" Placeholder="Quantity"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvQuantity_SQ" runat="server" ControlToValidate="txtQuantity_SQ"
                                            ErrorMessage="Quantity is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtPrice_SQ" runat="server" CssClass="form-control" Placeholder="Price"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPrice_SQ" runat="server" ControlToValidate="txtPrice_SQ"
                                            ErrorMessage="Price is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-3 mt-2">
                                        <asp:TextBox ID="txtDeliveryDate_SQ" runat="server" CssClass="form-control" Placeholder="YYYY-MM-DD"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDeliveryDate_SQ" runat="server" ControlToValidate="txtDeliveryDate_SQ"
                                            ErrorMessage="Delivery Date is required" CssClass="validation-message" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID="revDeliveryDate_SQ" runat="server" ControlToValidate="txtDeliveryDate_SQ"
                                            ValidationExpression="^\d{4}-\d{2}-\d{2}$" ErrorMessage="Enter a valid date (YYYY-MM-DD)"
                                            CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                </div>
                                <asp:Button ID="btnSendQuotation" runat="server" Text="Send Quotation" CssClass="btn btn-primary mt-3" OnClick="btnSendQuotation_Click" />
                            </div>
                        </asp:View>

                        <!-- Sent Quotations -->
                        <asp:View ID="vwSentQuotations" runat="server">
                            <div class="card shadow-sm p-3">
                                <h3 class="mb-3">Sent Quotations</h3>
                                <asp:Label ID="lblSentQuotationsMessage" runat="server" CssClass="alert d-none"></asp:Label>
                                <asp:GridView ID="gvSentQuotations" runat="server" CssClass="table table-striped table-bordered"
                                    AutoGenerateColumns="false" DataKeyNames="QuotationID">
                                    <Columns>
                                        <asp:BoundField DataField="QuotationID" HeaderText="Quotation ID" ReadOnly="true" />
                                        <asp:BoundField DataField="RequestID" HeaderText="Request ID" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                        <asp:BoundField DataField="Price" HeaderText="Price (LKR)" />
                                        <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                    </Columns>
                                </asp:GridView>

                                <h4 class="mt-4">Send Order</h4>
                                <div class="row g-2">
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtQuotationID_Order" runat="server" CssClass="form-control" Placeholder="Quotation ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvQuotationID_Order" runat="server" ControlToValidate="txtQuotationID_Order"
                                            ErrorMessage="Quotation ID is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtOrderID" runat="server" CssClass="form-control" Placeholder="Order ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvOrderID" runat="server" ControlToValidate="txtOrderID"
                                            ErrorMessage="Order ID is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                </div>
                                <asp:Button ID="btnSendOrder" runat="server" Text="Send Order" CssClass="btn btn-primary mt-3" OnClick="btnSendOrder_Click" />
                                <asp:Label ID="lblSendOrderMessage" runat="server" CssClass="alert mt-2 d-none"></asp:Label>
                            </div>
                        </asp:View>

                        <!-- Inventory Management -->
                        <asp:View ID="vwInventory" runat="server">
                            <div class="card shadow-sm p-3">
                                <h3 class="mb-3">Inventory Management</h3>
                                <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none"></asp:Label>
                                <asp:GridView ID="gvInventory" runat="server" CssClass="table table-bordered"
                                    AutoGenerateColumns="False" DataKeyNames="ProductID"
                                    OnRowEditing="gvInventory_RowEditing"
                                    OnRowUpdating="gvInventory_RowUpdating"
                                    OnRowCancelingEdit="gvInventory_RowCancelingEdit"
                                    OnRowDeleting="gvInventory_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" ReadOnly="true" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                        <asp:BoundField DataField="PricePerUnit" HeaderText="Price (LKR)" />
                                        <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                                    </Columns>
                                </asp:GridView>

                                <h4 class="mt-4">Add New Product</h4>
                                <div class="row g-2">
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control" Placeholder="Product ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProductID" runat="server" ControlToValidate="txtProductID"
                                            ErrorMessage="Product ID is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" Placeholder="Product Name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                                            ErrorMessage="Product Name is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Placeholder="Quantity"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                            ErrorMessage="Quantity is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Placeholder="Price"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice"
                                            ErrorMessage="Price is required" CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" Placeholder="YYYY-MM-DD"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                            ErrorMessage="Delivery Date is required" CssClass="validation-message" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID="revDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                            ValidationExpression="^\d{4}-\d{2}-\d{2}$" ErrorMessage="Enter a valid date (YYYY-MM-DD)"
                                            CssClass="validation-message" Display="Dynamic" />
                                    </div>
                                </div>
                                <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-success mt-3" OnClick="btnAddProduct_Click" />
                            </div>
                        </asp:View>

                        <!-- Edit Profile -->
                        <asp:View ID="vwEditProfile" runat="server">
                            <div class="card shadow-sm p-3">
                                <h3 class="mb-3">Edit Profile</h3>
                                <asp:Label ID="lblProfileMessage" runat="server" CssClass="alert d-none"></asp:Label>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control mb-2" Placeholder="Full Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                    ErrorMessage="Full Name is required" CssClass="validation-message" Display="Dynamic" />

                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-2" Placeholder="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email is required" CssClass="validation-message" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address"
                                    CssClass="validation-message" Display="Dynamic" />

                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mb-3" TextMode="Password" Placeholder="New Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Password is required" CssClass="validation-message" Display="Dynamic" />
                                <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" CssClass="btn btn-success" OnClick="btnSaveProfile_Click" />
                            </div>
                        </asp:View>

                    </asp:MultiView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
