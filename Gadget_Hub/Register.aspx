<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Gadget_Hub.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <link href="css/Register.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-box">
            <h2>Create Account</h2>

            <!-- Full Name -->
            <asp:TextBox ID="txtFullName" runat="server" placeholder="Full Name" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtFullName" ErrorMessage="Full Name is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>

            <!-- Email -->
            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Email is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Invalid email format"
                CssClass="validator" Display="Dynamic"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

            <!-- Password -->
            <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" 
                TextMode="Password" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password must be at least 6 characters"
                CssClass="validator" Display="Dynamic" ValidationExpression="^.{6,}$"></asp:RegularExpressionValidator>

            <!-- Phone -->
            <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone Number" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="txtPhone" ErrorMessage="Phone number is required"
                CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                ControlToValidate="txtPhone" ErrorMessage="Enter a valid 10-digit phone number"
                CssClass="validator" Display="Dynamic" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>

            <!-- Register Button -->
            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button" OnClick="btnRegister_Click" BackColor="#9999FF" />

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <p>Already have an account? <a href="Login.aspx">Login here</a></p>
        </div>
    </form>
</body>
</html>