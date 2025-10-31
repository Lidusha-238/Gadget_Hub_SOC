<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gadget_Hub.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="css/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>Login</h2>

            <!-- Email -->
            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Email is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Invalid email format"
                CssClass="validator" Display="Dynamic"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

            <!-- Password -->
            <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" 
                TextMode="Password" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
    ControlToValidate="txtPassword" ErrorMessage="Password must be at least 6 characters"
    CssClass="validator" Display="Dynamic" ValidationExpression="^.{6,}$"></asp:RegularExpressionValidator>

            <!-- Login Button -->
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button" OnClick="btnLogin_Click" BackColor="#9999FF" />

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <p>Don’t have an account? <a href="Register.aspx">Register here</a></p>
        </div>
    </form>
</body>
</html>
