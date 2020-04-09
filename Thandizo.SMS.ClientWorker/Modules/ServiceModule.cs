using Autofac;
using Thandizo.Messaging.Core;
using Thandizo.Messaging.SMS;

namespace Thandizo.SMS.ClientWorker.Modules
{
    public class ServiceModule : Module
    {

        private readonly string _baseUrl;
        public ServiceModule(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsMessagingService>()
                .As<IMessagingService>()
                 .WithParameter("baseUrl", _baseUrl)
                 .WithParameter("sender", "Khusa");
        }
    }
}
