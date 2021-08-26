using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Data
{
    public class InMemoryRepository<Entity> : IRepository<Entity> where Entity : class, IEntity
    {
        private readonly ConcurrentDictionary<int, Entity> _entities;

        public InMemoryRepository()
        {
            _entities = new ConcurrentDictionary<int, Entity>();
        }

        private static int lastGeneratedId = 0;

        public IQueryable<Entity> Table => _entities.Values.AsQueryable();

        public Entity GetById(int id)
        {
            if (!_entities.TryGetValue(id, out var entity))
                return null;

            return entity;
        }

        public int Insert(Entity entity)
        {
            entity.Id = Interlocked.Increment(ref lastGeneratedId);
            if (!_entities.TryAdd(entity.Id, entity))
                return 0;
            return entity.Id;
        }
    }
}
