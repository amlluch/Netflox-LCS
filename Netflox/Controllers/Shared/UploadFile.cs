using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Netflox.Controllers
{
    public class UploadFile
    {
        private readonly string user;
        private readonly string password;
        private readonly string server;



        public UploadFile(string user, string password, string server )
        {
            this.user = user;
            this.password = password;
            this.server = server;
 
        }
        public async Task<byte[]> sendfileAsync(string fileName, IFormFile FileToUpload)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(user, password);
                //                  request.EnableSsl = true;
                request.KeepAlive = false;
                //                  request.UsePassive = true;
                request.UseBinary = true;


                byte[] fileContents = null;
                using (var memoryStream = new MemoryStream())
                {
                    await FileToUpload.CopyToAsync(memoryStream);
                    fileContents = memoryStream.ToArray();
                }
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();

                return fileContents;
            } catch (Exception e)
            {
                return null;
            }
        }
    }
}
