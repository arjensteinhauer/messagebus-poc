using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MB.Access.Tenant.Database.Models
{
    [Table("Message", Schema = "tenant")]
    public class Message
    {
        [Required]
        [MaxLength(100)]
        public string TenantName { get; set; }

        public int MessageId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTimeOffset PublishedOnUTC { get; set; }
    }
}
