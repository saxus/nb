using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

//#define PROFILING

namespace NBouncer.Tools
{
    [Serializable]
    public class Logger
    {
        private static object s_lock = new object();

        private string _prefix;

        private static bool _showDeveloperMessages = false;
        public static bool ShowDeveloperMessages
        {
            get { return Logger._showDeveloperMessages; }
            set { Logger._showDeveloperMessages = value; }
        }


        public bool Quiet { get; set; }


        static Logger()
        {
            ShowDeveloperMessages = true;
        }

        public Logger(string prefix)
        {

            _lastDate = DateTime.Now;
            _prefix = prefix + ": ";
            _stopWatch.Start();
            Quiet = false;
        }

        private DateTime _lastDate;

        private string GetPrefix()
        {
            if (_lastDate.Day != DateTime.Now.Day)
            {
                _lastDate = DateTime.Now;

                Write("Day changed: {0}", DateTime.Now);
            }

            return DateTime.Now.ToString() + " " + _prefix;
        }

        private static ConsoleColor PromptColor = ConsoleColor.Green;
        private static ConsoleColor TextColor = ConsoleColor.Gray;
        private static ConsoleColor TimestampColor = ConsoleColor.DarkGray;
        private static ConsoleColor DeveloperColor = ConsoleColor.DarkBlue;

        public void Write(string msg)
        {
            if (Quiet) return;

            lock (s_lock)
            {
                Console.ForegroundColor =TimestampColor;
                Console.Write(GetPrefix());
                Console.ForegroundColor = TextColor;
                Console.WriteLine(msg);
                Console.ForegroundColor = PromptColor;
            }
        }


        public void Write(string msg, object arg0)
        {
            if (Quiet) return;

            lock (s_lock)
            {
                Console.ForegroundColor =TimestampColor;
                Console.Write(GetPrefix());
                Console.ForegroundColor = TextColor;
                Console.WriteLine(msg, arg0);
                Console.ForegroundColor = PromptColor;
            }
        }

        public void Write(string msg, object arg0, object arg1)
        {
            if (Quiet) return;

            lock (s_lock)
            {
                Console.ForegroundColor =TimestampColor;
                Console.Write(GetPrefix());
                Console.ForegroundColor = TextColor;
                Console.WriteLine(msg, arg0, arg1);
                Console.ForegroundColor = PromptColor;
            }
        }


        public void Write(string msg, object arg0, object arg1, object arg2)
        {
            if (Quiet) return;

            lock (s_lock)
            {
                Console.ForegroundColor =TimestampColor;
                Console.Write(GetPrefix());
                Console.ForegroundColor = TextColor;
                Console.WriteLine(msg, arg0, arg1, arg2);
                Console.ForegroundColor = PromptColor;
            }
        }

        public void Write(string msg, object arg0, object arg1, object arg2, object arg3)
        {
            if (Quiet) return;

            lock (s_lock)
            {
                Console.ForegroundColor =TimestampColor;
                Console.Write(GetPrefix());
                Console.ForegroundColor = TextColor;
                Console.WriteLine(msg, arg0, arg1, arg2, arg3);
                Console.ForegroundColor = PromptColor;
            }
        }

        [Conditional("DEBUG")]
        public void Developer(string msg)
        {
            if (Quiet) return;
            if (!ShowDeveloperMessages) return;

            lock (s_lock)
            {
                    Console.ForegroundColor = TimestampColor;
                    Console.Write(GetPrefix());
                    Console.ForegroundColor = DeveloperColor;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = PromptColor;
            }
        }

        public void Developer(string msg, object arg0)
        {
            Developer(string.Format(msg, arg0));
        }

        public void Developer(string msg, object arg0, object arg1)
        {
            Developer(string.Format(msg, arg0, arg1));
        }

        public void Developer(string msg, object arg0, object arg1, object arg2)
        {
            Developer(string.Format(msg, arg0, arg1, arg2));
        }

        public void Developer(string msg, object arg0, object arg1, object arg2, object arg3)
        {
            Developer(string.Format(msg, arg0, arg1, arg2, arg3));
        }


        private TimeSpan _lastEvent = DateTime.Now.TimeOfDay;
        private Stopwatch _stopWatch = new Stopwatch();





        [Conditional("PROFILING")]
        public void Profile(string section)
        {
            this.Profile(section, false);
        }


        [Conditional("PROFILING")]
        public void Profile(string section, bool reset)
        {
            var now = _stopWatch.Elapsed;

            if (reset)
            {
                Console.WriteLine("{1:c} | {0}",
                    section,
                    now
                );
            }
            else
            {
                Console.WriteLine("{1:c} | {0} ({2:c})",
                    section,
                    now,
                    now - _lastEvent
                );
            }

            _lastEvent = now;
        }


        public void Exception(Exception ex)
        {
            Exception(ex, 0);
        }


        private void Exception(Exception ex, int level)
        {
            //this.Write("{1}EXCEPTION: {0}", ex.GetType().FullName, (level > 0) ? level.ToString() + ": " : "");

            lock (s_lock)
            {
                var c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Magenta;
                ExceptionHelper.WriteRecursive(ex);
                Console.ForegroundColor = c;
            }

            //Console.WriteLine(ex.Message);
            //Console.WriteLine("-[ StackTrace ]-----------------------------------");
            //Console.WriteLine(ex.StackTrace);
            //
            //if (ex.InnerException != null)
            //{
            //    Exception(ex.InnerException, level + 1);
            //}
        }
    }
}
