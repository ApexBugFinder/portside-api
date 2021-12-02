using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class Role
    {
        private string message;
    [Key]
    [Required]
    [MaxLength(70)]
        public string ID { get; set; }

        [ForeignKey("Experience")]
    [MaxLength(70)]
        public string ExperienceID { get; set; }

        public virtual Experience Experience { get; set; }


    [MaxLength(200)]
        public string MyTitle { get; set; }

    [MaxLength(400)]
        public string MyRole { get; set; }

        public Role()
        {

            message = "";
        }

        public void Print()
        {
            message = "\nRole ID: " + this.ID
                + "\tExperience ID: " + this.ExperienceID
                + "\tMy roles was: " + this.MyRole;

            Notification.PostMessage(message);
        }
    }
}
