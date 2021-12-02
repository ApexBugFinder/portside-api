using AutoMapper;
using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Dtos
{
    public class PublishedHistoryDto
    {
        private string message;
        public string ID { get; set; }
        public string ProjectID { get; set; }
//        public virtual ProjectDto ProjectDto { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime UnPublishedOn { get; set; }

        // CONSTRUCTOR

        public PublishedHistoryDto()
        {
            message = "";
        }

        public void Print()
        {
            message = "Published History Object Print Report"
                + "\nPublishedHistory  ID: " + this.ID
                + "\nPublishedHistory  ProjectId: " + this.ProjectID
                + "\nPublishedHistory Published On Date: " + this.PublishedOn
                + "\nPublishedHistory UnPublished On Date: " + this.UnPublishedOn
                ;


            Notification.PostMessage(message);




        }
    }
}
