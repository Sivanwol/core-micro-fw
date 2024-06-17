using System.Linq.Dynamic.Core;
using Application.Exceptions;
using Domain.DTO.ConfigurableEntities;
using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.Requests.Processor.Services.Views;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Domain.Persistence.Repositories;

public class ConfigurableUserViewRepository : BaseRepository, IConfigurableUserViewRepository {
    public ConfigurableUserViewRepository(IDomainContext context) : base(context) { }
    public async Task<bool> HasViewExist(string userId, Guid viewClientId) {
        Log.Logger.Information("Checking if view {ViewClientId} exists for user {UserId}", viewClientId, userId);
        var userView = await Context.ConfigurableUserViews.FirstOrDefaultAsync(w =>
            w.UserId == userId && w.ClientUniqueKey == viewClientId && w.IsPredefined == false && w.DeletedAt == null);
        return userView != null;
    }
    public async Task<bool> HasViewDefault(Guid viewClientId) {
        Log.Logger.Information("Checking if view {ViewClientId} is default for system", viewClientId);
        var view = await Context.ConfigurableUserViews.FirstOrDefaultAsync(w => w.ClientUniqueKey == viewClientId && w.DeletedAt == null && w.IsPredefined == true);
        return view != null;
    }

    public async Task<ICollection<ViewFilter>> GetConfigurableViewFilters(ConfigurableUserView view, List<string>? permissions = null) {
        Log.Logger.Information("Fetch configurable user view filters for user view [{ViewUserId} ,{ViewId}]", view.UserId, view.Id);
        return (from vf in Context.ConfigurableUserViewsFilters
            where vf.ViewId == view.Id
            select new ViewFilter {
                Id = vf.Id,
                FilterCollectionOperation = vf.FilterCollectionOperation,
                FilterDateType = vf.FilterDateType,
                FilterFieldType = vf.FilterFieldType,
                FilterOperations = vf.FilterOperations,
                FilterFieldName = vf.FilterFieldName,
                Values = vf.FilterValues
            }).ToList();
    }

    public async Task<ICollection<ViewColumn>> GetConfigurableViewColumns(ConfigurableUserView view, List<string>? permissions = null) {
        Log.Logger.Information("Fetching configurable user view columns for user view [{ViewUserId} ,{ViewId}]", view.UserId, view.Id);
        List<ViewColumn> selectedColumns;
        if (permissions == null) {
            selectedColumns = (from vc in Context.ConfigurableUserViewHasConfigurableEntityColumnDefinitions
                where vc.ConfigurableUserViewId == view.Id && vc.ConfigurableEntityColumnDefinition.DisabledAt == null
                orderby vc.Position descending
                select new ViewColumn() {
                    Id = vc.ConfigurableEntityColumnDefinitionId,
                    DisplayName = vc.ConfigurableEntityColumnDefinition.DisplayName,
                    EntityName = vc.ConfigurableEntityColumnDefinition.EntityName,
                    FieldAlias = vc.ConfigurableEntityColumnDefinition.FieldAlias,
                    FieldPath = vc.ConfigurableEntityColumnDefinition.FieldPath,
                    FieldFormatter = vc.ConfigurableEntityColumnDefinition.FieldFormatter,
                    Description = vc.ConfigurableEntityColumnDefinition.Description,
                    ColumnName = vc.ConfigurableEntityColumnDefinition.ColumnName,
                    IsFilterable = vc.ConfigurableEntityColumnDefinition.IsFilterable,
                    IsSortable = vc.ConfigurableEntityColumnDefinition.IsSortable,
                    DataType = vc.ConfigurableEntityColumnDefinition.DataType,
                    FilterOperationType = vc.ConfigurableEntityColumnDefinition.FilterOperationType,
                    IsHidden = vc.IsHidden,
                    IsFixed = vc.IsFixed,
                    Order = vc.Position
                }).ToList();
            return selectedColumns;
        }
        selectedColumns = (from vc in Context.ConfigurableUserViewHasConfigurableEntityColumnDefinitions
            where vc.ConfigurableUserViewId == view.Id && vc.ConfigurableEntityColumnDefinition.DisabledAt == null &&
                  vc.ConfigurableEntityColumnDefinition.Permissions.Any(p => permissions.Contains(p))
            orderby vc.Position descending
            select new ViewColumn() {
                Id = vc.ConfigurableEntityColumnDefinitionId,
                DisplayName = vc.ConfigurableEntityColumnDefinition.DisplayName,
                EntityName = vc.ConfigurableEntityColumnDefinition.EntityName,
                FieldAlias = vc.ConfigurableEntityColumnDefinition.FieldAlias,
                FieldPath = vc.ConfigurableEntityColumnDefinition.FieldPath,
                Description = vc.ConfigurableEntityColumnDefinition.Description,
                FieldFormatter = vc.ConfigurableEntityColumnDefinition.FieldFormatter,
                ColumnName = vc.ConfigurableEntityColumnDefinition.ColumnName,
                IsFilterable = vc.ConfigurableEntityColumnDefinition.IsFilterable,
                IsSortable = vc.ConfigurableEntityColumnDefinition.IsSortable,
                DataType = vc.ConfigurableEntityColumnDefinition.DataType,
                FilterOperationType = vc.ConfigurableEntityColumnDefinition.FilterOperationType,
                IsHidden = vc.IsHidden,
                IsFixed = vc.IsFixed,
                Order = vc.Position
            }).ToList();
        return selectedColumns;
    }

