using AuthLearn.Database;
using AuthLearn.Entities;
using AuthLearn.Models;
using AuthLearn.Resources;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Repositories;

public interface IPermissionsRepository
{
    public Task<PermissionInfoResource?> GetPermissionAsync(int pid);
    public Task<List<PermissionInfoResource>> GetPermissionsAsync();
    Task<List<string>> GetUserPermissionsAsync(int uid);
    public Task AddUserPermissionAsync(int uid, List<string> permissions);
    public Task ClearUserPermissionsAsync(int uid);
    public Task UpdateUserPermissionAsync(UserPermissionModel userPermissionModel);
    public Task<bool> HasPermissionAsync(int uid, string permission);
}

public class PermissionsRepository : IPermissionsRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PermissionInfoResource?> GetPermissionAsync(int pid)
    {
        var permissionEntity = await _dbContext.Permissions
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == pid);
        return permissionEntity?.ToPermissionInfoResource();
    }

    public async Task<List<PermissionInfoResource>> GetPermissionsAsync()
    {
        return await _dbContext.Permissions
            .Include(p => p.User)
            .Select(x => x.ToPermissionInfoResource())
            .ToListAsync();
    }

    public async Task<List<string>> GetUserPermissionsAsync(int uid)
    {
        var userPermissions = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.UserId == uid);
        return userPermissions is not null ? userPermissions.Permissions : [];
    }

    public async Task AddUserPermissionAsync(int uid, List<string> permissions)
    {
        await _dbContext.Permissions.AddAsync(new UserPermissionEntity()
        {
            UserId = uid,
            Permissions = permissions
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task ClearUserPermissionsAsync(int uid)
    {
        var toBeDeleted = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.UserId == uid);
        if (toBeDeleted is null) return;
        _dbContext.Permissions.Remove(toBeDeleted);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserPermissionAsync(UserPermissionModel userPermissionModel)
    {
        var toBeUpdated =
            await _dbContext.Permissions.SingleOrDefaultAsync(p => p.UserId == userPermissionModel.UserId);

        if (toBeUpdated == null) return;

        //Update Permissions
        toBeUpdated.Permissions = userPermissionModel.Permissions;

        _dbContext.Permissions.Update(toBeUpdated);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasPermissionAsync(int uid, string permission)
    {
        return await _dbContext.Permissions.AnyAsync(p => p.UserId == uid && p.Permissions.Contains(permission));
    }
}