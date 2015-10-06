<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .modalBackground {
            background-color:#808080;
            filter:alpha(opacity=70);
            opacity:0.7;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:fixed; left:30%; top:30%">
    <p style="font-family: 'Times New Roman', Times, serif; font-size: x-large; text-transform: capitalize; color: #800080; text-align: center">MULTI-DATABASE QUERYING SYSTEM</p>
        <asp:Login ID="Login1" runat="server" style="margin:auto" BackColor="#FFD520" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Times New Roman" Font-Size="Large" Height="260px" OnAuthenticate="Login1_Authenticate" Width="450px"  InstructionText="Please enter your user name and password for login." ValidateRequestMode="Enabled">
            <CheckBoxStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <LoginButtonStyle BackColor="Silver" BorderStyle="Solid" BorderWidth="1px" Font-Names="Times New Roman" Font-Size="Large" ForeColor="#00543D" Font-Bold="True" />
            <TextBoxStyle Font-Size="0.8em" />
            <TitleTextStyle BackColor="#00543D" Font-Bold="True" Font-Size="X-Large" ForeColor="White" />
        </asp:Login>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" Text="*" ValidationGroup="Login1" ForeColor="Red"  />
    
    </div>
        <cc1:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        </asp:UpdatePanel>
        <asp:HiddenField ID="hiddenField" runat="server" />
        
        <cc1:ModalPopupExtender runat="server" ID="ProgressBarModalPopupExtender"
            BehaviorID="ProgressBarModalPopupExtender"
            TargetControlID="hiddenField"
            PopupControlID="Panel1"
            BackgroundCssClass="modalBackground"
            RepositionMode="RepositionOnWindowScroll" >
        </cc1:ModalPopupExtender>
        <br />
        <asp:Panel ID="Panel1" runat="server" Style="display: none; ">
            <img src="images/loader.gif" />
        </asp:Panel>
    </form>
</body>
</html>
