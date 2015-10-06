<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword_ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <link href="../StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <div id="header">
         <div class="left">
            <a href="/"><img src="../images/logo.png" alt="Logo" /></a>
         </div>
    </div>
    <div id="wrapper">
    <form id="form1" runat="server">
    <div>
        <asp:LoginView ID="LoginView1" Runat="server" Visible="true">
            <LoggedInTemplate>
            <asp:LoginName ID="LoginName1" runat="server" Font-Bold = "true" FormatString="Welcome {0}!"/>
            </LoggedInTemplate>
            <AnonymousTemplate>
                You are not logged in
            </AnonymousTemplate>
        </asp:LoginView>
            <asp:LoginStatus ID="LoginStatus1" runat="server" />

        <div class="nav">
        <ul>
        <li><asp:HyperLink ID="MainPg" runat="server" NavigateUrl="~/Default.aspx">SQL Practice</asp:HyperLink></li>
        <li><asp:HyperLink ID="ChangPwd" runat="server" NavigateUrl="~/ChangePassword/ChangePassword.aspx">Change Password</asp:HyperLink></li>
        <li><asp:HyperLink ID="AdminPg" runat="server" NavigateUrl="~/Admin/Admin.aspx" Visible="False">User Management</asp:HyperLink></li>
        <li><asp:HyperLink ID="DBPg" runat="server" NavigateUrl="~/Admin/DatabaseManagement.aspx" Visible="False">Database Management</asp:HyperLink></li>
         </ul>
        </div>
        <asp:Label ID="Label4" runat="server" Text="User name" Width="150px" 
            Font-Bold="True" ForeColor="#996633"></asp:Label>
        <asp:TextBox ID="txt_user" runat="server" TextMode="SingleLine"></asp:TextBox>
        <br /><br />
        
        <asp:Label ID="Label1" runat="server" Text="Old password" Width="150px" 
            Font-Bold="True" ForeColor="#996633"></asp:Label>
        <asp:TextBox ID="txt_opassword" runat="server" TextMode="Password"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txt_opassword" 
            ErrorMessage="*"
            ForeColor="red"></asp:RequiredFieldValidator>
        <br />
        <br />
         <asp:Label ID="Label2" runat="server" Text="New password" Width="150px" 
            Font-Bold="True" ForeColor="#996633"></asp:Label>
        <asp:TextBox ID="txt_npassword" runat="server" TextMode="Password"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txt_npassword" 
            ErrorMessage="*" 
            ForeColor="red"></asp:RequiredFieldValidator>
        <asp:Label ID="PasswordHint" runat="server" Text="at least 7 characters long, 
            containing number, alphabec and special character(@$!%*#?&)."></asp:Label>
        <br />
        <br />
        
         <asp:Label ID="Label3" runat="server" Text="Confirm password" Width="150px" 
            Font-Bold="True" ForeColor="#996633"></asp:Label>
        <asp:TextBox ID="txt_cpassword" runat="server" TextMode="Password"></asp:TextBox>   

        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="txt_cpassword" 
            ErrorMessage="*"
            ForeColor="red"></asp:RequiredFieldValidator>
        <br />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txt_npassword"            
            ErrorMessage="Invalid Password. Please try again."
            ForeColor="red"
            Display="Dynamic"
            ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{7,}$">
        </asp:RegularExpressionValidator>
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="txt_npassword"
            ControlToValidate="txt_cpassword" 
            ErrorMessage="The confirm password must match the new password entry"
            ForeColor="red"
            Display="Dynamic"></asp:CompareValidator>
        <asp:CustomValidator ID="CustomValidator1" runat="server"
            ErrorMessage="Old password and new password must be different.  Please try again."
            ControlToValidate="txt_npassword"
            ForeColor="red"
            Display="Dynamic" 
            OnServerValidate="ComparePwd"></asp:CustomValidator>
        
        <br />
        <asp:Label ID="lblError" Runat="server" ForeColor="Red" />
        <br />        
        <br />
        <asp:Button ID="btn_update" runat="server" Font-Bold="True" Text="Change Password" OnClick="ChangedPassword" />
        &nbsp;&nbsp;
        <asp:Button ID="btn_cancel" runat="server" Font-Bold="True" Text="Cancel" CausesValidation="False" OnClick="btn_cancel_Click" />
        <br />
        <br />
    </div>
    </form>
        </div>
</body>
</html>
