using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class DepartRepository : BaseRepository<Depart>, IDepartRepository
    {
        public DepartRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
