using UserApi.Controllers;
using System.Threading.Tasks;

namespace UserApi.Messaging
{
    public interface IMessageBus
    {
        Task SendEventAsync(IUserEvent @event);
    }
}