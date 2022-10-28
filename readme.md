Example runs on my internal server.

Server: ubunto with docker

Install RabbitMQ: sudo docker run -d -p 5672:5672 -p 15672:15672 -p 5671:5671  -p 4369:4369  -p 55672:55672  -p 35197:35197 --hostname my-rabbit --name some-rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password -e RABBITMQ_DEFAULT_VHOST=my_vhost rabbitmq:3-management

Added user: QueueUser, with password: QueueUser

Trying this yourself:
update the RabbitMQServer to point to your rabbitMQ installation.
Assuming you are using default ports.


