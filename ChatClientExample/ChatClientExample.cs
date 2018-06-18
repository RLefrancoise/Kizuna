using System;

namespace ChatClientExample
{
    public class ChatClientExample
    {
        static void Main(string[] args)
        {
            try
            {
                ChatClient client = new ChatClient("fe80::ca:6225:22f1:270d%3", 11000);
                client.OnConnectionLost += () =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Connection lost");
                    Console.ResetColor();
                };
                client.Start();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}
