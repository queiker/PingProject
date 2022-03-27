using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace PingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            

            CheckArgs(args);
            Console.WriteLine("==========================");
            List<Device> DeviceList = new List<Device>();
            //DeviceList.Add(new Device("127.0.0.1", "localhost"));
            //DeviceList = AddDevice(DeviceList);  adding device with name and ip from user
            //DeviceList = PingDevices(DeviceList);

            //PrintLogs(DeviceList);

            string key = "";

            while (key != "q" && key != "Q")
            {
                key = PrintMainMenu();
                switch (key)
                {
                    case "p" or "P":
                        DeviceList = PingDevices(DeviceList);
                        continue;

                    case "c" or "C":
                        DeviceList = CreateDevice(DeviceList);
                        continue;
                    case "r" or "R":
                        ReadDevice(DeviceList);

                        continue;
                    case "u" or "U":
                        UpdateDevice(DeviceList);
                        continue;
                    case "d" or "D":
                        DeviceList = DeleteDevice(DeviceList);
                        continue;

                    case "save" or "SAVE":
                        SaveDeviceList(DeviceList);
                        continue;
                    
                    case "load" or "LOAD":
                        DeviceList = LoadDeviceList();
                        continue;

                    case "l" or "L":
                        PrintLogs(DeviceList);
                        continue;
                    case "carline" or "CARLINE":
                        PingCarlineDevices();
                        continue;
                    
                    case "testemail" or "TESTEMAIL":
                        Email("PingProject LOG", "Test email Log OK");
                        continue;

                    case "e" or "E":
                        EmailLogs(DeviceList);
                        continue;


                    case "":
                        //do nothing
                        continue;
                    default:
                        Console.WriteLine("Command not recognized.");
                        continue;
                }
            }


        }
    
        public static void CheckArgs(string[] Args)
        {
            foreach (string arg in Args)
            {
                Console.WriteLine(arg);
                if (arg == "/carline" || arg == "/CARLINE")
                {
                    PingCarlineDevices();

                }
                    
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
            Console.WriteLine("Press P to ping");
            //CRUD
            Console.WriteLine("Press C to create / add devices");
            Console.WriteLine("Press R to read devices");
            Console.WriteLine("Press U to update devices");
            Console.WriteLine("Press D to delete devices");

            Console.WriteLine("Press E to email logs");
            Console.WriteLine("Press TESTEMAIL to test email");
            Console.WriteLine("Press CARLINE to ping carline devices");

            //Save and Load
            Console.WriteLine("Press save to save device list");
            Console.WriteLine("Press load to load device list");
            
            Console.WriteLine("Press L to read logs");
            Console.WriteLine("Press Q to quit");
            return Console.ReadLine();
        }

        public static void SaveDeviceList(List<Device> DL)
        {
            string str_device = Newtonsoft.Json.JsonConvert.SerializeObject(DL);
            string folder = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine(str_device);
            string path = @"\DevicesList.json";
            using (StreamWriter file = new StreamWriter(folder + path))
            {
                file.Write(str_device);
            }
 
        }
        public static List<Device> LoadDeviceList()
        {
            string folder = System.IO.Directory.GetCurrentDirectory();
            string path = @"\DevicesList.json";
            //string str_device = Newtonsoft.Json.JsonConvert.SerializeObject(DL);
            string str_device = "";

            using (StreamReader reader = new StreamReader(folder + path))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    str_device += line;
                }
            }
            Console.Write(str_device);
            List<Device> DL = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(str_device);
            return DL;

        }
        
        public static void Email(string Title, string Message)
        {
            /*
             * Żeby wysyłanie email działało na koncie gmail.com musi być wyłączona weryfikacja dwuetapowa
             * i włączone "dostęp mniej bezpiecznych aplikacji" 
             */
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                //TODO: WPISAĆ POPRAWNE HASŁO
                Credentials = new NetworkCredential("tradecomp.pl@gmail.com", "Tajne_HASŁO"),
                EnableSsl = true,
                Timeout = 5000,

            };
            
            try
            {
                client.Send("tradecomp.pl@gmail.com", "tradecomp.pl@gmail.com", Title, Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Logs was sended at email");
        }
        public static void SaveLogsToFile()
        {
            throw new NotImplementedException();
        }

        public static List<Device> CreateDevice(List<Device> DL)
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

        public static void ReadDevice(List<Device> DL)
        {
            int i = 1;
            foreach (Device dev in DL)
            {
                Console.Write(i);
                Console.Write(") ");
                Console.Write(dev.Ip); 
                Console.Write(" => "); 
                Console.WriteLine(dev.Description);
                i++;
            }
        }
        public static void UpdateDevice(List<Device> DL)
        {
            ReadDevice(DL);
            Console.WriteLine("Put number of device to update:");
            int device_nr;
            string str_device_nr = Console.ReadLine() ;
            device_nr = Convert.ToInt32(str_device_nr);

            Console.Write(DL[device_nr - 1].Ip); 
            Console.Write(" => "); 
            Console.WriteLine(DL[device_nr - 1].Description);

            Console.WriteLine("Put device IP :");
            DL[device_nr - 1].Ip = Console.ReadLine();
            Console.WriteLine("Put device description :");
            DL[device_nr - 1].Description = Console.ReadLine();
 
        }
        public static List<Device> DeleteDevice(List<Device> DL)
        {
            ReadDevice(DL);
            Console.WriteLine("Put number of device to delete:");
            int device_nr;
            string str_device_nr = Console.ReadLine();
            device_nr = Convert.ToInt32(str_device_nr);
            DL.RemoveAt(device_nr - 1);
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

        public static void PingCarlineDevices()
        {
            List<Device> DL = new List<Device>();
            DL.Add(new Device("192.168.1.14", "PLOTER HP"));
            DL.Add(new Device("192.168.1.4", "DRUKARKA KONIKA"));
            //DL.Add(new Device("127.0.0.1", "PĘTLA LOKALNA"));
            

            /*
            DL.Add(new Device("127.0.0.1", "PĘTLA LOKALNA"));
            DL.Add(new Device("192.168.1.13", "DRUKARKA OKI"));
            */


            while(PingError(DL) == false)
            {
                DL = PingDevices(DL);
                Thread.Sleep(1800000);

            }
            EmailLogs(DL);

        }

        public static bool PingError(List<Device> DL)
        {
            bool ToReturn = false;
            foreach(Device device in DL)
            {
                foreach (Log log in device.ListLog)
                {
                    // Success
                    if ((log.message == "DestinationHostUnreachable") || (log.message == "TimedOut"))
                    {
                        ToReturn = true;
                    }
                }
                
            }
            return ToReturn;
        }

        public static void EmailLogs(List<Device> DL)
        {
            string logs = "";
            foreach (Device device in DL)
            {
                logs += "=============================\n";
                logs += device.Ip + " - " + device.Description + "\n";
                foreach (Log log in device.ListLog)
                {
                    logs += log.message + " - " + log.time.Hour.ToString() + ":" + log.time.Minute.ToString() + ":" + log.time.Second.ToString() + "\n" ;
                }
            }
            Email("LOGI CARLINE", logs);
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
