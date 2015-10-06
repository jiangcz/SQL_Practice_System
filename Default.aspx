<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SQL Practice</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <script type="text/javascript">
        function showPopup() {
            var modalPopupBehavior = $find('ProgressBarModalPopupExtender');
            modalPopupBehavior.show();
        }
    </script>
    <style type="text/css">
        #TextArea1 {
            height: 54px;
            width: 297px;
        }
        .auto-style1 {
            width: 50%;
        }
        .auto-style2 {
            width: 161px;
            text-align: left;
        }
        .auto-style3 {
            width: 196px;
            text-align: left;
        }
        .auto-style4 {
            width: 161px;
            text-align: left;
            height: 30px;
        }
        .auto-style5 {
            width: 196px;
            text-align: left;
            height: 30px;
        }

        .modalBackground {
            background-color:#808080;
            filter:alpha(opacity=70);
            opacity:0.7;}
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
        <br /><br />
        <asp:SqlDataSource ID="ChooseServer" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT DISTINCT p.ServerName FROM Users AS u LEFT OUTER JOIN Permission AS p ON u.Role = p.Role WHERE (u.UserID = @UserID)" >
            <SelectParameters>
                <asp:SessionParameter DefaultValue="" Name="UserID" SessionField="LoginID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <div runat="server" id="imagediv" >
            <asp:Image ID="Image1" runat="server" style="display:block; " /> 
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style4">
        <asp:Label ID="server" runat="server" Text="Choose Server: " Font-Bold="True" ForeColor="#996633"></asp:Label>
                </td>
                <td class="auto-style5">
        <asp:DropDownList ID="DropDownList1" AutoPostBack="true" runat="server" DataSourceID="ChooseServer" DataTextField="ServerName" DataValueField="ServerName" Width="150px" Height="20px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" style="margin-left: 0px" AppendDataBoundItems="True">
            <asp:ListItem Selected="True">--SELECT SERVER--</asp:ListItem>
        </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td class="auto-style2">
        <asp:Label ID="ChooseDB" runat="server" Text="Choose Database: " Font-Bold="True" ForeColor="#996633"></asp:Label>
                </td>
                <td class="auto-style3">
        <asp:DropDownList ID="DropDownList2" runat="server" Height="18px" Width="150px" DataSourceID="DataBase" DataTextField="DBName" DataValueField="DBName" style=" margin-left: 0px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
        </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        <asp:SqlDataSource ID="DataBase" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT DISTINCT p.DBName FROM Users AS u LEFT OUTER JOIN Permission AS p ON u.Role = p.Role WHERE (u.UserID = @UserID) AND (p.ServerName = @ServerName)">
            <SelectParameters>
                <asp:SessionParameter Name="UserID" SessionField="LoginID" />
                <asp:ControlParameter ControlID="DropDownList1" Name="ServerName" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="Role" runat="server" ConnectionString="<%$ ConnectionStrings:ManagementConnectionString %>" SelectCommand="SELECT [Role] FROM [Users] WHERE ([UserID] = @UserID)">
            <SelectParameters>
                <asp:SessionParameter Name="UserID" SessionField="LoginID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label ID="output" runat="server" Text="Output: "></asp:Label>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Show Diagram" Width="135px" Font-Bold="True"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button3" title="Restore the database back to its original content" runat="server" OnClick="Button3_Click" OnClientClick="showPopup()" Text="Create/Reset DB" Width="134px" Font-Bold="True"/>
        <br /><br />   
        
        <asp:TextBox ID="TextBox1" runat="server" Height="200px" Width="490px" font-size="18px" Wrap="True" TextMode="MultiLine"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" OnClientClick="showPopup()" Text="Submit Query" Font-Bold="True"/>
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="commit" runat="server" OnClick="commit_Click" Text="Save Result"  Visible="false" Enabled="False" Font-Bold="True"/>
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="rollback" runat="server" OnClick="rollback_Click" Text="Undo" Visible="false" Enabled="False" Font-Bold="True"/>
        <br />
            <div id="result" style="width: 500px; height: 400px; overflow: scroll">
                <asp:label id="msg" runat="server" forecolor="Red" />
                <asp:GridView ID="GridView1" runat="server" Height="400px" Width="480px" style="overflow:scroll" emptydatatext="No data">
                    <HeaderStyle BackColor="#339933" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#FFFF99" />
                </asp:GridView>
            </div>
        <br />
             <!--end of div content-->
        
        <br />
        <br />

        <cc1:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button1"  EventName="Click"/>
        </Triggers>--%>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hiddenField" runat="server" />
        <cc1:ModalPopupExtender runat="server" ID="ProgressBarModalPopupExtender"
            BehaviorID="ProgressBarModalPopupExtender"
            TargetControlID="hiddenField"
            PopupControlID="Panel1"
            BackgroundCssClass="modalBackground"
            RepositionMode="RepositionOnWindowScroll" >
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" Style="display: none; ">
            <img src="images/loader.gif" />
        </asp:Panel>
    </form>
        </div>
    </body>
</html>
