namespace Labo.DownloadManager.Extensions.Chrome.MessagingHost
{
    using System;
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Program
    {
        public static void Main(string[] args)
        {
            JObject data;
            while ((data = Read()) != null)
            {
                try
                {
                    string processed = ProcessMessage(data);
                    Write(processed);
                    if (processed == "exit")
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    Write("error");
                }
            }
        }

        public static string ProcessMessage(JObject data)
        {
            JToken messageData = data["message"];
            if (messageData.Value<string>() == "exit")
            {
                return "exit";
            }

            MessageBase message = messageData.Value<MessageBase>();
            switch (message.Type)
            {
                case "download":
                    {
                        Message<string> downloadMessage = messageData.Value<Message<string>>();
                        string downloadUrl = downloadMessage.Value;

                        return "Added download url: " + downloadUrl;
                    }

                case "exit":
                    return "exit";
                default:
                    return "Echo: " + message.Type;
            }
        }

        public static JObject Read()
        {
            Stream stdin = Console.OpenStandardInput();
            int length = 0;

            byte[] lengthBytes = new byte[4];
            stdin.Read(lengthBytes, 0, 4);
            length = BitConverter.ToInt32(lengthBytes, 0);

            char[] buffer = new char[length];
            using (var reader = new StreamReader(stdin))
            {
                while (reader.Peek() >= 0)
                {
                    reader.Read(buffer, 0, buffer.Length);
                }
            }

            return (JObject)JsonConvert.DeserializeObject<JObject>(new string(buffer))["data"];
        }

        public static void Write(JToken data)
        {
            JObject json = new JObject();
            json["data"] = data;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));

            Stream stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }
    }
}