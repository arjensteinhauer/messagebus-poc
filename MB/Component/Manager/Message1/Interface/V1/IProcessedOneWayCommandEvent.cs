using System.Threading.Tasks;

namespace MB.Manager.Message1.Interface.V1
{
    public interface IProcessedOneWayCommandEvent
    {
        Task OnProcessedOneWayCommand(ProcessedOneWayCommandEventData eventData);
    }

    public class ProcessedOneWayCommandEventData
    {
        public string Result { get; set; }
    }
}
