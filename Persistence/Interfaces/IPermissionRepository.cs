﻿using Domain;
using Domain.StoreResults;

namespace Persistence.Interfaces;

public interface IPermissionRepository
{
    Task<Permission> GetByName(string permissionName);
    Task<bool> AddUserPermission(User user, string permissionName);
    Task<bool> IsPermissionAdded(User user, string permissionName);
    Task<IEnumerable<UserPermissionResult>> GetAll();
    Task<bool> DeleteUserFromPermission(User user, string permissionName);
    Task<HashSet<string>> GetUserPermissions(Guid userId);
}