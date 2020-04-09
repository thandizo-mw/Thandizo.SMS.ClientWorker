using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Thandizo.DataModels.Contracts;
using Thandizo.Messaging.Core;

namespace Thandizo.SMS.ClientWorker.Consumers
{
    public class SmsConsumer : IConsumer<IMessageModelRequest>
    {
        private readonly IMessagingService _service;

        public SmsConsumer(IMessagingService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<IMessageModelRequest> context)
        {
            var request = context.Message;
            var response = await _service.SendMessage(request.Message);
            await context.RespondAsync(response);
        }
    }
}
