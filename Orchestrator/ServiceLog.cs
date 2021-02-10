using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using PK.LogParsing;

namespace Orchestrator
{
    public static class ServiceLog
    {
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        /// <summary>  
        /// this function write Message to log file.  
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        #region Send Email Code Function  
        /// <summary>  
        /// Send Email with cc bcc with given subject and message.  
        /// </summary>  
        /// <param name="ToEmail"></param>  
        /// <param name="cc"></param>  
        /// <param name="bcc"></param>  
        /// <param name="Subj"></param>  
        /// <param name="Message"></param>  
        public static void SendEmail(String ToEmail, string cc, string bcc, String Subj, string Message)
        {
            //Reading sender Email credential from web.config file  

            string HostAdd = Properties.Settings.Default.Host;
            string FromEmailid = Properties.Settings.Default.FromMail;
            string Pass = Properties.Settings.Default.Password;

            //creating the object of MailMessage  
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
            mailMessage.Subject = Subj; //Subject of Email  
            mailMessage.Body = Message; //body or message of Email  
            mailMessage.IsBodyHtml = true;

            string[] ToMuliId = ToEmail.Split(',');
            foreach (string ToEMailId in ToMuliId)
            {
                mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
            }


            //string[] CCId = cc.Split(',');

            //foreach (string CCEmail in CCId)
            //{
            //    mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
            //}

            //string[] bccid = bcc.Split(',');

            //foreach (string bccEmailId in bccid)
            //{
            //    mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
            //}
            SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
            smtp.Host = HostAdd;              //host of emailaddress for example smtp.gmail.com etc  

            //network and security related credentials  

            smtp.EnableSsl = false;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = Pass;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 25;
            smtp.Send(mailMessage); //sending Email  

            //LogParsing.openOBS("","");
            //LogParsing.ExecuteCommand(@"C:\Users\Lorenzo\Desktop\openOBS.bat");
        }

        #endregion
    }
}
