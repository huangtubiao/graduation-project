using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class SuperviceRepository : BaseRepository<Supervice>, ISuperviceRepository
    {
        public SuperviceRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
