using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class Net
    {
        public static Message receiveMessage(Stream s)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (Message)bf.Deserialize(s);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        public static void sendMessage(Stream s, Message msg)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, msg);
        }

    }
}
