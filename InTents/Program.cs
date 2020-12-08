using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace InTents
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        static void Main(string[] args)
        {
            
            
            synth.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Adult);
            //Speak("Welcome to InTents Performance Counter.", VoiceGender.Male);
            

            #region My Performance Counters
            //This will pull the current CPU usage in percentage
            PerformanceCounter perfCPUCount = new PerformanceCounter("Processor Information", "% Processor Time","_Total" );
            perfCPUCount.NextValue();
            //Pull available memory in megabytes
            PerformanceCounter perfMemoryCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemoryCount.NextValue();
            //Retrieves system time of power-on (Seconds)
            PerformanceCounter perfUpTimeCount = new PerformanceCounter("System", "System Up Time");
            perfUpTimeCount.NextValue();
            #endregion

            //changing up time count from seconds to something digestible
            TimeSpan systemUptimeSpan = TimeSpan.FromSeconds(perfUpTimeCount.NextValue());
            string systemUptimeCounterMessage = String.Format("The current system uptime is {0} days, {1} hours, {2} minutes, and {3} seconds",
                (int)systemUptimeSpan.TotalDays,
                (int)systemUptimeSpan.Hours,
                (int)systemUptimeSpan.Minutes,
                (int)systemUptimeSpan.Seconds);
            synth.Speak(systemUptimeCounterMessage);

            //Infinite while loop
            while (true)
            {
                float currentCPUPercentage = perfCPUCount.NextValue();
                float currentMemoryAvailable = perfMemoryCount.NextValue();
                float currentUptimeCount = perfUpTimeCount.NextValue()/3600;

                //Every 1 second print CPU load to the console
                
                Console.WriteLine("CPU Load: {0} %", currentCPUPercentage);
                Console.WriteLine("Memory Available: {0} MB", currentMemoryAvailable);
                Console.WriteLine("System Up Time: {0} hours", currentUptimeCount);

                //message to be spoken relaying current statistics
                #region
                if (currentCPUPercentage > 80) 
                {
                    if (currentCPUPercentage==100)
                    {
                        Speak("Abort! ABORT YOUR CPU IS ABOUT TO CATCH FIRE", VoiceGender.Female); 
                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", (int)currentCPUPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Female);
                    }
                    
                
                }
                if (currentMemoryAvailable < 1000)
                {
                    string memoryAvailableVocalMessage = String.Format("The current memory available is {0} megabytes", (int)currentMemoryAvailable);
                    Speak(memoryAvailableVocalMessage, VoiceGender.Female);

                }
                
                if (currentUptimeCount > 100000)
                {
                    string systemUptimeVocalMessage = String.Format("The system has been up for {0} hours", (int)currentUptimeCount);
                    Speak(systemUptimeVocalMessage, VoiceGender.Female);
                }
                #endregion

                Thread.Sleep(1000);
            } //End of loop
        }//End of main
        //speak method with selected voice
        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }
        public static void Speak(string message, VoiceGender voiceGender, int rate)

        {
            synth.Rate = 3;
            Speak(message, voiceGender);
        }
        public static void OpenWebsite(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = "https://mail.google.com/mail/u/0/?tab=rm&ogbl#inbox";
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }
    }
}
