using System.Threading.Tasks;

namespace MB.Manager.Message1.Interface.V1
{
    public interface IEchoedEvent
    {
        Task OnEchoed(EchoedEventData eventData);
    }

    public class EchoedEventData
    {
        public string Result { get; set; }
    }
}
