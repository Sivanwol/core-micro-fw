using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Domain.Persistence.Context;

public interface IDomainContext : IDisposable {
    DbContext Instance { get; }
    DbSet<Countries> Countries { get; set; }
}