//using System.Net;

//namespace MediaService.BL.Model
//{
//    public class Log
//    {
//        /// <summary>
//        /// This constructor creates a log file with the details of the exception and the user details
//        /// </summary>
//        /// <param name="rq">This is the request object that contains the user details and the exception details</param>
//        public Log(LogRequest rq)
//        {
//            var line = Environment.NewLine + Environment.NewLine;

//            try
//            { // Get the current directory
//                string currentDirectory = Directory.GetCurrentDirectory();

//                string filePath = Path.Combine(currentDirectory, "~/ExceptionFile/CsExceptions/");
//                if (rq.UserMobile == "DB")
//                {
//                    filePath = Path.Combine(currentDirectory, "~/ExceptionFile/DBExceptions/");
//                }
//                // Specify the file path

//                if (!Directory.Exists(filePath))
//                {
//                    Directory.CreateDirectory(filePath);
//                }
//                filePath = filePath + DateTime.Today.ToString("dd-MMM-yyyy") + ".txt";   //Text File Name
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Dispose();
//                }
//                using (StreamWriter sw = File.AppendText(filePath))
//                {
//                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line
//                        + "Line No :" + " " + "" + line + "UserMobile:" + ""
//                        + rq.UserMobile + line + "LogName:" + " " + rq.LogName + line
//                        + "LogType :" + " " + rq.LogType + line + "API endpoint:" + ""
//                        + rq.APIEndpoint + line + "LogSource:" + " " + rq.LogSource + line + "LogAPIRequest:"
//                        + "" + rq.LogAPIRequest + line + "User Host IP:" + " " + GetIPAddress() + line;
//                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
//                    sw.WriteLine("-------------------------------------------------------------------------------------");
//                    sw.WriteLine(line);
//                    sw.WriteLine(error);
//                    sw.WriteLine("--------------------------------*End*------------------------------------------");
//                    sw.WriteLine(line);
//                    sw.Flush();
//                    sw.Close();
//                }
//            }
//            catch (Exception e)
//            {
//                e.ToString();

//            }
//        }

//        /// <summary>
//        /// This method returns the IP address of the local machine
//        /// </summary>
//        /// <returns>This method returns the IP address of the local machine</returns>
//        public static string GetIPAddress()
//        {
//            string IPAddress = "";
//            IPHostEntry Host = default;
//            string Hostname = null;
//            Hostname = Environment.MachineName;
//            Host = Dns.GetHostEntry(Hostname);
//            foreach (IPAddress IP in Host.AddressList)
//            {
//                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
//                {
//                    IPAddress = Convert.ToString(IP);
//                }
//            }
//            return IPAddress;
//        }
//    }
//}
