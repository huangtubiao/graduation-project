using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class SuperiorLetterRepository : BaseRepository<SuperiorLetter>, ISuperiorLetterRepository
    {
        public SuperiorLetterRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
