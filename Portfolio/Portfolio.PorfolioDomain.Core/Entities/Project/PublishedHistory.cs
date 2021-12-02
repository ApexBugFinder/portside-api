using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class PublishedHistory
    {
        private string message = "";
        [Key]
        [Required]
        [MaxLength(70)]
        public string ID { get; set; }

        [ForeignKey("Project")]
        [MaxLength(70)]
        public string ProjectID { get; set; }

        public virtual Project Project { get; set; }
        public DateTime PublishedOn { get; set; }

        public DateTime UnPublishedOn { get; set; }

        // CONSTRUCTOR

        public PublishedHistory()
        {
        }
        public void Print()
        {
            message = "Published History Object Print Report"
                + "\nPublishedHistory  ID: " + this.ID
                + "\nPublishedHistory  ProjectId: " + this.ProjectID
                + "\nPublishedHistory Published On Date: " + this.PublishedOn
                + "\nPublishedHistory UnPublished On Date: " + this.UnPublishedOn
                ;
        }
    }
}
