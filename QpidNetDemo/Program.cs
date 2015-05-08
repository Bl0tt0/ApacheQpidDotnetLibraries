using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QpidNetDemo.Connection.ApacheQpid;

namespace QpidNetDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var provider = new ApacheQpidConnectionProvider("amqp:tcp:127.0.0.1", "5672");
            try
            {
                provider.SendMessage("my test", "q1");

                provider.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                provider.Close();
            }
        }
    }
}
