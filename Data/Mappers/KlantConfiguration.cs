using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LekkerLokaal.Models.Domain;

namespace LekkerLokaal.Data.Mappers
{
    internal class KlantConfiguration : IEntityTypeConfiguration<Klant>
    {
        public void Configure(EntityTypeBuilder<Klant> builder)
        {
            builder.ToTable("Klant");
            builder.HasKey(t => t.KlantId);
            builder.Property(t => t.Voornaam).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Familienaam).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Emailadres).HasMaxLength(100);
        }
    }
}
