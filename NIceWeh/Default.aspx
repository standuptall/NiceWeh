<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NIceWeh._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container" style="width:100%;">
            <div class="col-md-2" id="fltr" >
                <span class="btn glyphicon glyphicon-search btn-lg" aria-hidden="true" title="Riepilogo per cliente"
                                onclick="openRiepilogo()" ></span>
                <span class="btn glyphicon glyphicon-list-alt btn-lg" aria-hidden="true" title="Excel con ore di lavoro" 
                                onclick="openExcel()" ></span>
                <div class="row">
                    <label for ="Employee">Dipendente</label>
                </div>
                <div class="row">
                    <asp:DropDownList 
                        CssClass="combo-flat"  
                        ID="Employee" runat="server" 
                         OnSelectedIndexChanged ="Employee_SelectedIndexChanged"
                        AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="row">
                    <asp:Calendar 
                         OnSelectionChanged="Calendar1_SelectionChanged"
                        ID="Calendar1" 
                         CssClass = "calendar" 
                         TitleStyle-CssClass ="calendar-header"
                         TodayDayStyle-BackColor =  "Green"
                         NextMonthText="<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>"
                         PrevMonthText ="<span class='glyphicon glyphicon-arrow-left' aria-hidden='true'></span>"
                        DayNameFormat ="FirstLetter"
                        
                        runat="server" >
                    </asp:Calendar>
                </div>
                <div class="row">
                    <label for ="Customer">Cliente</label>
                </div>
                <div class="row">
                    <asp:DropDownList
                        ID="Customer" 
                         CssClass ="input"
                        runat="server"></asp:DropDownList>
                </div>
                <%--hidden ----->--%>
                <div class="row">
                    <asp:TextBox ID="IdReport"  runat="server" style="display:none;"></asp:TextBox>
                </div>
                <div class="row">
                    <asp:TextBox ID="Datehidden"  runat="server" style="display:none;"></asp:TextBox>
                </div>
                <div class="row">
                    <asp:TextBox ID="ConfirmDelete"  runat="server" style="display:none;"></asp:TextBox>
                </div>
                <%--<-------------hidden--%>
                <% if ((string)ViewState["mandatory"] == "true")
{
    Response.Write("<div class=\"alert alert-danger\">Compilare correttamente tutti i campi!</div>");
}
                    %>
                <div class="row">
                    <label for ="Activity">Attività</label>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <asp:DropDownList CssClass="input" ID="Activity" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox ID ="Hours" runat="server" type="number" AutoPostBack="false"  CssClass="input"  />
                    </div>
                </div>

                <div class="row">
                    <label for ="Customer">Note</label>
                </div>
                <div class="row">
                    <asp:TextBox ID="Notes" runat="server" TextMode="MultiLine"  Rows="5"></asp:TextBox>
                </div>
                <div class="row">
                    <label for ="ReportNotes">Note Rapporto</label>
                </div>
                <div class="row">
                    <asp:TextBox ID="ReportNotes" runat="server" Rows="1"></asp:TextBox>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-6">
                        <asp:Button OnClick="ButtonSalva_Click"
                            CssClass="btn btn-primary" ID="ButtonSalva" runat="server" Text="Salva"
                             />
                    </div>
                    <div class="col-md-6">
                        <asp:Button CssClass="btn btn-primary" ID="ButtonElimina" 
                             OnClientClick="Confirm()"
                            OnClick="ButtonElimina_Click"  runat="server" Text="Elimina" />
                    </div>
                </div>
            </div>
            <div class="col-md-10" id="dvRprt">
                <asp:Table runat="server" 
                    CssClass="report-table"
                   
                    ID="ReptTable" >
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="..." />
                        <asp:TableHeaderCell  
                            Scope="Column" 
                            Text="Data" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Attività" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Cliente" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Ore" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Note" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Note Rapporto" />
                        <asp:TableHeaderCell 
                            Scope="Column" 
                            Text="Tot" />
                    </asp:TableHeaderRow>
                    
                </asp:Table>
            </div>
          
        </div>
    <script type="text/javascript">
        var selectedrow = null;
        $(".report-table tr").click(
            function () {
                if (selectedrow) {
                    $(selectedrow).removeClass("selected");
                    BlankItAll();
                }
                selectedrow = $(this);
                $(this).addClass("selected");
                let current_datetime = new Date()
                //let anno = $('#<%= Calendar1.ClientID %>').val().substr(0, 4);
                let mese = $(selectedrow).find('td')[1].innerHTML.substr(3, 2);
                let anno = "<%= ViewState["year"] %>";
                let giorno = $(selectedrow).find('td')[1].innerHTML.substr(0, 2);
                $('#<%= Datehidden.ClientID %>').val(anno+"/"+mese+"/"+giorno);
                $(selectedrow).find('td span').click();
            });
        function Edit(json) {
            event.stopPropagation();
            var rapp = JSON.parse(json);
            $('#<%= IdReport.ClientID %>').val(rapp.Id);
            $('#<%= Notes.ClientID %>').val(rapp.Notes);
            $('#<%= ReportNotes.ClientID %>').val(rapp.ReportNotes);
            $('#<%= Activity.ClientID %>').val(rapp.ActivityId);
            $('#<%= Hours.ClientID %>').val(rapp.Hours);
            $('#<%= Customer.ClientID %>').val(rapp.CustomerId);
            $('#<%= Datehidden.ClientID %>').val(rapp.Date);
        }
        function Confirm() {
            var val = "false";
            if (confirm("Sei sicuro di voler eliminare il rapporto selezionato?"))
                val = "true";
            $('#<%= ConfirmDelete.ClientID %>').val(val);
        }
        function BlankItAll() {
            $('#<%= IdReport.ClientID %>').val("0");
            $('#<%= Notes.ClientID %>').val("");
            $('#<%= ReportNotes.ClientID %>').val("");
            $('#<%= Activity.ClientID %>').val("0");
            $('#<%= Hours.ClientID %>').val("0");
            $('#<%= Customer.ClientID %>').val("0");
            $('#<%= Datehidden.ClientID %>').val("");
        }
        function openRiepilogo () {

            let mese = "<%= ViewState["month"] %>";
            let anno = "<%= ViewState["year"] %>";
            window.open('Riepilogo.aspx?y=' + anno + '&m='+mese, '_blank');
        }
        function openExcel() {
            let mese = "<%= ViewState["month"] %>";
            let anno = "<%= ViewState["year"] %>";
            window.open('ReportExcel.aspx?e=AZ&y=' + anno + '&m=' + mese, '_blank');
        }
    </script>
</asp:Content>