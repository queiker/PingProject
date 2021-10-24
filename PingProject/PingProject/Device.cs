using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace PingProject
{
    class Device
    {
        public string Ip;
        public string Description;
        public List<Log> ListLog = new List<Log>();
        public Device(string ip_addres, string description)
        {
            Ip = ip_addres;
            Description = description;

        }
        public void PingDevice()
        {
            

            Ping ping = new Ping();
            PingReply PR = ping.Send(Ip);
            Console.WriteLine("=========================================");
            Console.WriteLine(Ip + " - " + Description);
            Console.WriteLine(PR.Status.ToString());

            AddLog(PR.Status.ToString());

        }
        

        public void AddLog(string message)
        {
            
            ListLog.Add(new Log(message, Log.importance.OK));



        }

        

        
        public void ClearLogList()
        {
            throw new NotImplementedException();
        }
    }
}
