<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DatabaseManagement.aspx.cs" Inherits="DatabaseManagement_DatabaseManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Database Management</title>
    <link href="../StyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {}
        .auto-style2 {}
    </style>
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
            <br />
            <br />

        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label"></asp:Label>

        <asp:Table ID="Table1" runat="server" CellPadding="2" CellSpacing="0" Height="60px" Width="600px" BorderStyle="Solid" BorderWidth="1px">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">ID:<br />auto increament
                    
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">ServerName:<br />
                    <asp:DropDownList ID="txtServer" runat="server" DataSourceID="SqlDataSourceServer" DataTextField="ServerName" DataValueField="ServerName" Width="148" ></asp:DropDownList>
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">DBName:<br />
                    <asp:TextBox ID="txtDBName" runat="server" Width="148" />
                </asp:TableCell><asp:TableCell runat="server" Width="150" BorderStyle="Solid" BorderWidth="1">
                    <br /><asp:Button ID="btnAdd" runat="server" Text="Add New Database" OnClick="Insert" />
                </asp:TableCell></asp:TableRow></asp:Table><asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Width="600px" DataKeyNames="ID" DataSourceID="SqlDataSource1" OnRowDataBound="OnRowDataBound" EmptyDataText="There are no data records to display." ForeColor="Black" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" >
                    <ItemStyle Width="150px"></ItemStyle></asp:BoundField>
                    <asp:TemplateField HeaderText="ServerName" SortExpression="ServerName">
                        <EditItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceServer" DataTextField="ServerName" DataValueField="ServerName" Width="150px" SelectedValue='<%# Bind("ServerName") %>'>
                            </asp:DropDownList>                            
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ServerName") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:BoundField DataField="DBName" HeaderText="DBName" SortExpression="DBName" >
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowEditButton="True">
                        <ItemStyle Width="150px"></ItemStyle></asp:CommandField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="#003300" Font-Bold="True" ForeColor="#CCCC00" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingHeaderStyle BackColor="#006600" Font-Size="Larger" />
                <SortedDescendingHeaderStyle BackColor="#336600" Font-Size="Larger" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" DeleteCommand="DELETE FROM [Database] WHERE [ID] = @ID" InsertCommand="INSERT INTO [Database] ([ServerName], [DBName]) VALUES (@ServerName, @DBName)" SelectCommand="SELECT [ID], [ServerName], [DBName] FROM [Database]" UpdateCommand="UPDATE [Database] SET [ServerName] = @ServerName, [DBName] = @DBName WHERE [ID] = @ID">
                <DeleteParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:ControlParameter Name="ServerName" ControlID="txtServer" Type="String" />
                    <asp:ControlParameter Name="DBName" ControlID="txtDBName" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ServerName" Type="String" />
                    <asp:Parameter Name="DBName" Type="String" />
                    <asp:Parameter Name="ID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceServer" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT [ServerName] FROM [Server]"></asp:SqlDataSource>
        </div>
        <br /><br /><br />
        <b>Choose a Role:</b> <asp:DropDownList ID="ListRow" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceRole" DataTextField="Role" DataValueField="Role" CssClass="auto-style2" Width="140px" AppendDataBoundItems="True">
            <asp:ListItem Selected="True">--SELECT ROLE--</asp:ListItem></asp:DropDownList><asp:SqlDataSource ID="SqlDataSourceRole" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT [Role] FROM [Roles]"></asp:SqlDataSource>
        <br /><br /><br />
        <table>
            <tr>
                <td style="vertical-align: top;"><asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource2" OnRowDataBound="OnRowDataBound2" EmptyDataText="There are no data records to display." AllowPaging="True" AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" InsertVisible="False" Visible="False" /><asp:BoundField DataField="ServerName" HeaderText="ServerName" SortExpression="ServerName" /><asp:BoundField DataField="DBName" HeaderText="DBName" SortExpression="DBName" /><asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowSelectButton="True" /></Columns>
                    <FooterStyle BackColor="#CCCCCC" /><HeaderStyle BackColor="#003300" Font-Bold="True" ForeColor="#CCCC00" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#FF9966" Font-Bold="True" ForeColor="White" /><SortedAscendingHeaderStyle BackColor="#006600" Font-Size="Larger" /><SortedDescendingHeaderStyle BackColor="#336600" Font-Size="Larger" /></asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" 
                        DeleteCommand="DELETE FROM [Permission] WHERE [ID] = @ID" 
                        InsertCommand="INSERT INTO [Permission] ([ServerName], [DBName]) VALUES (@ServerName, @DBName)" 
                        SelectCommand="SELECT [ID], [ServerName], [DBName] FROM [Permission] WHERE ([Role] = @Role)" 
                        UpdateCommand="UPDATE [Permission] SET [ServerName] = @ServerName, [DBName] = @DBName WHERE [ID] = @ID">
                        <DeleteParameters>
                            <asp:Parameter Name="ID" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="ServerName" Type="String" /><asp:Parameter Name="DBName" Type="String" /></InsertParameters>
                        <SelectParameters><asp:ControlParameter ControlID="ListRow" Name="Role" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="ServerName" Type="String" /><asp:Parameter Name="DBName" Type="String" /><asp:Parameter Name="ID" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>

                </td>
                <td style="vertical-align: top;" >
                    <asp:DetailsView ID="DetailsView1" runat="server" 
                        AutoGenerateRows="False" BackColor="White" BorderColor="#999999" 
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                        DataKeyNames="ID" DataSourceID="SqlDataSourceDetails" 
                        ForeColor="Black" GridLines="Vertical" Height="50px" Width="181px" CssClass="auto-style1" 
                        HeaderText="Database Details:" 
                        onitemupdated="DetailsView_ItemUpdated" 
                        oniteminserted="DetailsView_ItemInserted">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <EditRowStyle BackColor="#FF6600" Font-Bold="False" ForeColor="Black" BorderStyle="Solid" /><Fields>
                            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                            <asp:TemplateField HeaderText="Role" SortExpression="Role">
                                <EditItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Role") %>'>
                                    </asp:Label></EditItemTemplate><InsertItemTemplate>
                                    <asp:DropDownList ID="DropDownList4" runat="server" DataSourceID="SqlDataSourceRole" Width="140px" DataTextField="Role" DataValueField="Role" SelectedValue='<%# Bind("Role") %>'>
                                    </asp:DropDownList>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Role") %>'>
                                    </asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="ServerName" SortExpression="ServerName">
                                <EditItemTemplate><asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSourceServer" Width="140px" DataTextField="ServerName" DataValueField="ServerName" SelectedValue='<%# Bind("ServerName") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="SqlDataSourceServer" Width="140px" DataTextField="ServerName" DataValueField="ServerName" SelectedValue='<%# Bind("ServerName") %>'>
                                    </asp:DropDownList>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ServerName") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:BoundField DataField="DBName" HeaderText="DBName" SortExpression="DBName" />
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                            <asp:CommandField ButtonType="Button" ShowEditButton="True" ShowInsertButton="True" /></Fields><FooterStyle BackColor="#003300" /><HeaderStyle BackColor="#003300" Font-Bold="True" ForeColor="#CCCC00" /><InsertRowStyle BackColor="#FF6600" BorderStyle="Solid" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />

                    </asp:DetailsView>
                    <asp:SqlDataSource ID="SqlDataSourceDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" 
                        SelectCommand="SELECT * FROM [Permission] WHERE ([ID] = @ID)" 
                        DeleteCommand="DELETE FROM [Permission] WHERE [ID] = @ID" 
                        InsertCommand="INSERT INTO [Permission] ([Role], [ServerName], [DBName], [Description]) VALUES (@Role, @ServerName, @DBName, @Description)" 
                        UpdateCommand="UPDATE [Permission] SET [Role] = @Role, [ServerName] = @ServerName, [DBName] = @DBName, [Description] = @Description WHERE [ID] = @ID">
                        <DeleteParameters><asp:Parameter Name="ID" Type="Int32" /></DeleteParameters>
                        <InsertParameters><asp:Parameter Name="Role" Type="String" />
                            <asp:Parameter Name="ServerName" Type="String" />
                            <asp:Parameter Name="DBName" Type="String" />
                            <asp:Parameter Name="Description" Type="String" />
                            </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="GridView2" Name="ID" PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                        <UpdateParameters><asp:Parameter Name="Role" Type="String" />
                            <asp:Parameter Name="ServerName" Type="String" />
                            <asp:Parameter Name="DBName" Type="String" />
                            <asp:Parameter Name="Description" Type="String" />
                            <asp:Parameter Name="ID" Type="Int32" />
                        </UpdateParameters>

                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>

    </form>
        </div>
</body>
</html>
