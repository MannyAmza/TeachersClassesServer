using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TeachersClassesServer.Models;

[Table("Class")]
public partial class Class
{
    [Key]
    [Column("ClassID")]
    public int ClassId { get; set; }

    [Column("CName")]
    [Unicode(false)]
    public string Cname { get; set; } = null!;

    [Column("CNum")]
    [Unicode(false)]
    public string Cnum { get; set; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<Professor> Professors { get; set; } = new List<Professor>();
}
