using Kontakter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kontakter
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private Service _service;

        private Service Service
        {
            // A Service-object is created only when it's needed for the first time.
            get { return _service ?? (_service = new Service()); }
        }

        /// <summary>
        /// Gets all the contact information stored in the database.
        /// </summary>
        public IEnumerable<Contact> ContactListView_GetData()
        {
            return Service.GetContacts();
        }

        protected void PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager pager = (DataPager)ContactListView.FindControl("ContactsDataPager");
            pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            int totalRowCount;

            ContactListView_GetDataPageWise(e.StartRowIndex, e.MaximumRows, out totalRowCount);
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
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle tas bort.");
            }
        }
    }
}