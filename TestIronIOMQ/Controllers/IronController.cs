using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using io.iron.ironmq;
using io.iron.ironmq.Data;
using System.Text;

namespace TestIronIOMQ.Controllers
{
    public class IronController : Controller
    {
        private static Client GetIronMQClient()
        {
            //put here for simplicity 
            string IronMQHost = System.Configuration.ConfigurationManager.AppSettings.Get("IronMQHost");
            string IronMQProjectId = System.Configuration.ConfigurationManager.AppSettings.Get("IronMQProjectId");
            string IronMQToken = System.Configuration.ConfigurationManager.AppSettings.Get("IronMQToken");

            if (string.IsNullOrEmpty(IronMQHost))
            {
                return new Client(IronMQProjectId, IronMQToken);   // defualt Host and Port            
            }
            else
            {
                return new Client(IronMQProjectId, IronMQToken, IronMQHost);   // specific Host and Port            
            }
        }

        //
        // GET: /Iron/Publish
        public string Publish()
        {
            Client client = GetIronMQClient();
            Queue queue = client.queue("test_queue");
            string msg = "Hello, world! " + DateTime.Now;
            queue.push(msg);
            return "Published Msg: " + msg;
        }

        // GET: /Iron/Get
        public string Get()
        {
            Client client = GetIronMQClient();
            Queue queue = client.queue("test_queue");
            Message msg = queue.get();
            if (msg == null)
            {
                return "Got Msg: NULL";
            }
            else
            {
                return "Got Msg: " + msg.Body;
            }
        }

        // GET: /Iron/Delete
        public string Delete()
        {
            Client client = GetIronMQClient();
            Queue queue = client.queue("test_queue");
            Message msg = queue.get();
            if (msg == null)
            {
                return "Deleted Msg: NULL";
            }
            else
            {
                queue.deleteMessage(msg);
                return "Deleted Msg: " + msg.Body;
            }
        }

        // GET: /Iron/GetMessages
        public string GetMessages()
        {
            Client client = GetIronMQClient();
            Queue queue = client.queue("test_queue");
            IList<Message> messages = queue.get(100);
            if (messages == null || messages.Count() == 0)
            {
                return "No Messages to Get";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Messages: <br />");
                foreach (Message msg in messages)
                {
                    sb.Append(msg.Body);
                    sb.Append("<br />");
                }
                return sb.ToString();
            }

        }
    }
}
