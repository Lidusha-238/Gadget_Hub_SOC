<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GadgetHub_Cart.aspx.cs" Inherits="Gadget_Hub.GadgetHub_Cart" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Cart - Gadget Hub</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
       <link href="css/Cart.css" rel="stylesheet" />

</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-4">

        <h2 class="text-center mb-4">🛒 My Shopping Cart</h2>

        <%-- Message Label (always visible) --%>
        <asp:Label ID="lblMessage" runat="server"
                   CssClass="text-danger fw-bold d-block text-center mb-3"
                   Text=" "
                   Visible="true"></asp:Label>

        <%-- Cart Table --%>
        <asp:Repeater ID="rptCart" runat="server">
            <HeaderTemplate>
                <table class="table table-bordered table-striped text-center align-middle shadow-sm">
                    <thead class="table-dark">
                        <tr>
                            <th>Product</th>
                            <th>Quantity</th>
                            <th>Price per Unit</th>
                            <th>Total</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr>
                    <%-- Product Name --%>
                    <td class="fw-semibold"><%# Eval("ProductName") %></td>

                    <%-- Quantity + Update --%>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server"
                                     Text='<%# Eval("Quantity") %>'
                                     CssClass="form-control form-control-sm text-center"
                                     Width="70px"></asp:TextBox>
                        <asp:Button ID="btnUpdate" runat="server"
                                    Text="Update"
                                    CommandArgument='<%# Eval("CartID") %>'
                                    CssClass="btn btn-sm btn-primary mt-1"
                                    OnCommand="btnUpdate_Command" />
                    </td>

                    <%-- Price and Total --%>
                    <td>Rs. <%# Eval("PricePerUnit") %></td>
                    <td class="fw-bold text-success">Rs. <%# Eval("Total") %></td>

                    <%-- Remove --%>
                    <td>
                        <asp:Button ID="btnRemove" runat="server"
                                    Text="Remove"
                                    CommandArgument='<%# Eval("CartID") %>'
                                    CssClass="btn btn-sm btn-danger"
                                    OnClientClick="return confirm('Are you sure you want to remove this item from your cart?');"
                                    OnCommand="btnRemove_Command" />
                    </td>
                </tr>
            </ItemTemplate>

            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <%-- Cart Total + Checkout --%>
        <div class="d-flex justify-content-end align-items-center mt-3 gap-3">
            <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold fs-5 text-dark"></asp:Label>
            <asp:Button ID="btnCheckout" runat="server"
                        Text="Checkout"
                        CssClass="btn btn-success px-4"
                        OnClientClick="return confirm('Are you sure you want to place this order?');"
                        OnClick="btnCheckout_Click" />
        </div>

        <%-- Empty cart message if needed --%>
        <asp:Panel ID="pnlEmptyCart" runat="server" CssClass="text-center mt-4 text-muted fw-bold" Visible="false">
            Your cart is currently empty.
        </asp:Panel>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
