﻿using System.Threading.Tasks;

namespace MB.Manager.Message1.Interface.V1
{
    public interface IAmAliveEvent
    {
        Task OnAmAlive(AmAliveEventData eventData);
    }

    public class AmAliveEventData
    {
        public string Message { get; set; }
    }
}
