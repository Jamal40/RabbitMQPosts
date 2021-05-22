using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using Receive.Data;
using Send;
using Newtonsoft.Json;
using System.Linq;

namespace Receive
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
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.ExchangeDeclare(exchange: "chshja", type: ExchangeType.Fanout);
                    var queueName = channel.QueueDeclare("Shawqy");
                    channel.QueueBind("Shawqy",
                                      exchange: "chshja",
                                      routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        SentData sentMessage = JsonConvert.DeserializeObject<SentData>(message);
                        Post post = new Post {ID = sentMessage.PostId, Description = sentMessage.Message };
                        Event _event = new Event { Opertation = sentMessage.Operation };

                        Console.WriteLine($"We received this message: {post.Description} \nThe operation is: {_event.Opertation}");

                        var db = new PostContext();

                        switch (_event.Opertation.ToLower())
                        {
                            case "delete":
                                Console.WriteLine("The post has been deleted");
                                var deletedPost = db.Posts.Where(p => p.ID == post.ID).FirstOrDefault();
                                db.Posts.Remove(deletedPost);
                                break;

                            case "update":
                                Console.WriteLine("The post has been updated");
                                db.Posts.Update(post);
                                break;

                            case "add":
                                db.Posts.Add(post);
                                break;
                        }

                        db.Events.Add(_event);
                        db.SaveChanges();

                        Console.WriteLine("Here are all the posts:");
                        foreach (var _post in db.Posts)
                        {
                            Console.WriteLine(post.Description);
                        }

                        Console.WriteLine("Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
