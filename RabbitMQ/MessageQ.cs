using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ
{
    public static class MessageQ
    {
        //Sending a message is easy from class, but receiving give a disposed object issue.
        public static void SendMessage(string host, string queueName, string username, string password, string message)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = host, Port = 5672 };
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = "my_vhost";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, autoDelete: false, exclusive: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            channel.QueueDeclare(queue: queueName, durable: false, autoDelete: false, exclusive: false, arguments: null);
            channel.BasicPublish("", queueName, null, Encoding.UTF8.GetBytes(message));
        }
    }
}