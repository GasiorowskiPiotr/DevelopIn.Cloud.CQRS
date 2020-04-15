using System;

namespace DevelopIn.Cloud.CQRS.Infrastructure.RabbitMq
{
    public class RabbitMqSettings
    {
        public Uri HostAddress { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ushort Heartbeat { get; set; }
    }
}