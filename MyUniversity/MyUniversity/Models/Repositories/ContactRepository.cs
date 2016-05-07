
using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
