using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
namespace Domain.Persistence.Context;

public class DomainContext : IdentityDbContext<ApplicationUser>, IDomainContext {
    private readonly ILogger<DomainContext> _logger;
    public DbContext Instance => this;

    public virtual DbSet<Countries> Countries { get; set; }

    public DomainContext(DbContextOptions<DomainContext> options, ILogger<DomainContext> logger)
        : base(options) {
        _logger = logger;
        _logger.LogInformation("Pre Initializing DomainContext.");
        Initialize();
    }

    private void Initialize() {
        Log.Information("Initializing DomainContext.");
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        _logger.LogInformation("Configuring DomainContext model.");
        builder.Entity<Countries>();
    }

    public new void Dispose() {
        _logger.LogInformation("Disposing DomainContext instance.");
        base.Dispose();
    }
}