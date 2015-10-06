<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManage.aspx.cs" Inherits="UserManage_UserManage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script type="text/javascript">
            function StartProgressBar() {
                var myExtender = $find('ProgressBarModalPopupExtender');
                myExtender.show();
                return true;
            }

    </script>

    <style>
.modalBackground {
background-color:#808080;
filter:alpha(opacity=70);
opacity:0.7;

}

.modalPopup {
background-color:#ffffdd;
border-width:3px;
border-style:solid;
border-color:Gray;
padding:3px;
width:250px;
}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:LoginView ID="LoginView1" Runat="server" Visible="true">
            <LoggedInTemplate>
            <asp:LoginName ID="LoginName1" runat="server" Font-Bold = "true" FormatString="Welcome {0}!"/><br />
            </LoggedInTemplate>
            <AnonymousTemplate>
                You are not logged in
            </AnonymousTemplate>
        </asp:LoginView><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LoginStatus ID="LoginStatus1" runat="server" />
        <asp:HyperLink ID="AdminPg" runat="server" NavigateUrl="~/Admin/Admin.aspx" Visible="False">Go To Admin Page</asp:HyperLink>
        <br />
        <a href="../Default.aspx">SQL Practice</a>

        <asp:ChangePassword ID="ChangePassword1" runat="server"
            CancelDestinationPageUrl="~/Default.aspx"
            OnChangingPassword = "ChangingPassword"
            OnChangedPassword = "ChangedPassword"
            PasswordHintText = 
            "Please enter a password at least 7 characters long, 
            containing a number and one special character." 
            NewPasswordRegularExpressionErrorMessage =
            "" DisplayUserName="True" Height="187px" PasswordLabelText="Old Password:" style="text-align: left" Width="614px" OnChangePasswordError="ChangedPassword" >
        </asp:ChangePassword><br />
        <asp:Label ID="lblError" Runat="server" ForeColor="Red" />
        <asp:Label ID="Message1" runat="server" Text="Label"></asp:Label>
        <br />
    </div>
        
<%--        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Button ID="btnSubmit" onclick="btnSubmit_Click" OnClientClick="StartProgressBar()"
                     runat="server" Text="Submit Time" Width="170px" />
                    <cc1:ModalPopupExtender ID="ProgressBarModalPopupExtender" runat="server"
                     BackgroundCssClass="ModalBackground" behaviorID="ProgressBarModalPopupExtender"
                     TargetControlID="hiddenField" PopupControlID="Panel1" />
                    <asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: #C0C0C0;">
                        <img src="../images/ajax_loader.gif" alt="Loading..."/>
                    </asp:Panel>
                    <asp:HiddenField ID="hiddenField" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
 
 <cc1:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1" />
    <script type="text/javascript">
        function showPopup() {
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }
        //function hidepopup() {
        //    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        //    modalPopupBehavior.hide();
        //}
    </script>

    <asp:Button ID="loginButton" runat="server" Text="login"  OnClientClick="showPopup()"
        onclick="loginButton_Click" /><br />
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              Put the control which needs to update here, such as GridView.    

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="loginButton"  EventName="Click"/>
        </Triggers>
    </asp:UpdatePanel>
        <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" style="display:none"/>
        <cc1:ModalPopupExtender runat="server" ID="programmaticModalPopup"
            BehaviorID="programmaticModalPopupBehavior"
            TargetControlID="hiddenTargetControlForModalPopup"
            PopupControlID="programmaticPopup"
            BackgroundCssClass="modalBackground"
            DropShadow="True"
            RepositionMode="RepositionOnWindowScroll" >
        </cc1:ModalPopupExtender>
        <asp:Panel ID="programmaticPopup" runat="server" Style="display: none; background-color: #C0C0C0;">
                        <img src="../images/ajax_loader.gif" alt="Loading..."/>
                    </asp:Panel>
        <%--<asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopup" style="background-color:##FFFFCC;display:none;height:25px;width:85px;padding:10px">
        <div id='messagediv' style="text-align:center">Loading...</div>

        </asp:Panel>--%>
        <p>
            If you have any questions or encounter any problems logging in,
            please contact a site administrator.
        </p>
    </form>


</body>
</html>
