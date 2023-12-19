namespace Platinum.Infrastructure.Services.Azure
{
    public interface IServiceC { }
    public class ServiceC : IServiceC
    {
        public ServiceC(IServiceA serviceA) { }
    }
}
