using Platinum.Infrastructure.Services.Azure;
using System.Collections.Concurrent;

namespace Platinum.Infrastructure.Services
{
    public class AzureServiceBusFactory : IMessageBusFactory
    {
        private readonly object _lockObject = null;
        private readonly ConcurrentDictionary<string, ServiceBusClient> _clients = new ConcurrentDictionary<string, ServiceBusClient>();
        private readonly ConcurrentDictionary<string, ServiceBusSender> _senders = new ConcurrentDictionary<string, ServiceBusSender>();
        private readonly ConcurrentDictionary<string, ServiceBusAdministrationClient> _adminClients = new ConcurrentDictionary<string, ServiceBusAdministrationClient>();
        private readonly ConcurrentDictionary<string, QueueClient> _queueClient = new ConcurrentDictionary<string, QueueClient>();

        public AzureServiceBusFactory() { }

        public IQueueBus GetAdminClient(string connectionString)
        {
            var key = $"{connectionString}";

            lock (_lockObject)
            {
                if (ClientDoesntExistOrIsClosed(connectionString))
                {
                    var client = new ServiceBusAdministrationClient(connectionString, new ServiceBusAdministrationClientOptions
                    {

                    });

                    _adminClients[key] = client;
                }

                return ServiceBusAdministrationService.Create(_adminClients[key]);
            }
        }

        private bool ClientDoesntExistOrIsClosed(string connectionString)
        {
            return !_clients.ContainsKey(connectionString) || _clients[connectionString].IsClosed;
        }

        private bool QueueClienDoesntExistOrIsClosed(string connectionString)
        {
            return !_queueClient.ContainsKey(connectionString) || !_queueClient[connectionString].Exists();
        }

        public async Task<IMessageBus> GetClient(string connectionString, string senderName)
        {
            var key = $"{connectionString}-{senderName}";
            IMessageBus service = null;

            if (_senders.ContainsKey(key) && !_senders[key].IsClosed)
            {
                service = AzureServiceBusService.Create(_senders[key]);
            }

            var client = GetServiceBusClient(connectionString);

            lock (_lockObject)
            {
                if (_senders.ContainsKey(key) && _senders[key].IsClosed)
                {
                    service = AzureServiceBusService.Create(_senders[key]);
                }

                var sender = client.CreateSender(senderName);

                _senders[key] = sender;
            }

            service = AzureServiceBusService.Create(_senders[key]);
            var options = new ServiceBusProcessorOptions
            {
                // By default after the message handler returns, the processor will complete the message
                // If I want more fine-grained control over settlement, I can set this to false.
                AutoCompleteMessages = false,

                // I can also allow for multi-threading
                MaxConcurrentCalls = 2
            };

            try
            {
                // create a processor that we can use to process the messages
                ServiceBusProcessor processor = _clients[connectionString].CreateProcessor(senderName, options);

                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();
            }
            catch (Exception ex)
            {

            }

            return service;

        }

        public IQueueStorage GetQueueClient(string connectionString, string senderName)
        {
            var key = $"{connectionString}";

            lock (_lockObject)
            {
                if (QueueClienDoesntExistOrIsClosed(connectionString))
                {
                    var client = new QueueClient(connectionString, senderName, new QueueClientOptions()
                    {
                        MessageEncoding = QueueMessageEncoding.Base64
                    });

                    _queueClient[key] = client;
                }

                return AzureQueueStorageService.Create(_queueClient[key]);
            }
        }

        protected virtual ServiceBusClient GetServiceBusClient(string connectionString)
        {
            var key = $"{connectionString}";

            lock (_lockObject)
            {
                if (ClientDoesntExistOrIsClosed(connectionString))
                {
                    var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
                    {
                        TransportType = ServiceBusTransportType.AmqpTcp
                    });

                    _clients[key] = client;
                }

                // Processor
                var options = new ServiceBusProcessorOptions
                {
                    // By default after the message handler returns, the processor will complete the message
                    // If I want more fine-grained control over settlement, I can set this to false.
                    AutoCompleteMessages = false,

                    // I can also allow for multi-threading
                    MaxConcurrentCalls = 2
                };


                return _clients[key];
            }
        }

        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Message received from processor: {body}");

            // we can evaluate application logic and use that to determine how to settle the message.
            await args.CompleteMessageAsync(args.Message);
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // the error source tells me at what point in the processing an error occurred
            Console.WriteLine(args.ErrorSource);
            // the fully qualified namespace is available
            Console.WriteLine(args.FullyQualifiedNamespace);
            // as well as the entity path
            Console.WriteLine(args.EntityPath);
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
