using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class AnswerRepository : BaseRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
