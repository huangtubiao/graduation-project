using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class FollowsRepository : BaseRepository<Follows>, IFollowsRepository
    {
        public FollowsRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
