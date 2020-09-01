using System.Threading.Tasks;

namespace MB.Manager.Message1.Interface.V1
{
    public interface IPublishSomethingEvent
    {
        Task OnPublishSomething(PublishSomethingEventData eventData);
    }

    public class PublishSomethingEventData
    {
        public string Name { get; set; }

        public string Message { get; set; }
    }
}
