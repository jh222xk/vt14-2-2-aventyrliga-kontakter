using Kontakter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kontakter
{

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ee256141(v=vs.100).aspx
    /// </summary>

    public partial class Default : System.Web.UI.Page
    {
        private DataPager _lastPage;

        private Service _service;

        private DataPager LastPage
        {
            get { return _lastPage ?? (_lastPage = (DataPager)ContactListView.FindControl("ContactsDataPager")); }
        }

        private Service Service
        {
            // A Service-object is created only when it's needed for the first time.
            get { return _service ?? (_service = new Service()); }
        }

        private string Message
        {
            get { return Session["Message"] as string; }
            set
            {
                Session["Message"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ContactListView.InsertItemPosition = InsertItemPosition.None;

            if (Message != null)
            {
                PanelSuccess.Visible = true;
                var messageString = Message;
                LabelSuccess.Text += messageString;
                Message = messageString;
                Session.Clear();
            }
        }

        /// <summary>
        /// Gets all the contact information stored in the database.
        /// </summary>
        public IEnumerable<Contact> ContactListView_GetData()
        {
            return Service.GetContacts();
        }

        /// <summary>
        /// Sets the number of rows to show and the maximumrows.
        /// </summary>
        protected void PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            LastPage.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        }

        /// <summary>
        /// Gets contact information page wise from the database.
        /// </summary>
        public IEnumerable<Contact> ContactListView_GetDataPageWise(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

        /// <summary>
        /// Adds the contact information in the database.
        /// </summary>
        public void ContactListView_InsertItem(Contact contact)
        {
            try
            {
                Service.SaveContact(contact);
                Message = "Kontaktuppgiften har lagts till.";
                Response.Redirect(String.Format("?page={0}", LastPage.TotalRowCount / LastPage.MaximumRows + 1));
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle läggas till.");
            }
        }

        /// <summary>
        /// Updating the contact information in the database.
        /// </summary>
        public void ContactListView_UpdateItem(int contactId)
        {
            try
            {
                var contact = Service.GetContact(contactId);
                if (contact == null)
                {
                    ModelState.AddModelError(String.Empty,
                        String.Format("Kontaktuppgiften med kontaktnummer {0} hittades inte", contactId));
                    return;
                }

                if (TryUpdateModel(contact))
                {
                    Service.SaveContact(contact);
                    var messageString = String.Format("Kontaktuppgiften '{0}' har uppdaterats.", contact.ContactID);
                    Message = messageString;
                    Response.Redirect(String.Format("?page={0}", LastPage.TotalRowCount / LastPage.MaximumRows + 1));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle uppdateras.");
            }
        }

        /// <summary>
        /// Removes the specified contact information from the database.
        /// </summary>
        public void ContactListView_DeleteItem(int contactId)
        {
            try
            {
                Service.DeleteContact(contactId);
                Message = "Kontaktuppgiften har tagits bort.";
                Response.Redirect(String.Format("?page={0}", LastPage.TotalRowCount / LastPage.MaximumRows + 1));
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle tas bort.");
            }
        }

        protected void ButtonNewContact_Click(object sender, EventArgs e)
        {
            ContactListView.InsertItemPosition = InsertItemPosition.FirstItem;
        }
    }
}