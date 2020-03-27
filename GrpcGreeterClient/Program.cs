using Grpc.Core;
using Grpc.Net.Client;
using GrpcService_test;
using System;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    class Program
    {
        const string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1ODUzMDA0NzksImV4cCI6MTU4NTkwNTI" +
            "3OSwiaWF0IjoxNTg1MzAwNDc5fQ.9bPvZAwD0JxUmFRmqHMnAohb037gveomSe--ihuD_EQ";
        static async Task Main(string[] args)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_token}");
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" },headers);
            Console.WriteLine("Greeting: " + reply.Message);

            var authenticatedChannel = CreateAuthenticatedChannel("https://localhost:5001");
            var authenticatedClient = new Greeter.GreeterClient(authenticatedChannel);
            var response = await authenticatedClient.SayHelloAsync(new HelloRequest { Name = "Greeter by Authenticated Client" });
            Console.WriteLine("Greeting: " + response.Message);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /*Configuring ChannelCredentials on a channel is an alternative way to send the token to the service with gRPC calls. 
          The credential is run each time a gRPC call is made, which avoids the need to write code in multiple places to pass the token yourself.
        */
        private static GrpcChannel CreateAuthenticatedChannel(string address)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
    }
}
