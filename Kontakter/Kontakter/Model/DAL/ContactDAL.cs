using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Kontakter.Model.DAL
{
    /// <summary>
    /// Class for managing data.
    /// </summary>
    public class ContactDAL : DALBase
    {
        /// <summary>
        /// Retrieving a contact entry with a specific contact number from the database.
        /// </summary>
        /// <param name="contactId">Contact Information's Contact Number.</param>
        /// <returns>A Contact-object containing contact information.</returns>
        public Contact GetContactById(int contactId)
        {
            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand("Person.uspGetContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contactId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    var contactIdIndex = reader.GetOrdinal("ContactID");
                    var emailAddressIndex = reader.GetOrdinal("EmailAddress");
                    var firstNameIndex = reader.GetOrdinal("FirstName");
                    var lastNameIndex = reader.GetOrdinal("LastName");

                    if (reader.Read())
                    {
                        return new Contact
                        {
                            ContactID = reader.GetInt32(contactIdIndex),
                            EmailAddress = reader.GetString(emailAddressIndex),
                            FirstName = reader.GetString(firstNameIndex),
                            LastName = reader.GetString(lastNameIndex)
                        };
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets all the contact information stored in the database.
        /// </summary>
        /// <returns>List of references to Contact-objects containing contact information.</returns>
        public IEnumerable<Contact> GetContacts()
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var contacts = new List<Contact>(20);

                    var cmd = new SqlCommand("Person.uspGetContacts", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var contactIdIndex = reader.GetOrdinal("ContactID");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");

                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                ContactID = reader.GetInt32(contactIdIndex),
                                EmailAddress = reader.GetString(emailAddressIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex)
                            });
                        }
                    }

                    contacts.TrimExcess();

                    return contacts;
                }
                catch (Exception)
                {
                    throw new ApplicationException("An error occured while getting contacts from the database.");
                }
            }
        }

        /// <summary>
        /// Gets contact information page wise stored in the database.
        /// </summary>
        /// <returns>List of references to Contact-objects containing contact information.</returns>
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var contacts = new List<Contact>(maximumRows);

                    var cmd = new SqlCommand("Person.uspGetContactsPageWise", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = startRowIndex / maximumRows + 1;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = maximumRows;

                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    totalRowCount = (int)cmd.Parameters["@RecordCount"].Value;

                    using (var reader = cmd.ExecuteReader())
                    {
                        var contactIdIndex = reader.GetOrdinal("ContactID");
                        var emailAddressIndex = reader.GetOrdinal("EmailAddress");
                        var firstNameIndex = reader.GetOrdinal("FirstName");
                        var lastNameIndex = reader.GetOrdinal("LastName");

                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                ContactID = reader.GetInt32(contactIdIndex),
                                EmailAddress = reader.GetString(emailAddressIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex)
                            });
                        }
                    }

                    contacts.TrimExcess();

                    return contacts;
                }
                catch (Exception)
                {
                    throw new ApplicationException("An error occured while getting contacts page wise from the database.");
                }
            }
        }

        /// <summary>
        /// Creates a new entry in the table Contact.
        /// </summary>
        /// <param name="contact">Contact information that will be saved.</param>
        public void InsertContact(Contact contact)
        {
            if (contact == null) { throw new ArgumentNullException("Får ej vara null..."); }

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand("Person.uspAddContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                conn.Open();

                cmd.ExecuteNonQuery();

                contact.ContactID = (int)cmd.Parameters["@ContactId"].Value;
            }

        }

        /// <summary>
        /// Updating contact information in the table Contact.
        /// </summary>
        /// <param name="customer">Contact information that will be saved.</param>
        public void UpdateContact(Contact contact)
        {
            using (SqlConnection conn = CreateConnection())
            {

                var cmd = new SqlCommand("Person.uspUpdateContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contact.ContactID;
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Removes the specified contact information from the database.
        /// </summary>
        /// <param name="contactId">Contact Information to be deleted.</param>
        public void DeleteContact(int contactId)
        {
            using (SqlConnection conn = CreateConnection())
            {

                try
                {
                    var cmd = new SqlCommand("Person.uspRemoveContact", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContactId", SqlDbType.Int, 4).Value = contactId;
                    cmd.Parameters["@ContactId"].Value = contactId;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("FEL VID DELETE", ex);
                }
            }
        }
    }
}