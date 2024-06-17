using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
namespace Domain.Utils;

public static class ModelBuilderExtensions {
    public static void SetDefaultValuesTableName(this ModelBuilder modelBuilder) {
        foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.GetCustomAttribute(typeof(TableAttribute)) != null)) {
            var entityClass = entity.ClrType;

            foreach (var property in entityClass.GetProperties().Where(p =>
                         (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(Guid)) &&
                         p.CustomAttributes.Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute)))) {
                var defaultValueSql = "GetDate()";
                if (property.PropertyType == typeof(Guid)) {
                    defaultValueSql = "newsequentialid()";
                }
                modelBuilder.Entity(entityClass).Property(property.Name).HasDefaultValueSql(defaultValueSql);
            }
        }
    }
    public static void SetDefaultForIdsAndDates(this ModelBuilder modelBuilder) {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }

    }
}