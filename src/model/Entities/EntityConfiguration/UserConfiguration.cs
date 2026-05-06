using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace src.model.Entities.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b=>b.Email).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Name);//.IsRequired().HasMaxLength(200);
            builder.HasIndex(b=>b.GoogleID).IsUnique();
        }

    }
}