using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Thandizo.SMS.ClientWorker.Consumers;

namespace Thandizo.SMS.ClientWorker.Modules
{
    public class ConsumersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsConsumer>();
        }
    }
}
