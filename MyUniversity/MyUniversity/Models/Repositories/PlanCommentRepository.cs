
using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class PlanCommentRepository : BaseRepository<Plancomment>, IPlanCommentRepository
    {
        public PlanCommentRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
