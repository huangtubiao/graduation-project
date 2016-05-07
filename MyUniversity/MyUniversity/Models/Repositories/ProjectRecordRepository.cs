using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class ProjectRecordRepository : BaseRepository<ProjectRecord>, IProjectRecordRepository
    {
        public ProjectRecordRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
