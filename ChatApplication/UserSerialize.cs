using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication
{
    public class UserSerialize
    {
        private readonly List<User> users;

        public UserSerialize(List<User> users)
        {
            this.users = users;
        }

        public void SerializeNow()
        {
            FileStream fs = new FileStream("users.txt", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, users);
            fs.Close();
        }
        public List<User> DeSerializeNow()
        {
            FileStream fs = new FileStream("users.txt", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<User> DeserializedUsers = (List<User>)bf.Deserialize(fs);

            fs.Close();
            return DeserializedUsers;
        }
    }
}
