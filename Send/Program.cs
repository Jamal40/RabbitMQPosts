using System;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.Port = 5672;
            factory.HostName = "45.63.116.153";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "chshja", type: ExchangeType.Fanout);

                Console.WriteLine("What do you want to do?");
                string request = Console.ReadLine();

                Console.WriteLine("What's the post id?");
                int postId = Convert.ToInt32(Console.ReadLine());

                SentData message = new SentData {PostId = postId, Message = "This is a good post from Jamal.", Operation = request };
                
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "chshja",
                                     routingKey: "",
                                     basicProperties: properties,
                                     body: body);

                Console.WriteLine(" [Jamal] Sent {0}", message);

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
