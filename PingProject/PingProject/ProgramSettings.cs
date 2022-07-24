using System;
using System.Net.Mail;
using System.Net;
using System.IO;



namespace PingProject
{
    class ProgramSettings
    {
        public string email_addres;
        public string email_password;
        public string email_smtp_host;
        public int email_smtp_port;
        public string email_title;
        public string email_message;
        public bool email_enable_ssl;
        public int email_timeout ;
        public int automatic_ping_delay;

        public ProgramSettings()
        {
            email_addres = "tradecomp.pl@gmail.com";
            email_password = "$ECRET_PASSWORD_TO_CHANGE";
            email_smtp_host = "smtp.gmail.com";
            email_smtp_port = 587;
            email_title = "";
            email_message = "";
            email_enable_ssl = true;
            email_timeout = 5000;
            automatic_ping_delay = 1000;



    }

        public void Email(string Title, string Message)
        {
            /*
             * Żeby wysyłanie email działało na koncie gmail.com musi być wyłączona weryfikacja dwuetapowa
             * i włączone "dostęp mniej bezpiecznych aplikacji" 
             */
            
            var client = new SmtpClient( email_smtp_host, email_smtp_port)
            {
                
                Credentials = new NetworkCredential(email_addres, email_password),
                EnableSsl = email_enable_ssl,
                Timeout = email_timeout,
            };

            try
            {
                client.Send("tradecomp.pl@gmail.com", "tradecomp.pl@gmail.com", Title, Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Logs was sended at email");
        }




        public ProgramSettings LoadProgramSettings()
        {
            string folder = System.IO.Directory.GetCurrentDirectory();
            string path = @"\ProgramSettings.json";
            string str_device = "";
            ProgramSettings programSettings;

            try
            {
                using (StreamReader reader = new StreamReader(folder + path))
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        str_device += line;
                    }
                programSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ProgramSettings>(str_device);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Error reading from {0}. Message = {1}", path, e.Message);
            }
            finally
            {
                programSettings = new ProgramSettings();
            }

            return programSettings;
        }

        public void SaveProgramSettings(ProgramSettings PS)
        {
            string str_device = Newtonsoft.Json.JsonConvert.SerializeObject(PS);
            string folder = System.IO.Directory.GetCurrentDirectory();
            string path = @"\ProgramSettings.json";
            using (StreamWriter file = new StreamWriter(folder + path))
            {
                file.Write(str_device);
            }

        }


    }
}




