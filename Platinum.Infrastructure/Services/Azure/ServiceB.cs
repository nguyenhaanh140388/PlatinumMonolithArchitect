namespace Platinum.Infrastructure.Services.Azure
{
    public interface IServiceB { }
    public class ServiceB : IServiceB
    {
        public ServiceB(IServiceC serviceC) { }
    }
}
