using System;
using System.Collections.Generic;


namespace PingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome in PingProject");

            List<Device> DeviceList = new List<Device>() ;
            DeviceList.Add(new Device("127.0.0.1", "MyPc"));
            DeviceList.Add(new Device("192.168.1.99", "Wrong IP"));
            DeviceList.Add(new Device("192.168.1.1", "Router"));

            foreach(Device device in DeviceList)
            {
                device.PingDevice();
            }

            foreach (Device device in DeviceList)
            {
                device.PingDevice();
            }

            foreach (Device device in DeviceList)
            {
                Console.WriteLine("=============================");
                Console.WriteLine(device.Ip + " - " + device.Description);
                foreach(Log log in device.ListLog)
                {
                    Console.WriteLine(log.message + " - " + log.time.Hour.ToString() + ":" + log.time.Minute.ToString()+ ":" + log.time.Second.ToString());
                }
                
                
                
            }




        }
        public static void EmailLogs()
        {
            throw new NotImplementedException();
        }
        public static void SaveLogsToFile()
        {
            throw new NotImplementedException();
        }
    }
}
