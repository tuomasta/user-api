using System;

namespace UserApi.Controllers
{
    public interface IUserEvent
    {
        Guid UserId { get; }
    }
}