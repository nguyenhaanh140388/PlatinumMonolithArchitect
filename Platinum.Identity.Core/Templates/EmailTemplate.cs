using Platinum.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platinum.Identity.Core.Templates
{
    public class EmailTemplate : EntityBase
    {
        [Required]
        public string TemplateName { get; set; }
        [Required]

        public string Subject { get; set; }

        [Required]
        public int TemplateCode { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string TemplateBody { get; set; }
    }
}
