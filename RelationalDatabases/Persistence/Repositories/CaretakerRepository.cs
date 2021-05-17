using System.Linq;
using Microsoft.EntityFrameworkCore;
using RelationalDatabases.DataModels;
using RelationalDatabases.DataModels.QueryModels;

namespace RelationalDatabases.Persistence.Repositories
{
    public class CaretakerRepository : BaseRepository<Caretaker, BaseQueryModel>
    {
        public CaretakerRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            
        }
        #region ApplyRelations
        protected override IQueryable<Caretaker> ApplyRelations(IQueryable<Caretaker> query)
        {
            query = query
                .Include(x => x.address)
                    .ThenInclude(x => x.city)
                .Include(x => x.pets);

            return base.ApplyRelations(query);
        }
        #endregion
    }
}