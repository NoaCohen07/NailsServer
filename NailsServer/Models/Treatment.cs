using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class Treatment
{
    [Key]
    [Column("TreatmentID")]
    public int TreatmentId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(80)]
    public string TreatmentText { get; set; } = null!;

    public int Duration { get; set; }

    public int Price { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Treatments")]
    public virtual User? User { get; set; }
}
