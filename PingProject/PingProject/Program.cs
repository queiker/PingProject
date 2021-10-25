using System;
using System.Collections.Generic;


namespace PingProject
{
    class Program
    {
        static void Main(string[] args)
        {

            CheckArgs(args);

            
            Console.WriteLine("==========================");

            
            List<Device> DeviceList = new List<Device>();
            DeviceList.Add(new Device("127.0.0.1", "MyPc"));
            DeviceList = AddDevice(DeviceList);
            DeviceList = PingDevices(DeviceList);
            DeviceList = PingDevices(DeviceList);
            DeviceList = PingDevices(DeviceList);
            PrintLogs(DeviceList);

            string key = "";

           while(key != "q" && key != "Q")
            {
                switch (key)
                {
                    case "l":
                        ReadLogs();
                        key = PrintMainMenu();
                        continue;
                    case "L":
                        ReadLogs();
                        key = PrintMainMenu();
                        continue;
                    case "t":
                        //Test();
                        key = PrintMainMenu();
                        continue;
                    case "T":
                        //Test();
                        key = PrintMainMenu();
                        continue;
                    case "d":
                        DevicesMenu();
                        continue;

                    case "D":
                        DevicesMenu();
                        continue;
                    case "":
                        //do nothing
                        key = PrintMainMenu();
                        
                        continue;
                    default:
                        Console.WriteLine("Command not recognized.");
                        key = PrintMainMenu();
                        continue;


                }
                

            }

        }

        public static void CheckArgs(string[] Args)
        {
            foreach (string arg in Args)
            {
                Console.WriteLine(arg);
                if (arg == "/t" || arg == "/T")
                {
                    Test();
                }
            }
        }

        public static string PrintMainMenu()
        {
            Console.WriteLine("----==== Ping project! ====----");
            Console.WriteLine("Main menu : ");
            Console.WriteLine("Press l to read logs");
            Console.WriteLine("Press d to change devices");
            Console.WriteLine("Press t to test function");
            Console.WriteLine("Press q to Quit");
            return Console.ReadLine();
        }


        public static void DevicesMenu()
        {
            throw new NotImplementedException();
        }
        public static void EmailLogs()
        {
            throw new NotImplementedException();
        }
        public static void SaveLogsToFile()
        {
            throw new NotImplementedException();
        }

        public static void ReadLogs()
        {

        }

        

        public static List<Device> AddDevice(List<Device> DL)
        {

            string device_ip;
            string device_description;
            Console.WriteLine("Put device IP :");
            device_ip = Console.ReadLine();
            Console.WriteLine("Put device description :");
            device_description = Console.ReadLine();


            DL.Add(new Device(device_ip, device_description));

            return DL;

        }

        public static List<Device> PingDevices(List<Device> DL)
        {
            foreach (Device device in DL)
            {
                device.PingDevice();
            }
            return DL;
        }
        public static void PrintLogs(List<Device> DL)
        {
            

            foreach (Device device in DL)
            {
                Console.WriteLine("=============================");
                Console.WriteLine(device.Ip + " - " + device.Description);
                foreach (Log log in device.ListLog)
                {
                    Console.WriteLine(log.message + " - " + log.time.Hour.ToString() + ":" + log.time.Minute.ToString() + ":" + log.time.Second.ToString());
                }



            }


        }


        public static void Test()
        {
            Console.WriteLine("Welcome in PingProject");

            List<Device> DeviceList = new List<Device>();
            DeviceList.Add(new Device("127.0.0.1", "MyPc"));
            DeviceList.Add(new Device("192.168.1.99", "Wrong IP"));
            DeviceList.Add(new Device("192.168.1.1", "Router"));

            foreach (Device device in DeviceList)
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
                foreach (Log log in device.ListLog)
                {
                    Console.WriteLine(log.message + " - " + log.time.Hour.ToString() + ":" + log.time.Minute.ToString() + ":" + log.time.Second.ToString());
                }



            }
        }

    }
}
