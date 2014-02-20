<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kontakter.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/app.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="Scripts/app.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <h1><a href="Default.aspx">Äventyrliga kontakter</a></h1>

            <div>
                <asp:ValidationSummary
                    ID="errorList"
                    CssClass="alert"
                    DisplayMode="BulletList"
                    EnableClientScript="true"
                    HeaderText="Fel inträffade. Åtgärda felen och försök igen."
                    runat="server" />
                <asp:ValidationSummary
                    ValidationGroup="EditValidationGroup"
                    ID="errorListEdit"
                    CssClass="alert"
                    DisplayMode="BulletList"
                    EnableClientScript="true"
                    HeaderText="Fel inträffade. Åtgärda felen och försök igen."
                    runat="server" />
            </div>

            <asp:Panel ID="PanelSuccess" runat="server" Visible="false">
                <asp:Label ID="LabelSuccess" class="alert" runat="server" Text="">
                    <a class="close" href="#">&times;</a>
                </asp:Label>
            </asp:Panel>

            <asp:Button ID="ButtonNewContact" runat="server" Text="Ny kontakt" OnClick="ButtonNewContact_Click" />
            <asp:ListView ID="ContactListView" runat="server"
                ItemType="Kontakter.Model.Contact"
                SelectMethod="ContactListView_GetDataPageWise"
                InsertMethod="ContactListView_InsertItem"
                UpdateMethod="ContactListView_UpdateItem"
                DeleteMethod="ContactListView_DeleteItem"
                OnPagePropertiesChanging="PagePropertiesChanging"
                DataKeyNames="ContactId"
                InsertItemPosition="FirstItem"
                ViewStateMode="Enabled">
                <LayoutTemplate>
                    <table>
                        <tr>
                            <th>Förnamn
                            </th>
                            <th>Efternamn
                            </th>
                            <th>E-post
                            </th>
                            <th></th>
                            <th></th>
                        </tr>
                        <%-- Placeholder for new rows --%>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </table>
                    <%-- Paging --%>
                    <asp:DataPager ID="ContactsDataPager" runat="server" PagedControlID="ContactListView" PageSize="20" QueryStringField="page">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True"
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <b>Visar
                                        <asp:Label runat="server" ID="CurrentPageLabel"
                                            Text="<%# Container.StartRowIndex %>" />
                                        till
                                        <asp:Label runat="server" ID="TotalPagesLabel"
                                            Text="<%# Container.StartRowIndex+Container.PageSize %>" />
                                        (av
                                        <asp:Label runat="server" ID="TotalItemsLabel"
                                            Text="<%# Container.TotalRowCount %>" />
                                        kontakter)
                                        <br />
                                    </b>
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                        </Fields>
                    </asp:DataPager>
                </LayoutTemplate>
                <ItemTemplate>
                    <%-- Template for new rows. --%>
                    <tr>
                        <td>
                            <asp:Label ID="FirstNameLabel" runat="server" Text='<%#: Item.FirstName %>' />
                        </td>
                        <td>
                            <asp:Label ID="LastNameLabel" runat="server" Text='<%#: Item.LastName %>' />
                        </td>
                        <td>
                            <asp:Label ID="EmailAddressLabel" runat="server" Text='<%#: Item.EmailAddress %>' />
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButtonEdit1" runat="server" CommandName="Edit" Text="Redigera" CausesValidation="false" />
                        </td>
                        <td>
                            <span onclick="return confirm('Ta bort kontakten permanent?')">
                                <asp:LinkButton ID="LinkButtonDelete1" runat="server" CommandName="Delete" Text="Ta bort" CausesValidation="false" />
                            </span>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <%-- This appears when the contact information is missing in the database. --%>
                    <p>
                        Kontaktuppgifter saknas.
                    </p>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <%-- Template row in the table to add the new contact information. --%>
                    <tr>
                        <td>
                            <asp:TextBox ID="FirstName" runat="server" MaxLength="50" Text='<%#: BindItem.FirstName %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="LastName" runat="server" MaxLength="50" Text='<%#: BindItem.LastName %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="EmailAddress" runat="server" MaxLength="50" Text='<%#: BindItem.EmailAddress %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" CommandName="Insert" Text="Lägg till" />
                        </td>
                        <td>
                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Rensa" CausesValidation="false" />
                        </td>
                    </tr>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsertFirstName" runat="server" ErrorMessage="Förnamn måste anges." Display="None" ControlToValidate="FirstName"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsertLastName" runat="server" ErrorMessage="Efternamn måste anges." Display="None" ControlToValidate="LastName"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsetEmailAddress" runat="server" ErrorMessage="E-postadressen måste anges." Display="None" ControlToValidate="EmailAddress"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmailAddress" runat="server" ErrorMessage="E-postadressen är inte giltig." Display="None" ControlToValidate="EmailAddress" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"></asp:RegularExpressionValidator>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <%-- Template row in the table to edit contact information. --%>
                    <td>
                        <asp:TextBox ID="FirstName" runat="server" ValidationGroup="EditValidationGroup" Text='<%#: BindItem.FirstName %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="LastName" runat="server" ValidationGroup="EditValidationGroup" Text='<%#: BindItem.LastName %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="EmailAddress" runat="server" ValidationGroup="EditValidationGroup" Text='<%#: BindItem.EmailAddress %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButtonEdit" runat="server" ValidationGroup="EditValidationGroup" CommandName="Update" Text="Spara" />
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButtonCancelEdit" runat="server" CommandName="Cancel" Text="Avbryt" CausesValidation="false" />
                    </td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsertFirstName" ValidationGroup="EditValidationGroup" runat="server" ErrorMessage="Förnamn måste anges." Display="None" ControlToValidate="FirstName"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsertLastName" ValidationGroup="EditValidationGroup" runat="server" ErrorMessage="Efternamn måste anges." Display="None" ControlToValidate="LastName"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsetEmailAddress" ValidationGroup="EditValidationGroup" runat="server" ErrorMessage="E-postadressen måste anges." Display="None" ControlToValidate="EmailAddress"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmailAddress" ValidationGroup="EditValidationGroup" runat="server" ErrorMessage="E-postadressen är inte giltig." Display="None" ControlToValidate="EmailAddress" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"></asp:RegularExpressionValidator>
                </EditItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
