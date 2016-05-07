using MyUniversity.Models.Repositories.Interface;
namespace MyUniversity.Models.Repositories
{
    public class ClockLogRepository : BaseRepository<Clocklog>, IClockLogRepository
    {
        public ClockLogRepository(UniversityEntities dbContext)
            : base(dbContext)
        { }
    }
}
