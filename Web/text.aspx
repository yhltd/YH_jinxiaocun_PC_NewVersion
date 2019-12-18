<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="text.aspx.cs" Inherits="Web.text" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>text 1</title>
</head>
<body>
   <%-- <form id="form1" runat="server">
    <div>
    
    </div>
    </form>--%>
    <div>
        
        <% if (Session["all"] != null)
           {
               List<string> all = Session["all"] as List<string>;
               for ( int i =0; i <all.Count; i++)
               {
               %><div class ="header"><%=all[i] %></div><%
               }
           }%>
        
        <%--<div class ="body">b</div>
        <div class ="fool">c</div>--%>
    </div>
</body>
</html>
