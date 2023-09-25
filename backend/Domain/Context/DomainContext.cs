﻿

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Domain.Context;

public partial class DomainContext : DbContext, IDomainContext {
    public DbContext Instance => this;
    private readonly ILogger<DomainContext> _logger;

    public DomainContext(ILogger<DomainContext> logger) {
        _logger = logger;
        Initialize();
    }

    public DomainContext(DbContextOptions<DomainContext> options, ILogger<DomainContext> logger)
        : base(options) {
        _logger = logger;
        Initialize();
    }

    public void Initialize() {
        _logger.LogInformation("Initializing DBContext.");
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        _logger.LogInformation("Configuring DBContext.");
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        _logger.LogInformation("Configuring DBContext model.");
        // modelBuilder.HasPostgresEnum<GameType>(); 
        modelBuilder.Entity<User>();
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public new void Dispose() {
        _logger.LogInformation("Disposing DBContext instance.");
        base.Dispose();
    }
}