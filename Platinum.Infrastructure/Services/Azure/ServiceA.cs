namespace Platinum.Infrastructure.Services.Azure
{
    public interface IServiceA { }
    public class ServiceA : IServiceA
    {
        public ServiceA(IServiceB serviceB) { }
    }
}
