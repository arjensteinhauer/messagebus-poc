using MB.Manager.Message1.Service.V1;

namespace MB.Microservice.Message1.WebJob.Factory.V1
{
    public interface IMessage1ManagerFactory
    {
        Message1Manager Create();
    }
}
