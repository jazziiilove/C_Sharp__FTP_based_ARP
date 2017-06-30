using System.Collections.Generic;
using System.Net;
using System.IO;

namespace FTPARP
{
    class Program
    {        
        static void Main(string[] args)
        {
            FtpWebRequest request = FtpWebRequest.Create("ftp://139.179.33.186/ARP/blackarp.txt") as FtpWebRequest;
            //request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("ARP", "PRA");
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            Stream responseStream = response.GetResponseStream();
            //... FTP commands
            List<string> files = new List<string>();
            StreamReader reader = new StreamReader(responseStream);

            /*
            while (!reader.EndOfStream)
            {            
            files.Add(reader.ReadLine());
            }
            
                Console.WriteLine(files.ElementAt(0));
            */

            // Stream responseStream = response.GetResponseStream();

            MemoryStream memStream = new MemoryStream();
            int size = 1024;
            byte[] buffer = new byte[size];
            int bytesRead = responseStream.Read(buffer, 0, buffer.Length);
            while (bytesRead != 0) //while(true)
            {
                //Try to read the data
                //bytesRead = reader.Read(buffer, 0, buffer.Length);
                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                //Write the downloaded data
                memStream.Write(buffer, 0, bytesRead);
            }
            byte[] downloadedData = new byte[0];
            //Convert the downloaded stream to a byte array
            downloadedData = memStream.ToArray();

            //Clean up
            reader.Close();
            memStream.Close();

            //for (int i = 0; i < downloadedData.Length; i++)
            //Console.Write("aa" + (char)downloadedData[i]);

            System.Text.Encoding enc = System.Text.Encoding.ASCII;


            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            info.FileName = "arp.exe";
            info.Arguments = " -s " + (enc.GetString(downloadedData).ToString());
            process.StartInfo = info;
            process.Start();

            //Console.Write(enc.GetString(downloadedData).ToString());
            //StreamWriter sw = new StreamWriter("c:\\ultimatearp.txt");

            // write a line of text to the file
            //sw.Write(enc.GetString(downloadedData).ToString());

            responseStream.Close();
            response.Close(); //Closes the connection to the server
            //sw.Close();
        }
    }
}