    public async Task<ICollection<ViewColumn>> GetAvailableColumnsForView(GetAvailableColumnsForViewRequest input, List<string>? permissions = null) {
        ICollection<ViewColumn> columns;
        var entityName = GetEntityName(input.EntityType);
        if (permissions == null) {
            var columnsQuery = (from vc in Context.ConfigurableEntityColumnDefinitions
                where vc.EntityName == entityName && vc.DisabledAt == null
                select new ViewColumn() {
                    Id = vc.Id,
                    DisplayName = vc.DisplayName,
                    EntityName = vc.EntityName,
                    FieldAlias = vc.FieldAlias,
                    FieldPath = vc.FieldPath,
                    Description = vc.Description,
                    FieldFormatter = vc.FieldFormatter,
                    ColumnName = vc.ColumnName,
                    IsFilterable = vc.IsFilterable,
                    IsSortable = vc.IsSortable,
                    DataType = vc.DataType,
                    FilterOperationType = vc.FilterOperationType,
                });
            columns = await columnsQuery.ToListAsync();
        } else {
            var columnsQuery = (from vc in Context.ConfigurableEntityColumnDefinitions
                where vc.EntityName == entityName && vc.DisabledAt == null && vc.Permissions.Any(p => permissions.Contains(p))
                select new ViewColumn() {
                    Id = vc.Id,
                    DisplayName = vc.DisplayName,
                    EntityName = vc.EntityName,
                    FieldAlias = vc.FieldAlias,
                    FieldPath = vc.FieldPath,
                    Description = vc.Description,
                    FieldFormatter = vc.FieldFormatter,
                    ColumnName = vc.ColumnName,
                    IsFilterable = vc.IsFilterable,
                    IsSortable = vc.IsSortable,
                    DataType = vc.DataType,
                    FilterOperationType = vc.FilterOperationType,
                });
            columns = await columnsQuery.ToListAsync();
        }
        return columns;
    }

