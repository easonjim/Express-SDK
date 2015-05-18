<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="kuaidi.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    快递速查API<br />
    快递单号：<asp:TextBox ID="txt_Number" runat="server"></asp:TextBox>&nbsp;&nbsp;
    返回类型：
    <asp:DropDownList ID="ddl_Type" runat="server">
        <asp:ListItem Selected="True" Text="HTML" Value="html"></asp:ListItem>
        <asp:ListItem Text="JSON" Value="json"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Button ID="btn_Call" runat="server" Text="调用" OnClick="btn_Call_Click" />
    <br/>
    <br/>
    快递名称速查API<br />
    快递拼音名称：<asp:TextBox ID="txt_name" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btn_Call1" runat="server" Text="调用" OnClick="btn_Call1_Click" />
    </div>
    </form>
</body>
</html>
