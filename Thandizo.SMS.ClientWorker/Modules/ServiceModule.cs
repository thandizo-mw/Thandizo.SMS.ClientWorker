using Autofac;
using Thandizo.DataModels.SMS;
using Thandizo.Messaging.Core;
using Thandizo.Messaging.SMS;

namespace Thandizo.SMS.ClientWorker.Modules
{
    public class ServiceModule : Module
    {
        private readonly SmsConfiguration _smsConfiguration;

        public ServiceModule(SmsConfiguration smsConfiguration)
        {
            _smsConfiguration = smsConfiguration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsMessagingService>()
                .As<IMessagingService>()
                 .WithParameter("smsConfiguration", _smsConfiguration);
        }
    }
}
