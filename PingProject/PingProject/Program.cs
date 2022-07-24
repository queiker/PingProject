using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace PingProject
{
    class Program
    {
        

        public static void Main(string[] args)
        {
           

            //if CheckArgs false it mean that there was no arguments
            if (!CheckArgs(args))
            {
                InteractiveMenu();
            }
        }

        public static void InteractiveMenu()
        {
            Console.WriteLine("==========================");
            List<Device> DeviceList = new List<Device>();
            ProgramSettings programSettings = new ProgramSettings();

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

                    case "savedevicelist" or "SAVEDEVICELIST":
                        SaveDeviceList(DeviceList);
                        continue;

                    case "loaddevicelist" or "LOADDEVICELIST":
                        DeviceList = LoadDeviceList();
                        continue;

                    case "l" or "L":
                        PrintLogs(DeviceList);
                        continue;
                    case "automatic" or "AUTOMATIC":
                        PingDevicesAutomatically();
                        continue;

                    case "testemail" or "TESTEMAIL":
                        
                        programSettings.Email("PingProject LOG", "Test email Log OK");


                        continue;

                    case "e" or "E":
                        EmailLogs(DeviceList, programSettings);
                        continue;
                    
                    case "loadsettings" or "LOADSETTINGS":
                        programSettings = programSettings.LoadProgramSettings();
                            
                        continue;
                    
                    case "savesettings" or "SAVESETTINGS":
                        programSettings.SaveProgramSettings(programSettings);

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
    
        public static bool CheckArgs(string[] Args)
        {
            bool to_return = false;

            foreach (string arg in Args)
            {
                to_return = true;
                Console.WriteLine(arg);
                if (arg == "/automatic" || arg == "/AUTOMATIC" || arg == "/carline" || arg == "/CARLINE")
                {
                    PingDevicesAutomatically();
                }
                    
                if (arg == "/t" || arg == "/T")
                {
                    Test();
                }
            }
            return to_return;
        }
        public static string PrintMainMenu()
        {
            Console.WriteLine("----==== Ping project! ====----");
            Console.WriteLine("Main menu : ");
            Console.WriteLine("Put P to ping");
            //CRUD
            Console.WriteLine("Put C to create / add devices");
            Console.WriteLine("Put R to read devices");
            Console.WriteLine("Put U to update devices");
            Console.WriteLine("Put D to delete devices");

            Console.WriteLine("Put E to email logs");
            Console.WriteLine("Put TESTEMAIL to test email");
            Console.WriteLine("Put AUTOMATIC to load, constantly ping and email logs with error");

            //Save and Load
            Console.WriteLine("Put savedevicelist to save device list");
            Console.WriteLine("Put loaddevicelist to load device list");
            
            //save and load settings
            Console.WriteLine("Put savesettings to save program settings");
            Console.WriteLine("Put loadsettings to load program settings");
            
            Console.WriteLine("Put L to read logs");
            Console.WriteLine("Put Q to quit");
            return Console.ReadLine();
        }

        public static void SaveDeviceList(List<Device> DL)
        {
            string str_device = Newtonsoft.Json.JsonConvert.SerializeObject(DL);
            string folder = System.IO.Directory.GetCurrentDirectory();
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

        public static void PingDevicesAutomatically()
        {
            ProgramSettings ps = new ProgramSettings();
            ps = ps.LoadProgramSettings();

            List<Device> dl = LoadDeviceList();
            while(PingError(dl) == false)
            {
                dl = PingDevices(dl);
                Thread.Sleep(ps.automatic_ping_delay);
            }
            
            EmailLogs(dl,ps);
            

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

        public static void EmailLogs(List<Device> DL, ProgramSettings ps)
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
            ps.Email("PingProject Logs", logs);
            
            
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
