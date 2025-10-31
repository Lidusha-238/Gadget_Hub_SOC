<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="Gadget_Hub.MainPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shop - Gadget Hub</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="css/MainPage.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- Navbar -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand fw-bold" href="#">Gadget Hub</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse justify-content-end" id="navbarNav">
                    <ul class="navbar-nav">
                        <!-- Profile Link -->
                        <li class="nav-item me-3">
                            <a class="nav-link" href="UserDashboard.aspx">
                                <i class="bi bi-person-circle fs-5"></i> Profile
                            </a>
                        </li>

                        <!-- Cart Link -->
                        <li class="nav-item me-3">
                            <a class="nav-link" href="GadgetHub_Cart.aspx">
                                <i class="bi bi-cart4 fs-5"></i> Cart
                            </a>
                        </li>

                        <!-- Logout Button -->
                        <li class="nav-item">
                            <asp:LinkButton ID="btnLogout" runat="server" CssClass="nav-link" OnClick="btnLogout_Click">
                                <i class="bi bi-box-arrow-right fs-5"></i> Logout
                            </asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- Main Content -->
        <div class="container mt-4">
            <h2 class="text-center mb-4">Get All You Need</h2>

            <!-- Search Bar -->
            <div class="input-group mb-4">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search product..."></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-dark" Text="Search" OnClick="btnSearch_Click" />
            </div>

            <!-- Product Grid -->
            <div class="row">
                 <!-- Message Label -->
                <asp:Label ID="lblMessage" runat="server" CssClass="text-success mb-3 d-block"></asp:Label>
                <asp:Repeater ID="rptProducts" runat="server">
                    <ItemTemplate>
                        <div class="col-md-3 col-sm-6 mb-4 d-flex">
                            <div class="card product-card w-100">
                                <img src='<%# Eval("ImageUrl") %>' class="card-img-top product-img" alt='<%# Eval("ProductName") %>' />
                                <div class="card-body text-center">
                                    <h5 class="card-title"><%# Eval("ProductName") %></h5>
                                    <p class="text-muted mb-2">$<%# String.Format("{0:0.00}", Eval("MarketPrice")) %></p>
                                    <asp:Button ID="btnAddToCart" runat="server" CommandArgument='<%# Eval("ProductID") %>'
                                        CssClass="btn btn-purple w-100" Text="Add to Cart" OnClick="btnAddToCart_Click" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
