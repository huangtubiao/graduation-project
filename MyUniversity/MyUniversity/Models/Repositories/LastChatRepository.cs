using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class LastChatRepository : BaseRepository<LastChat>, ILastChatRepository
    {
        public LastChatRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
