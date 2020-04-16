using FluentScheduler;
using StrumentiMusicali.Library.Repo;

namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class IndexOptimizeJob : IJob
    {
        public void Execute()
        {
            using (var uof = new UnitOfWork())
            {
                uof.OptimizeIndex();
            }
        }
    }
}