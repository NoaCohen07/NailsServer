using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NailsServer.Models;

namespace NailsServer.DTO
{
    public class Treatment
    {
        public int TreatmentId { get; set; }

       
        public int? UserId { get; set; }

        public string TreatmentText { get; set; } = null!;

        public int Duration { get; set; }

        public int Price { get; set; }

       //public virtual User? User { get; set; }

        public Treatment() { }
        public Treatment(Models.Treatment treatment)
        {
            this.TreatmentId = treatment.TreatmentId;   
            this.UserId = treatment.UserId;
            this.TreatmentText= treatment.TreatmentText;
            this.Duration = treatment.Duration;
            this.Price = treatment.Price;
        }
        public Models.Treatment GetModel()
        {
            Models.Treatment t = new Models.Treatment();
            t.TreatmentId = this.TreatmentId;
            t.UserId = this.UserId;
            t.TreatmentText = this.TreatmentText;
            t.Duration = this.Duration;
            t.Price = this.Price;
            return t;
        }
    }
}
