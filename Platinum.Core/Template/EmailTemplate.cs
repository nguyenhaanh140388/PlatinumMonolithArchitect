using Platinum.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platinum.Core.Template
{
    [Table("EmailTemplate")]
    public class EmailTemplate : EntityBase
    {
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        [Required]
        public int TemplateCode { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string TemplateBody { get; set; }
    }
}
