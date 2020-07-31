using System;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Assumes the task is of type Task<Response<T>>
        /// Becuase the task is an untyped variable, we use reflection to obtain the message value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static T GetResponseMessage<T>(this Task task)
        {
            // get the masstransit responseobject
            var response = task.GetType().GetProperty("Result").GetValue(task);
            // get the actual message from response object
            T responseMessage = (T)response.GetType().GetProperty("Message").GetValue(response);
            return responseMessage;
        }

        public static Type GetResponseMessageType(this Task task)
        {
            return task.GetType().GenericTypeArguments[0].GenericTypeArguments[0];
        }
    }
}