using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Orchestrator
{
    public partial class MailService : ServiceBase
    {
        private Timer timer1= new Timer();
        int getCallType;

        public MailService()
        {
            InitializeComponent();
            Scheduler();
        }

        protected override void OnStart(string[] args)
        {
            timer1.AutoReset = true;
            timer1.Enabled = true;
            ServiceLog.WriteErrorLog("Daily Reporting service started");
        }

        protected override void OnStop()
        {
            timer1.AutoReset = false;
            timer1.Enabled = false;
            ServiceLog.WriteErrorLog("Daily Reporting service stopped");
        }

        public void Scheduler()
        {
            //InitializeComponent();
            int strTime = Properties.Settings.Default.callDuration;
            getCallType = Properties.Settings.Default.callType;
            if (getCallType == 1)
            {
                timer1 = new Timer();
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
            else
            {
                timer1 = new Timer();
                timer1.Interval = strTime * 1000;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
        }

        private double GetNextInterval()
        {
            string timeString = Properties.Settings.Default.StartTime;
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            int x;
            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddDays(1) - System.DateTime.Now;//Here you can increase the timer interval based on your requirments.   
            }
            return ts.TotalMilliseconds;
        }

        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            string Msg = "Hi ! This is DailyMailSchedulerService mail.";//whatever msg u want to send write here.  
                                                                        // Here you can write the   
            ServiceLog.SendEmail("lorenzoac88@gmail.com", "", "", "Daily Report of DailyMailSchedulerService on " + DateTime.Now.ToString("dd-MMM-yyyy"), Msg);

            if (getCallType == 1)
            {
                timer1.Stop();
                System.Threading.Thread.Sleep(1000000);
                SetTimer();
            }
        }
    }
}
