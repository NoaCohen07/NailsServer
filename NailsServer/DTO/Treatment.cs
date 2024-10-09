using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Treatment
    {
        public int TreatmentId { get; set; }

       
        public int? UserId { get; set; }

        public string TreatmentText { get; set; } = null!;

        public int Duration { get; set; }

        public int Price { get; set; }


        public virtual User? User { get; set; }
    }
}
