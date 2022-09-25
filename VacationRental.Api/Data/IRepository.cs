using System.Linq;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Data
{
    public interface IRepository<Entity> where Entity : class, IEntity
    {
        IQueryable<Entity> Table { get; }
        Entity GetById(int id);
        int Insert(Entity entity);
        bool Update(Entity entity);
    }
}