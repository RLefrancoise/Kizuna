using System;

namespace ChatClientExample
{
    public class ChatClientExample
    {
        private static void Main(string[] args)
        {
            try
            {
                ChatClient client = new ChatClient(args[0], int.Parse(args[1]));
                client.OnConnectionLost += () =>
                {
                    ChatClient.LogError("Connection lost");
                };
                client.Start();
            }
            catch (Exception e)
            {
                ChatClient.LogError(e.Message);
            }
        }
    }
}
