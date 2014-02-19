using Kontakter.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kontakter.Model
{
    /// <summary>
    /// The class provides methods presentation logic layer 
    /// Calls to manage data. Mainly contains the class 
    /// Methods that make use of classes in the data access layer.
    /// </summary>
    public class Service
    {
        private ContactDAL _contactDAL;

        public ContactDAL ContactDAL
        {
            // A ContactDAL-object is created only when it's needed for the first time.
            get { return _contactDAL ?? (_contactDAL = new ContactDAL()); }
        }

        /// <summary>
        /// Retrieving a contact entry with a specific contact number from the database.
        /// </summary>
        /// <param name="contactId">Contact Information's Contact Number.</param>
        /// <returns>A Contact-object containing contact information.</returns>
        public Contact GetContact(int contactId)
        {
            return ContactDAL.GetContactById(contactId);
        }

        /// <summary>
        /// Gets all the contact information stored in the database.
        /// </summary>
        /// <returns>List of references to Contact-objects containing contact information.</returns>
        public IEnumerable<Contact> GetContacts()
        {
            return ContactDAL.GetContacts();
        }

        /// <summary>
        /// Gets contact information page wise stored in the database.
        /// </summary>
        /// <returns>List of references to Contact-objects containing contact information.</returns>
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

        /// <summary>
        /// Save the contact information to the database.
        /// </summary>
        /// <param name="contact">Contact information that will be saved.</param>
        public void SaveContact(Contact contact)
        {
            // Create a new entry if ContactID is equal to zero.
            if (contact.ContactID == 0) 
            {
                ContactDAL.InsertContact(contact);
            }
            else
            {
                ContactDAL.UpdateContact(contact);
            }
        }

        /// <summary>
        /// Removes the specified contact information from the database.
        /// </summary>
        /// <param name="contactId">Contact Information to be deleted.</param>
        public void DeleteContact(int contactId)
        {
            ContactDAL.DeleteContact(contactId);
        }
    }
}