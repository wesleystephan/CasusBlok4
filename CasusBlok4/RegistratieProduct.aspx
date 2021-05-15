<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistratieProduct.aspx.cs" Inherits="CasusBlok4.RegistratieProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CasusBlok4ConnectionString %>" SelectCommand="SELECT * FROM [Transacties]" DeleteCommand="DELETE FROM [Transacties] WHERE [TransactieID] = @TransactieID" InsertCommand="INSERT INTO [Transacties] ([Lidnummer], [Datum], [ArtikelID], [ArtikelAantal], [ArtikelPunten], [Serienummer], [Donatie], [Lening], [ArtikelNaam], [ArtikelSoort]) VALUES (@Lidnummer, @Datum, @ArtikelID, @ArtikelAantal, @ArtikelPunten, @Serienummer, @Donatie, @Lening, @ArtikelNaam, @ArtikelSoort)" UpdateCommand="UPDATE [Transacties] SET [Lidnummer] = @Lidnummer, [Datum] = @Datum, [ArtikelID] = @ArtikelID, [ArtikelAantal] = @ArtikelAantal, [ArtikelPunten] = @ArtikelPunten, [Serienummer] = @Serienummer, [Donatie] = @Donatie, [Lening] = @Lening, [ArtikelNaam] = @ArtikelNaam, [ArtikelSoort] = @ArtikelSoort WHERE [TransactieID] = @TransactieID">
        <DeleteParameters>
            <asp:Parameter Name="TransactieID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Lidnummer" Type="Int32" />
            <asp:Parameter Name="Datum" Type="DateTime" />
            <asp:Parameter Name="ArtikelID" Type="Int32" />
            <asp:Parameter Name="ArtikelAantal" Type="Int32" />
            <asp:Parameter Name="ArtikelPunten" Type="Int32" />
            <asp:Parameter Name="Serienummer" Type="String" />
            <asp:Parameter Name="Donatie" Type="Boolean" />
            <asp:Parameter Name="Lening" Type="Boolean" />
            <asp:Parameter Name="ArtikelNaam" Type="String" />
            <asp:Parameter Name="ArtikelSoort" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Lidnummer" Type="Int32" />
            <asp:Parameter Name="Datum" Type="DateTime" />
            <asp:Parameter Name="ArtikelID" Type="Int32" />
            <asp:Parameter Name="ArtikelAantal" Type="Int32" />
            <asp:Parameter Name="ArtikelPunten" Type="Int32" />
            <asp:Parameter Name="Serienummer" Type="String" />
            <asp:Parameter Name="Donatie" Type="Boolean" />
            <asp:Parameter Name="Lening" Type="Boolean" />
            <asp:Parameter Name="ArtikelNaam" Type="String" />
            <asp:Parameter Name="ArtikelSoort" Type="String" />
            <asp:Parameter Name="TransactieID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
<asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="TransactieID">
    <Columns>
        <asp:BoundField DataField="Lidnummer" HeaderText="Lidnummer" SortExpression="Lidnummer" />
        <asp:BoundField DataField="Datum" HeaderText="Datum" SortExpression="Datum" />
        <asp:BoundField DataField="ArtikelID" HeaderText="ArtikelID" SortExpression="ArtikelID" />
        <asp:BoundField DataField="ArtikelAantal" HeaderText="ArtikelAantal" SortExpression="ArtikelAantal" />
        <asp:BoundField DataField="ArtikelPunten" HeaderText="ArtikelPunten" SortExpression="ArtikelPunten" />
        <asp:BoundField DataField="Serienummer" HeaderText="Serienummer" SortExpression="Serienummer" />
        <asp:CheckBoxField DataField="Donatie" HeaderText="Donatie" SortExpression="Donatie" />
        <asp:CheckBoxField DataField="Lening" HeaderText="Lening" SortExpression="Lening" />
        <asp:BoundField DataField="ArtikelNaam" HeaderText="ArtikelNaam" SortExpression="ArtikelNaam" />
        <asp:BoundField DataField="ArtikelSoort" HeaderText="ArtikelSoort" SortExpression="ArtikelSoort" />
        <asp:BoundField DataField="TransactieID" HeaderText="TransactieID" InsertVisible="False" ReadOnly="True" SortExpression="TransactieID" />
    </Columns>
    </asp:GridView>









    <br />
    <br />

    <br />
    <br />

    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:CasusBlok4ConnectionString %>" SelectCommand="SELECT * FROM [Artikelen] WHERE ([ArtikelSoortID] = @ArtikelSoortID)">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownList2" Name="ArtikelSoortID" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <br />
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:CasusBlok4ConnectionString %>" SelectCommand="SELECT * FROM [ArtikelSoorten]"></asp:SqlDataSource>








                Artikel soort:
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="ArtikelSoortNaam" DataValueField="ArtikelSoortID">
            </asp:DropDownList>
    <br />
            Artikel Naam:
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource3" DataTextField="ArtikelNaam" DataValueField="ArtikelID">
            </asp:DropDownList>
        <br />
    <asp:FormView ID="FormView1" runat="server" AllowPaging="True" DataKeyNames="TransactieID" DataSourceID="SqlDataSource1" DefaultMode="Insert">
        <EditItemTemplate>
            Lidnummer:
            <asp:TextBox ID="LidnummerTextBox" runat="server" Text='<%# Bind("Lidnummer") %>' />
            <br />
            Datum:
            <asp:TextBox ID="DatumTextBox" runat="server" Text='<%# Bind("Datum") %>' />
            <br />
            ArtikelID:
            <asp:TextBox ID="ArtikelIDTextBox" runat="server" Text='<%# Bind("ArtikelID") %>' />
            <br />
            ArtikelAantal:
            <asp:TextBox ID="ArtikelAantalTextBox" runat="server" Text='<%# Bind("ArtikelAantal") %>' />
            <br />
            ArtikelPunten:
            <asp:TextBox ID="ArtikelPuntenTextBox" runat="server" Text='<%# Bind("ArtikelPunten") %>' />
            <br />
            Serienummer:
            <asp:TextBox ID="SerienummerTextBox" runat="server" Text='<%# Bind("Serienummer") %>' />
            <br />
            Donatie:
            <asp:CheckBox ID="DonatieCheckBox" runat="server" Checked='<%# Bind("Donatie") %>' />
            <br />
            Lening:
            <asp:CheckBox ID="LeningCheckBox" runat="server" Checked='<%# Bind("Lening") %>' />
            <br />
            ArtikelNaam:
            <asp:TextBox ID="ArtikelNaamTextBox" runat="server" Text='<%# Bind("ArtikelNaam") %>' />
            <br />
            ArtikelSoort:
            <asp:TextBox ID="ArtikelSoortTextBox" runat="server" Text='<%# Bind("ArtikelSoort") %>' />
            <br />
            TransactieID:
            <asp:Label ID="TransactieIDLabel1" runat="server" Text='<%# Eval("TransactieID") %>' />
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </EditItemTemplate>
        <InsertItemTemplate>
            Lidnummer:
            <asp:TextBox ID="LidnummerTextBox" runat="server" Text='<%# Bind("Lidnummer") %>' />
            <br />
            Datum:
            <asp:TextBox ID="DatumTextBox" runat="server" Text='<%# Bind("Datum") %>' />
            <br />
                                        Artikel soort:
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="ArtikelSoortNaam" DataValueField="ArtikelSoortID">
            </asp:DropDownList>
            <br />
                        Artikel Naam:
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource3" DataTextField="ArtikelNaam" DataValueField="ArtikelID">
            </asp:DropDownList>
            <br />
            ArtikelID:
            <asp:TextBox ID="ArtikelIDTextBox" runat="server" Text='<%# Bind("ArtikelID") %>' />
            <br />
            ArtikelAantal:
            <asp:TextBox ID="ArtikelAantalTextBox" runat="server" Text='<%# Bind("ArtikelAantal") %>' />
            <br />
            ArtikelPunten:
            <asp:TextBox ID="ArtikelPuntenTextBox" runat="server" Text='<%# Bind("ArtikelPunten") %>' />
            <br />
            Serienummer:
            <asp:TextBox ID="SerienummerTextBox" runat="server" Text='<%# Bind("Serienummer") %>' />
            <br />
            Donatie:
            <asp:CheckBox ID="DonatieCheckBox" runat="server" Checked='<%# Bind("Donatie") %>' />
            <br />
            Lening:
            <asp:CheckBox ID="LeningCheckBox" runat="server" Checked='<%# Bind("Lening") %>' />
            <br />
            ArtikelNaam:
            <asp:TextBox ID="ArtikelNaamTextBox" runat="server" Text='<%# Bind("ArtikelNaam") %>' />
            <br />
            ArtikelSoort:
            <asp:TextBox ID="ArtikelSoortTextBox" runat="server" Text='<%# Bind("ArtikelSoort") %>' />
            <br />

            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </InsertItemTemplate>
        <ItemTemplate>
            Lidnummer:
            <asp:Label ID="LidnummerLabel" runat="server" Text='<%# Bind("Lidnummer") %>' />
            <br />
            Datum:
            <asp:Label ID="DatumLabel" runat="server" Text='<%# Bind("Datum") %>' />
            <br />
            ArtikelID:
            <asp:Label ID="ArtikelIDLabel" runat="server" Text='<%# Bind("ArtikelID") %>' />
            <br />
            ArtikelAantal:
            <asp:Label ID="ArtikelAantalLabel" runat="server" Text='<%# Bind("ArtikelAantal") %>' />
            <br />
            ArtikelPunten:
            <asp:Label ID="ArtikelPuntenLabel" runat="server" Text='<%# Bind("ArtikelPunten") %>' />
            <br />
            Serienummer:
            <asp:Label ID="SerienummerLabel" runat="server" Text='<%# Bind("Serienummer") %>' />
            <br />
            Donatie:
            <asp:CheckBox ID="DonatieCheckBox" runat="server" Checked='<%# Bind("Donatie") %>' Enabled="false" />
            <br />
            Lening:
            <asp:CheckBox ID="LeningCheckBox" runat="server" Checked='<%# Bind("Lening") %>' Enabled="false" />
            <br />
            ArtikelNaam:
            <asp:Label ID="ArtikelNaamLabel" runat="server" Text='<%# Bind("ArtikelNaam") %>' />
            <br />
            ArtikelSoort:
            <asp:Label ID="ArtikelSoortLabel" runat="server" Text='<%# Bind("ArtikelSoort") %>' />
            <br />
            TransactieID:
            <asp:Label ID="TransactieIDLabel" runat="server" Text='<%# Eval("TransactieID") %>' />
            <br />

            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
            &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New" Text="New" />

        </ItemTemplate>
    </asp:FormView>
</asp:Content>

