<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Management</title>
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
    </div>

    <div>           
        <br /><br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label"></asp:Label>

        <asp:Table ID="Table1" runat="server" CellPadding="2" CellSpacing="0" Height="60px" Width="900px" BorderStyle="Solid" BorderWidth="1px">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">UserID:<br />
                    <asp:TextBox ID="txtUserID" runat="server" Width="148" />
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">UserName(nullable):<br />
                    <asp:TextBox ID="txtUserName" runat="server" Width="148" />
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">Password:<br />
                    <asp:TextBox ID="txtPassword" runat="server" Width="148" />
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">Role:<br />
                    <asp:DropDownList ID="txtRole" runat="server" DataSourceID="SqlDataSourceRole" DataTextField="Role" DataValueField="Role" Width="148">
                    </asp:DropDownList>
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">LastLoginDate(nullable):<br />
                    <asp:TextBox ID="txtLastLoginDate" runat="server" Width="148" />
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">
                    <br /><asp:Button ID="btnAdd" runat="server" Text="Add New User" OnClick="Insert" />
                </asp:TableCell></asp:TableRow></asp:Table><asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="3" DataKeyNames="UserID" DataSourceID="SqlDataSource1" OnRowDataBound="OnRowDataBound" EmptyDataText="There are no data records to display." ForeColor="Black" GridLines="Vertical" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="UserID" ReadOnly="True" SortExpression="UserID" >
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" >
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Password" DataFormatString="***" HeaderText="Password" SortExpression="Password" >
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Role" SortExpression="Role">
                    <EditItemTemplate><asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceRole" DataTextField="Role" DataValueField="Role" AutoPostBack="True" Width="150px" SelectedValue='<%# Bind("Role") %>'></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Role") %>'></asp:Label></ItemTemplate><ItemStyle Width="150px" />
                    </asp:TemplateField>
                <asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" SortExpression="LastLoginDate" ReadOnly="True" >
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>
                <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowEditButton="True" ItemStyle-Width="150">
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:CommandField>
            </Columns>
            <FooterStyle BackColor="#003300" />
            <HeaderStyle BackColor="#003300" Font-Bold="True" ForeColor="#FFD520" HorizontalAlign="Left" />
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle BackColor="#003300" Font-Bold="True" Font-Overline="False" Font-Strikeout="False" ForeColor="White" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingHeaderStyle BackColor="#006600" Font-Bold="True" Font-Overline="False" Font-Size="Larger" /><SortedDescendingHeaderStyle BackColor="#336600" Font-Bold="True" Font-Size="Larger" /></asp:GridView>
        <br />
        <br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" 
            DeleteCommand="DELETE FROM [Users] WHERE [UserID] = @UserID" 
            InsertCommand="INSERT INTO [Users] ([UserID], [UserName], [Password], [Role], [LastLoginDate]) VALUES (@UserID, @UserName, @Password, @Role, @LastLoginDate)" 
            SelectCommand="SELECT [UserID], [UserName], [Password], [Role], [LastLoginDate] FROM [Users]" 
            UpdateCommand="UPDATE [Users] SET [UserName] = @UserName, [Password] = @Password, [Role] = @Role, [LastLoginDate] = @LastLoginDate WHERE [UserID] = @UserID"><DeleteParameters>
                <asp:Parameter Name="UserID" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:ControlParameter Name="UserID" ControlID="txtUserID" Type="String" />
                <asp:ControlParameter Name="UserName" ControlID="txtUserName" Type="String" />
                <asp:ControlParameter Name="Password" ControlID="txtPassword" Type="String" />
                <asp:ControlParameter Name="Role" ControlID="txtRole" Type="String" />
                <asp:controlParameter Name="LastLoginDate" ControlID="txtLastLoginDate" Type="DateTime" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="UserName" Type="String" />
                <asp:Parameter Name="Password" Type="String" />
                <asp:Parameter Name="Role" Type="String" />
                <asp:Parameter Name="LastLoginDate" Type="DateTime" />
                <asp:Parameter Name="UserID" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceRole" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT [Role] FROM [Roles]"></asp:SqlDataSource><br />
           
    </div>
    </form>
        </div>
</body>
</html>
