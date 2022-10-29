using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Darth Vader is up to no good");
Console.WriteLine("Press any key to exit");

string RabbitMQServer = ConfigurationManager.AppSettings["RabbitMQServer"];
string username = ConfigurationManager.AppSettings["username"];
string password = ConfigurationManager.AppSettings["password"];
string RabbitMQQueue = "theForce";
string RabbitMQQueueRec = "theDarkForce";


//look for response
EventHandler<BasicDeliverEventArgs> consumerReceived = new EventHandler<BasicDeliverEventArgs>((model, e) => {
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");

    string ReceivedName = message.Replace("Hello my name is, ", ""); //yes, I know it is lazy.But it is a hard coded input that can me removed.

    //send response
    RabbitMQ.MessageQ.SendMessage(RabbitMQServer, RabbitMQQueueRec, username, password, $"Hello {ReceivedName}, I am your father!");
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

Console.ReadKey();
Console.WriteLine("Darth Vader melts into the darkness");
