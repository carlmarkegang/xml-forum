<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="xml_forum._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">



    <div class="row">
        <%foreach (System.Xml.XmlNodeList Posts in PostsList)
            {
                foreach (System.Xml.XmlNode Post in Posts)
                {
                    Response.Write("<div>" + Post.InnerText + "</div>");
                }
            }
        %>
    </div>
    <input type="text" name="text" />
    <input type="submit" />

</asp:Content>
