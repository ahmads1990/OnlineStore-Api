using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OnlineStore_Api.Repositories;

public class Repository<Entity> : IRepository<Entity> where Entity : BaseEntity
{
    private AppDbContext _context { get; set; }
    DbSet<Entity> _entities;
    public Repository(AppDbContext context)
    {
        _context = context;
        _entities = _context.Set<Entity>();
    }
    public IQueryable<Entity> GetAll()
    {
        return _entities.Where(e => !e.Deleted);
    }
    public IQueryable<Entity> GetAllWithDeleted()
    {
        return _entities;
    }
    public async Task<Entity?> GetByID(int id)
    {
        return await GetAll()
                    .Where(e => e.ID == id)
                    .FirstOrDefaultAsync();
    }
    public async Task Add(Entity entity)
    {
        entity.CreatedDate = DateTime.Now;
        await _entities.AddAsync(entity);
    }
    public void Update(Entity entity)
    {
        _entities.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
    public void SaveInclude(Entity entity, params string[] properties)
    {
        // Try to find entity in memory
        var local = _entities.Local.FirstOrDefault(x => x.ID == entity.ID);

        EntityEntry entry = default!;

        if (local is null)
        {
            // couldn't find it in memory then create it
            entry = _context.Entry(entity);
        }
        else
        {
            entry = _context.ChangeTracker
                            .Entries<Entity>()
                            .First(x => x.Entity.ID == entity.ID);
        }

        foreach (var property in entry.Properties)
        {
            if (properties.Contains(property.Metadata.Name))
            {
                property.CurrentValue = entity.GetType()
                                              .GetProperty(property.Metadata.Name)
                                              .GetValue(entity);
                property.IsModified = true;
            }
        }
    }
    public void Delete(Entity entity)
    {
        _entities.Remove(entity);
    }
    public void HardDelete(Entity entity)
    {
        entity.Deleted = true;
        SaveInclude(entity, nameof(BaseEntity.Deleted));
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
