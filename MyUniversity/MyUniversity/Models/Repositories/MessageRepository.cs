using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
