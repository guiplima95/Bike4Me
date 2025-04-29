using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bike4Me.Application.Abstractions.Messaging;

public interface IApplicationEventBus : IDisposable
{
    Task PublishMessage<T>(T message) where T : Message;

    Task StartConsumer();

    Task StopConsumer();
}