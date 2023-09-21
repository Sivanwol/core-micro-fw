using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities; 

[Table("users")]
public partial class User {
    [Column("id")] public int Id { get; set; }

    [Required] [Column("auth0_id")] public string Auth0Id { get; set; } = null!;
    
    [DefaultValue(true)]
    [Column("active")]
    public bool Active { get; set; }
}