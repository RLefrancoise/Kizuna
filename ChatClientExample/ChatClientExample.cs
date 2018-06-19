using System;

namespace ChatClientExample
{
    public class ChatClientExample
    {
        static void Main(string[] args)
        {
            try
            {
                ChatClient client = new ChatClient(args[0], int.Parse(args[1]));
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
