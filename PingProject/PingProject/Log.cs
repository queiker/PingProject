using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingProject
{
    class Log
    {
        public DateTime time;
        public string message;
        importance Priority;
        public enum importance 
        {
            WARNING,
            ERROR,
            INFO,
            OK,
        }
        public Log(string Message, importance priority)
        {
            message = Message;
            Priority = priority;
            time = DateTime.Now;



        }

    }
}
