using Autofac;
using Thandizo.Messaging.Core;
using Thandizo.Messaging.SMS;

namespace Thandizo.SMS.ClientWorker.Modules
{
    public class ServiceModule : Module
    {

        private readonly string _baseUrl;
        private readonly string _sender;

        public ServiceModule(string baseUrl, string sender)
        {
            _baseUrl = baseUrl;
            _sender = sender;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsMessagingService>()
                .As<IMessagingService>()
                 .WithParameter("baseUrl", _baseUrl)
                 .WithParameter("sender", _sender);
        }
    }
}
