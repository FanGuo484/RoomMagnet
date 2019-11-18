﻿<%@ Page Title="" Language="C#" MasterPageFile="~/RoomMagnet.master" AutoEventWireup="true" CodeFile="Message.aspx.cs" Inherits="WebPages_Message" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="Server">
        <table>
            <tr>
                <td colspan="2">
                    <asp:image id="Image1" runat="server" imageurl="~/img/roommagnet-text.png" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px; text-align: right"></td>
                
                <td style="width: 94px; text-align: center">
                <asp:label id="Label1" runat="server" text="You are sending message to : UserName"></asp:label>
            </tr>
            <tr>
                <td style="width: 100px; height: 260px"></td>
                <td style="width: 94px; height: 260px">
                    <asp:textbox id="txtmsg" runat="server" height="250px" textmode="MultiLine" width="472px" ReadOnly="true"></asp:textbox>
                </td>
                
            </tr>
            <tr>
                <td style="width: 100px; height: 77px;"></td>
                <td style="width: 94px; height: 77px;">
                    <table style="width: 480px">
                        <tr>
                            <td style="width: 100px; height: 50px;">
                                <asp:textbox id="txtsend" runat="server" height="40px" textmode="MultiLine" width="384px"></asp:textbox>
                            </td>
                            <td style="width: 100px; height: 50px;">
                                <asp:button id="Button1" runat="server" height="47px" onclick="Button1_Click" text="Send"
                                    width="72px" font-bold="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:button ID="BN" runat="server" text="Clear" onclick="Clear_Click"/>
                </td>
            </tr>
        </table>
    <asp:TextBox ID="Messages" TextMode="MultiLine" runat="server" Height="134px" Width="538px"></asp:TextBox>
    <asp:DropDownList ID="RenterNames" runat="server">
        <asp:ListItem Value="1">James Bond</asp:ListItem>
                </asp:DropDownList>
</asp:Content>