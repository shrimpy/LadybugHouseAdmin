using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DonationPage.Models
{
    public class DonationEntry
    {
        [Key]
        [DisplayName("Donation ID")]
        public string DonationID { get; set; }

        [DisplayName("Donation title")]
        public string DonationTitle { get; set; }

        [DisplayName("Donation message")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DisplayName("Approved")]
        public bool Approved { get; set; }
    }
}