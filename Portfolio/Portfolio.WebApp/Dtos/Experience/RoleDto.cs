using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Dtos
{
    public class RoleDto
    {
        private string message;

        public string ID { get; set; }


        public string ExperienceID { get; set; }




        public string MyTitle { get; set; }
        public string MyRole { get; set; }
        public string EditState { get; set; }

        public RoleDto()
        {

            message = "";
        }

        public void Print()
        {
            message = "\nRoleDto ID: " + this.ID
                + "\tExperienceDto ID: " + this.ExperienceID
                + "\tMy roles was: " + this.MyRole;

            Notification.PostMessage(message);
        }
    }
}
