using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml.Linq;

namespace TestingTheForce
{
    public class ForceTester
    {
        string RabbitMQServer = "192.168.0.131";
        string RabbitMQQueue = "TestQueue";
        string username = "QueueUser";
        string password = "QueueUser";
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void AccessToTheForce()
        {
            string sentmessage = DateTime.Now.ToString();

            EventHandler<BasicDeliverEventArgs> consumerReceived = new EventHandler<BasicDeliverEventArgs>((model, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if(message == sentmessage) Assert.Pass();
                else Assert.Fail();
            });


            ConnectionFactory factory = new ConnectionFactory { HostName = RabbitMQServer, Port = 5672 };
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = "my_vhost";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: RabbitMQQueue, durable: false, autoDelete: false, exclusive: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += consumerReceived;
            channel.BasicConsume(queue: RabbitMQQueue, autoAck: true, consumer: consumer);



            RabbitMQ.MessageQ.SendMessage(RabbitMQServer, RabbitMQQueue, username, password, sentmessage);


        }
    }
}