using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Context;

public interface IDomainContext : IDisposable {
    DbContext Instance { get; }
}