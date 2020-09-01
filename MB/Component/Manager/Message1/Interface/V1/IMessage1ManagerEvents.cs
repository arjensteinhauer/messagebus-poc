namespace MB.Manager.Message1.Interface.V1
{
    public interface IMessage1ManagerEvents : IEchoedEvent, IProcessedOneWayCommandEvent, IAmAliveEvent, IPublishSomethingEvent
    {
    }
}