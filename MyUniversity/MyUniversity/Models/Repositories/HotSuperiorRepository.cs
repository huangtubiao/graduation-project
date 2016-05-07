using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class HotSuperiorRepository : BaseRepository<hotSuperior>, IHotSuperiorRepository
    {
        public HotSuperiorRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
