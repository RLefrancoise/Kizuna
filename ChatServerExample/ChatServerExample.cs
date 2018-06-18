namespace ChatServerExample
{
    public class ChatServerExample
    {
        static int Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();
            return 0;
        }
    }
}
