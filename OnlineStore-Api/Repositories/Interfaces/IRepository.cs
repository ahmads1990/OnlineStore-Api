using System.Linq.Expressions;

namespace OnlineStore_Api.Repositories.Interfaces;

public interface IRepository<Entity>
{
    //IQueryable<Entity> GetQueryable();
    IQueryable<Entity> GetAll();
    IQueryable<Entity> GetAllWithDeleted();
    Task<Entity?> GetByID(int id);
    Task Add(Entity entity);
    void Update(Entity entity);
    void SaveInclude(Entity entity, params string[] properties);
    void Delete(Entity entity);
    void HardDelete(Entity entity);
    void SaveChanges();
    Task SaveChangesAsync();
}