    public async Task<View> GetViewDefinition(string userId, Guid viewClientId, List<string> permissions = null) {
        ICollection<ViewColumn> columns;
        var isDefaultView = await HasViewDefault(viewClientId);
        ConfigurableUserView? view;
        if (isDefaultView) {
            view = await GetViewModel(userId, viewClientId, permissions);
            columns = await GetConfigurableViewColumns(view, permissions);
        } else {
            var hasView = await HasViewExist(userId, viewClientId);
            if (!hasView) {
                Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
                throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{userId}-{viewClientId}]");
            }
            view = await GetViewModel(userId, viewClientId, permissions);
            columns = await GetConfigurableViewColumns(view, permissions);
        }
        var filter = await GetConfigurableViewFilters(view, permissions);
        return ToView(view, columns, filter);
    }

    public async Task<ICollection<View>> GetViews(string userId, List<string>? permissions = null) {
        Log.Logger.Information($"Fetching configurable user views for user {userId}");
        List<ConfigurableUserView> views;
        ICollection<View> result = new List<View>();
        if (permissions == null) {
            views = await Context.ConfigurableUserViews
                .Where(w => (w.UserId == userId && w.DeletedAt == null && w.IsPredefined == false) || (w.DeletedAt == null && w.IsPredefined == true)).ToListAsync();
        } else {
            views = await Context.ConfigurableUserViews.Where(w =>
                (w.UserId == userId && w.DeletedAt == null && w.IsPredefined == false && w.Permissions.Any(p => permissions.Contains(p))) ||
                (w.DeletedAt == null && w.IsPredefined == true && w.Permissions.Any(p => permissions.Contains(p)))).ToListAsync();
        }
        foreach (var view in views) {
            var columns = await GetConfigurableViewColumns(view, permissions);
            var filters = await GetConfigurableViewFilters(view, permissions);
            result.Add(ToView(view, columns, filters));
        }
        return result;
    }

    public async Task<View> GetView(string userId, Guid viewClientId, List<string>? permissions) {
        Log.Logger.Information($"Fetching configurable user view for user {userId} and view {viewClientId}");
        var view = await GetViewModel(userId, viewClientId, permissions);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{userId}-{viewClientId}]");
        }
        var columns = await GetConfigurableViewColumns(view, permissions);
        var filters = await GetConfigurableViewFilters(view, permissions);
        return ToView(view, columns, filters);
    }

    public async Task<View> CreateView(CreateViewRequest input, List<string>? permissions) {
        var view = await GetViewModel(input.UserId, input.ViewClientKey, permissions);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
        }
        var columns = await GetViewColumns(view);
        var hasAddedView = false;
        if (view.IsPredefined) {
            view = await AddNewView(view, columns, input, permissions);
            hasAddedView = true;
        }
        if (view.IsShareAble && !view.IsPredefined && !hasAddedView) {
            view = await AddNewView(view, columns, input, permissions);
            hasAddedView = true;
        }
        if (view.UserId == input.UserId && !view.IsShareAble && !hasAddedView) {
            view = await AddNewView(view, columns, input, permissions);
            hasAddedView = true;
        }
        if (hasAddedView) {
            columns = await GetViewColumns(view);
            return ToView(view, columns.Select(vc => new ViewColumn() {
                Id = vc.ConfigurableEntityColumnDefinitionId,
                DisplayName = vc.ConfigurableEntityColumnDefinition.DisplayName,
                EntityName = vc.ConfigurableEntityColumnDefinition.EntityName,
                FieldAlias = vc.ConfigurableEntityColumnDefinition.FieldAlias,
                FieldPath = vc.ConfigurableEntityColumnDefinition.FieldPath,
                Description = vc.ConfigurableEntityColumnDefinition.Description,
                FieldFormatter = vc.ConfigurableEntityColumnDefinition.FieldFormatter,
                ColumnName = vc.ConfigurableEntityColumnDefinition.ColumnName,
                IsFilterable = vc.ConfigurableEntityColumnDefinition.IsFilterable,
                IsSortable = vc.ConfigurableEntityColumnDefinition.IsSortable,
                DataType = vc.ConfigurableEntityColumnDefinition.DataType,
                FilterOperationType = vc.ConfigurableEntityColumnDefinition.FilterOperationType,
                IsHidden = vc.IsHidden,
                IsFixed = vc.IsFixed,
                Order = vc.Position
            }), new List<ViewFilter>());
        }
        Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
        throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
    }

    public async Task<View> UpdateView(UpdateViewRequest input) {
        var view = await GetViewModel(input.UserId, input.ViewClientKey, null);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
        }
        if (view.IsPredefined) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View is predefined");
            throw new AuthorizationException();
        }
        if (view.UserId != input.UserId) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View is not owned by user");
            throw new AuthorizationException();
        }
        var columns = await GetViewColumns(view);
        view.Name = input.Name;
        view.Description = input.Description;
        view.IsShareAble = input.IsShareAble;
        view.Color = input.Color;
        Context.ConfigurableUserViews.Update(view);
        await Context.Instance.SaveChangesAsync();
        return ToView(view, columns.Select(vc => new ViewColumn() {
            Id = vc.ConfigurableEntityColumnDefinitionId,
            DisplayName = vc.ConfigurableEntityColumnDefinition.DisplayName,
            EntityName = vc.ConfigurableEntityColumnDefinition.EntityName,
            FieldAlias = vc.ConfigurableEntityColumnDefinition.FieldAlias,
            FieldPath = vc.ConfigurableEntityColumnDefinition.FieldPath,
            Description = vc.ConfigurableEntityColumnDefinition.Description,
            FieldFormatter = vc.ConfigurableEntityColumnDefinition.FieldFormatter,
            ColumnName = vc.ConfigurableEntityColumnDefinition.ColumnName,
            IsFilterable = vc.ConfigurableEntityColumnDefinition.IsFilterable,
            IsSortable = vc.ConfigurableEntityColumnDefinition.IsSortable,
            DataType = vc.ConfigurableEntityColumnDefinition.DataType,
            FilterOperationType = vc.ConfigurableEntityColumnDefinition.FilterOperationType,
            IsHidden = vc.IsHidden,
            IsFixed = vc.IsFixed,
            Order = vc.Position
        }), new List<ViewFilter>());
    }

    public async Task<bool> IsAllowViewCreation(CreateViewRequest input) {
        var view = await GetViewModel(input.UserId, input.ViewClientKey, null);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
        }
        var query = (from vc in Context.ConfigurableUserViews
            where vc.UserId == input.UserId && vc.Name == input.Name && vc.DeletedAt == null && vc.EntityType == view.EntityType
            select vc).Count();
        return query == 0;
    }

    public async Task<View> UpdateViewFilter(UpdateViewFilterRequest input) {
        var view = await GetViewModel(input.UserId, input.ViewClientKey, null);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
        }
        var columns = await GetConfigurableViewColumns(view, null);
        // let clear the filters
        var existedFilters = Context.ConfigurableUserViewsFilters.Where(x => x.ViewId == view.Id);
        if (existedFilters.Any()) {
            Context.ConfigurableUserViewsFilters.RemoveRange(existedFilters);
        }
        foreach (var item in input.Filters) {
            Context.ConfigurableUserViewsFilters.Add(new ConfigurableUserViewsFilters {
                UserId = input.UserId,
                ViewId = view.Id,
                FilterCollectionOperation = item.FilterCollectionOperation,
                FilterDateType = item.FilterDateType,
                FilterOperations = item.FilterOperations,
                FilterFieldType = item.FilterFieldType,
                FilterFieldName = item.FilterFieldName,
                FilterValues = item.Values
            });
        }
        await Context.Instance.SaveChangesAsync();
        var filters = await GetConfigurableViewFilters(view, null);
        return ToView(view, columns, filters);
    }
    
    public async Task<View> UpdateViewColumns(UpdateViewColumnsRequest input) {
        return await Context.ExecuteTransactionAsync(async () => {
            var view = await GetViewModel(input.UserId, input.ViewClientKey, null);
            if (view == null) {
                Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
                throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{input.UserId}-{input.ViewClientKey}]");
            }
            var columns = new List<ConfigurableUserViewHasConfigurableEntityColumnDefinition>();
            var clearColumns = await GetViewColumns(view);
            Context.ConfigurableUserViewHasConfigurableEntityColumnDefinitions.RemoveRange(clearColumns);
            await Context.Instance.SaveChangesAsync();

            foreach (var column in input.Columns) {
                var tColumn = await Context.ConfigurableUserViewHasConfigurableEntityColumnDefinitions.FirstOrDefaultAsync(w => w.ConfigurableEntityColumnDefinitionId == column);
                if (tColumn == null) {
                    Log.Logger.Error($"ConfigurableUserViewRepository: Column not found");
                    throw new EntityNotFoundException(nameof(ConfigurableEntityColumnDefinition), $"[{column}]");
                }
                columns.Add(new ConfigurableUserViewHasConfigurableEntityColumnDefinition() {
                    ConfigurableEntityColumnDefinitionId = tColumn.ConfigurableEntityColumnDefinitionId,
                    ConfigurableUserViewId = view.Id,
                    IsHidden = tColumn.IsHidden,
                    IsFixed = tColumn.IsFixed,
                    Position = tColumn.Position
                });
            }

            view.Columns = columns;
            Context.ConfigurableUserViews.Update(view);
            await Context.Instance.SaveChangesAsync();
            var newColumns = await GetViewColumns(view);
            return ToView(view, newColumns.OrderBy(vc => vc.Position).Select(vc => new ViewColumn() {
                Id = vc.ConfigurableEntityColumnDefinitionId,
                DisplayName = vc.ConfigurableEntityColumnDefinition.DisplayName,
                EntityName = vc.ConfigurableEntityColumnDefinition.EntityName,
                FieldAlias = vc.ConfigurableEntityColumnDefinition.FieldAlias,
                FieldPath = vc.ConfigurableEntityColumnDefinition.FieldPath,
                Description = vc.ConfigurableEntityColumnDefinition.Description,
                FieldFormatter = vc.ConfigurableEntityColumnDefinition.FieldFormatter,
                ColumnName = vc.ConfigurableEntityColumnDefinition.ColumnName,
                IsFilterable = vc.ConfigurableEntityColumnDefinition.IsFilterable,
                IsSortable = vc.ConfigurableEntityColumnDefinition.IsSortable,
                DataType = vc.ConfigurableEntityColumnDefinition.DataType,
                FilterOperationType = vc.ConfigurableEntityColumnDefinition.FilterOperationType,
                IsHidden = vc.IsHidden,
                IsFixed = vc.IsFixed,
                Order = vc.Position
            }), new List<ViewFilter>());
        }, (ex) => {
            Log.Logger.Error(ex, "Failed to update view columns");
            throw ex;
        });
    }

    public async Task<bool> DeleteView(string userId, Guid viewClientId) {
        var view = await GetViewModel(userId, viewClientId, null);
        if (view == null) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View not found");
            return false;
        }
        if (view.IsPredefined) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View is predefined");
            return false;
        }
        if (view.UserId != userId) {
            Log.Logger.Error($"ConfigurableUserViewRepository: View is not owned by user");
            return false;
        }
        view.DeletedAt = DateTime.UtcNow;
        Context.ConfigurableUserViews.Update(view);
        await Context.Instance.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerifyViewExist(VerifyViewExistRequest input) {
        var view = await GetViewModel(input.UserId, input.ViewClientKey, null);
        return (view != null && view.Name == input.Name);
    }

    private string GetEntityName(EntityTypes entityType) {
        var entityName = "";
        switch (entityType) {
            case EntityTypes.Activity:
            case EntityTypes.UserActivity:
                entityName = "ActivityLogs";
                break;
            case EntityTypes.Users:
                entityName = "ApplicationUser";
                break;
            case EntityTypes.Clients:
                entityName = "Clients";
                break;
            case EntityTypes.ClientContracts:
                entityName = "ClientContracts";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
        }
        return entityName;
    }

    private async Task<ConfigurableUserView> AddNewView(ConfigurableUserView view, IEnumerable<ConfigurableUserViewHasConfigurableEntityColumnDefinition> columns, CreateViewRequest input, List<string>? permissions) {
        var newView = new ConfigurableUserView() {
            ClientUniqueKey = Guid.NewGuid(),
            ParentId = view.Id,
            Name = input.Name,
            Description = input.Description,
            IsPredefined = false,
            EntityType = view.EntityType,
            IsShareAble = input.IsShareAble,
            Settings = new List<ConfigurableEntityMetaData>(),
            Color = input.Color,
            UserId = input.UserId,

            Permissions = permissions ?? new List<string>()
        };
        var columnsToAdd = new List<ConfigurableUserViewHasConfigurableEntityColumnDefinition>();
        foreach (var column in columns) {
            if (permissions != null) {
                if (permissions.Any() && column.ConfigurableEntityColumnDefinition.Permissions.Any(p => permissions.Contains(p))) {
                    columnsToAdd.Add(new ConfigurableUserViewHasConfigurableEntityColumnDefinition() {
                        ConfigurableEntityColumnDefinitionId = column.ConfigurableEntityColumnDefinitionId,
                        ConfigurableUserViewId = newView.Id,
                        IsHidden = column.IsHidden,
                        IsFixed = column.IsFixed,
                        Position = column.Position
                    });
                    continue;
                }
            }
            columnsToAdd.Add(new ConfigurableUserViewHasConfigurableEntityColumnDefinition() {
                ConfigurableEntityColumnDefinitionId = column.ConfigurableEntityColumnDefinitionId,
                ConfigurableUserViewId = newView.Id,
                IsHidden = column.IsHidden,
                IsFixed = column.IsFixed,
                Position = column.Position
            });
        }
        newView.Columns = columnsToAdd;
        await Context.ConfigurableUserViews.AddAsync(newView);
        await Context.Instance.SaveChangesAsync();
        return newView;
    }

    private async Task<ConfigurableUserView?> GetViewModel(string userId, Guid viewClientId, List<string>? permissions) {
        Log.Logger.Information("Fetching configurable user view for user {UserId} and view {ViewClientId}", userId, viewClientId);
        var fetchingView = (from view in Context.ConfigurableUserViews
            where view.ClientUniqueKey == viewClientId && view.DeletedAt == null
            select view);
        if (permissions != null && permissions.Any()) {
            fetchingView = fetchingView.Where(w => w.Permissions.Any(p => permissions.Contains(p)));
        }
        if (fetchingView == null) {
            Log.Logger.Error("View {ViewClientId} not found for user {UserId}", viewClientId, userId);
            throw new EntityNotFoundException(nameof(ConfigurableUserView), $"[{userId}]-[{viewClientId}]");
        }
        var tView = fetchingView.FirstOrDefault();
        return tView != null && tView.IsPredefined ? tView : fetchingView.FirstOrDefault(w => w.UserId == userId);
    }

    private async Task<IEnumerable<ConfigurableUserViewHasConfigurableEntityColumnDefinition>> GetViewColumns(ConfigurableUserView view) {
        Log.Logger.Information("Fetching configurable user view columns for user view [{ViewUserId} ,{ViewId}]", view.UserId, view.Id);
        var selectedColumns = (from vc in Context.ConfigurableUserViewHasConfigurableEntityColumnDefinitions
            where vc.ConfigurableUserViewId == view.Id
            orderby vc.Position descending
            select vc).Include(vc => vc.ConfigurableEntityColumnDefinition);
        return selectedColumns;
    }

    private View ToView(ConfigurableUserView view, IEnumerable<ViewColumn> columns, IEnumerable<ViewFilter> filters) {
        return new View() {
            ClientKey = view.ClientUniqueKey,
            Name = view.Name,
            EntityType = view.EntityType,
            Description = view.Description,
            IsPredefined = view.IsPredefined,
            IsShareAble = view.IsShareAble,
            Color = view.Color,
            Columns = columns,
            Filters = filters
        };
    }
    private View ToView(ConfigurableUserView view) {
        return new View() {
            ClientKey = view.ClientUniqueKey,
            Name = view.Name,
            EntityType = view.EntityType,
            Description = view.Description,
            IsPredefined = view.IsPredefined,
            IsShareAble = view.IsShareAble,
            Color = view.Color
        };
    }
}