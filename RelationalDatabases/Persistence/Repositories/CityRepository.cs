using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RelationalDatabases.DataModels;
using RelationalDatabases.DataModels.QueryModels;

namespace RelationalDatabases.Persistence.Repositories
{
    public class CityRepository : BaseRepository<City, BaseQueryModel>
    {
        public CityRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        
        }

        #region GetByZip
        public async Task<City> GetByZipAsync(string zip, bool includeRelated = false)
        {
            if (!includeRelated)
                return await this.databaseContext.cities.SingleOrDefaultAsync(x => x.zip == zip);

            var query = this.databaseContext.cities.AsQueryable();

            query = this.ApplyRelations(query);

            return await query.SingleOrDefaultAsync(x => x.zip == zip);
        }
        #endregion
    }
}