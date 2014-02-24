using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kontakter.Model
{
    /// <summary>
    /// Class for managing contact information.
    /// </summary>
    public class Contact
    {
        // Properties' names and types are given 
        // in the table Contact in the database.
        public int ContactID { get; set; }

        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }
    }
}