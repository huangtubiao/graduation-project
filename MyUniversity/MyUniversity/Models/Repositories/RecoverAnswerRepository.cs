using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class RecoverAnswerRepository : BaseRepository<RecoverAnswer>, IRecoverAnswerRepository
    {
        public RecoverAnswerRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
