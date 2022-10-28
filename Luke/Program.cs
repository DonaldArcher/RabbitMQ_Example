using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml.Linq;

Console.WriteLine("Luke has entered the building");
Console.WriteLine("type 'exit' to close application");

string RabbitMQServer = "192.168.0.131";
string RabbitMQQueue = "theForce";
string RabbitMQQueueRec = "theDarkForce";
string username = "QueueUser";
string password = "QueueUser";

//look for response
EventHandler<BasicDeliverEventArgs> consumerReceived = new EventHandler<BasicDeliverEventArgs>((model, e) => {
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");
});


ConnectionFactory factory = new ConnectionFactory { HostName = RabbitMQServer, Port = 5672 };
factory.UserName = username;
factory.Password = password;
factory.VirtualHost = "my_vhost";
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: RabbitMQQueueRec, durable: false, autoDelete: false, exclusive: false, arguments: null);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += consumerReceived;
channel.BasicConsume(queue: RabbitMQQueueRec, autoAck: true, consumer: consumer);

//RabbitMQ.MessageQ.ReceiveMessage(RabbitMQServer, RabbitMQQueueRec, username, password, consumerReceived);


//Loop and wait for name
while (true)
{
    var Name = Console.ReadLine();
    if (Name.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    RabbitMQ.MessageQ.SendMessage(RabbitMQServer, RabbitMQQueue, username, password, $"Hello my name is, {Name}");
}

Console.WriteLine("Luke has left the building");
