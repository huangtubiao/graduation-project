
using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class PlanComeOnRepository : BaseRepository<PlanComeOn>, IPlanComeOnRepository
    {
        public PlanComeOnRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
