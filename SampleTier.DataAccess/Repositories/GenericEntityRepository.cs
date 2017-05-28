using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampleTier.API.Models;

namespace SampleTier.DataAccess.Repositories
{
    public class GenericEntityRepository<TEntity> : EntityRepositoryBase<TEntity> where TEntity : class, new()
    {
		public GenericEntityRepository(AngularContext context) : base(context)
		{ }
	}
}