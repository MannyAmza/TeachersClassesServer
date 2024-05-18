using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TeachersClassesServer.Models;

public partial class ClassSourceContext : IdentityDbContext<CoursesUser>
{
    public ClassSourceContext()
    {
    }

    public ClassSourceContext(DbContextOptions<ClassSourceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; } = null!;

    public virtual DbSet<Professor> Professors { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                
               
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
       
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the IdentityUserLogin<string> entity type
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.ProfessorId).HasName("PK_Professor");

            entity.Property(e => e.ProfessorId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Class)
                .WithMany(p => p.Professors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Professor_Class");
            //FK_Professor_Class
            //PK_Class


        }); 
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
