using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatientApp.Data;

public class PatientContext:DbContext
{
    public PatientContext(DbContextOptions<PatientContext> options): base(options)
    {
        
    }
    
    //TODO: when we upgrade this to .net 7 remove the null suppression
    //suppressed the null warning with null!.. this is known issue with ef core 6.0
    //it is fixed in ef core 7+
    public DbSet<Model.Patient> Patient { get; set; } = null!;
    public DbSet<Model.Key> Key { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {
        //Configure default schema
        modelBuilder.HasDefaultSchema("dbo");
        
        //Patient Table
        modelBuilder.Entity<Model.Patient>(b =>
        {
            b.HasKey(x => x.IdPatient);
            b.Property(x => x.IdPatient).ValueGeneratedOnAdd().UseIdentityColumn(1);
            b.Ignore(x => x.BirthDate);
            b.Property(x => x.FirstName).HasMaxLength(50);
            b.Property(x => x.LastName).HasMaxLength(50);
            b.Property(x => x.BirthDate).HasMaxLength(50);
        });

        modelBuilder.Entity<Model.Key>(b =>
        {
            b.HasKey(x => x.IdPatient);
            b.Property(x => x.Value).HasMaxLength(50);
        });
    }
    public override int SaveChanges()
    {
        ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity)
            .ToList()
            .ForEach(entity =>
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            });
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity)
            .ToList()
            .ForEach(entity =>
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            });
        return base.SaveChangesAsync(cancellationToken);
    } 
}