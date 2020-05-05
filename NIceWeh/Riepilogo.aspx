<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Riepilogo.aspx.cs" Inherits="NIceWeh.Riepilogo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%Response.Write("<span>"+new DateTime(anno,mese,1).ToString("MMMM yyyy")+"</span>"); %>
            <table>
                <thead>
                    <tr>
                        <% foreach (var dip in dipendenti)
                            {
                                Response.Write("<th>");
                                Response.Write("<th>" + dip.Nome + " " + dip.Cognome + "</th>");
                            }
                        %>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var cli in clienti)
                        {
                            Response.Write("<tr>");
                            Response.Write("<td>" + cli.Nome + "</td>");
                            foreach(var dip in dipendenti)
                            {
                                Response.Write("<td>" + GetOre(cli, dip) + "</td>"); 
                            }
                            Response.Write("</tr>");
                        }
                    %>
                </tbody>
            </table>

        </div>
    </form>
</body>
</html>
