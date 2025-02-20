using System;
using System.ComponentModel.DataAnnotations;

namespace CRMConnect.Models
{
    public class CommunicationLog
    {
        public int LogID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public string CommunicationType { get; set; }

        [Required]
        public DateTime CommunicationDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }
    }
}

