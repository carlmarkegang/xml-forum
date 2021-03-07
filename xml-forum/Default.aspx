<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="xml_forum._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">



    <div class="row">
        <%foreach (System.Xml.XmlNode Post in Posts){
                Response.Write("<div>" + Post.InnerText + "</div>");
            }
        %>

    </div>

</asp:Content>
