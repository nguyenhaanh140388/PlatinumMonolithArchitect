using System.Threading.Tasks;

namespace Platinum.Core.Handfires
{
    public interface IJobBase
    {
        Task Excute();
    }

    public abstract class JobBase : IJobBase
    {
        public virtual async Task Excute()
        {
            await Task.FromResult<string>(null);
        }
    }
}
