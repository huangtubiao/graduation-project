using MyUniversity.Models.Repositories.Interface;

namespace MyUniversity.Models.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
