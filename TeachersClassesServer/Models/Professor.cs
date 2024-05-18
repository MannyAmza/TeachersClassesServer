using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TeachersClassesServer.Models;

[Table("Professor")]
public partial class Professor
{
    [Key]
    [Column("ProfessorID")]
    public int ProfessorId { get; set; }

    [Column("Professor")]
    [Unicode(false)]
    public string Professor1{ get; set; } = null!;

    public int CourseNum { get; set; }

    [Unicode(false)]
    public string Days { get; set; } = null!;

    [Unicode(false)]
    public string Time { get; set; } = null!;

    [Unicode(false)]
    public string Location { get; set; } = null!;

    [Column("ClassID")]
    public int ClassId { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Professors")]
    public virtual Class Class { get; set; } = null!;
}
