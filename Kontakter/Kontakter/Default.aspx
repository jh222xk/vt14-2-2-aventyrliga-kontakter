<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kontakter.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/app.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <h1><a href="Default.aspx">Äventyrliga kontakter</a></h1>

            <asp:ListView ID="ContactListView" runat="server"
                ItemType="Kontakter.Model.Contact"
                SelectMethod="ContactListView_GetDataPageWise"
                InsertMethod="ContactListView_InsertItem"
                UpdateMethod="ContactListView_UpdateItem"
                DeleteMethod="ContactListView_DeleteItem"
                OnPagePropertiesChanging="PagePropertiesChanging"
                DataKeyNames="ContactId"
                ItemPlaceholderID="itemPlaceholder"
                InsertItemPosition="FirstItem">
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
                    <asp:DataPager ID="ContactsDataPager" runat="server" PagedControlID="ContactListView" PageSize="20">
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
                <AlternatingItemTemplate>
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
                </AlternatingItemTemplate>
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
                            <asp:TextBox ID="FirstNameTextBoxAdd" runat="server" Text='<%#: BindItem.FirstName %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="LastNameTextBoxAdd" runat="server" Text='<%#: BindItem.LastName %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="EmailAddressTextBoxAdd" runat="server" Text='<%#: BindItem.EmailAddress %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" CommandName="Insert" Text="Lägg till" />
                        </td>
                        <td>
                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Rensa" CausesValidation="false" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <%-- Template row in the table to edit contact information. --%>
                    <td>
                        <asp:TextBox ID="FirstNameTextBoxEdit" runat="server" Text='<%#: BindItem.FirstName %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="LastNameTextBoxEdit" runat="server" Text='<%#: BindItem.LastName %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="EmailAddressTextBoxEdit" runat="server" Text='<%#: BindItem.EmailAddress %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="Update" Text="Spara" />
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButtonCancelEdit" runat="server" CommandName="Cancel" Text="Avbryt" CausesValidation="false" />
                    </td>
                </EditItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
