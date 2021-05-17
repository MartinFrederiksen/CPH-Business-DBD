using System.Linq;
using Microsoft.EntityFrameworkCore;
using RelationalDatabases.DataModels;
using RelationalDatabases.DataModels.QueryModels;

namespace RelationalDatabases.Persistence.Repositories
{
    public class PetRepository<T> : BaseRepository<T, BaseQueryModel>
        where T : Pet
    {
        public PetRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        
        }

        #region ApplyRelations
        protected override IQueryable<T> ApplyRelations(IQueryable<T> query)
        {
            query = query
                .Include(x => x.vet)
                    .ThenInclude(x => x.address)
                        .ThenInclude(x => x.city)
                .Include(x => x.caretaker)
                    .ThenInclude(x => x.address)
                        .ThenInclude(x => x.city);

            return base.ApplyRelations(query);
        }
        #endregion
    }
}