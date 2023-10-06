using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Domain.Context;

public class DomainContext : IdentityDbContext<ApplicationUser>, IDomainContext {
    public DbContext Instance => this;

    public DbSet<Countries> Countries { get; set; }

    public DomainContext(DbContextOptions<DomainContext> options)
        : base(options) {
        Initialize();
    }

    private void Initialize() {
        Log.Information("Initializing DomainContext.");
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
    }
}