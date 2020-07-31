using MB.Manager.Message2.Service.V1;

namespace MB.Microservice.Message2.WebJob.Factory.V1
{
    public interface IMessage2ManagerFactory
    {
        Message2Manager Create();
    }
}
