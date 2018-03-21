﻿using System;

namespace UserApi.Controllers
{
    public class UserCreatedEvent : IUserEvent
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}