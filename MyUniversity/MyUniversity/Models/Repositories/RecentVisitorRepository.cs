using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class RecentVisitorRepository : BaseRepository<RecentVisitor>, IRecentVisitorRepository
    {
        public RecentVisitorRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
