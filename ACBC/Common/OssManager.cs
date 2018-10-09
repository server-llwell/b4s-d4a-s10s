using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class OssManager
    {
        private static OssClient ossClient;
        private static string path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "upload");

        private static OssClient GetInstance()
        {
            if (ossClient == null)
            {
                ossClient = new OssClient(Global.OssHttp, Global.AccessId, Global.AccessKey);
            }
            return ossClient;
        }

        public static string UploadFileToOSS(string fileName, string ossDir, string newFileName)
        {
            string ss = "";
            try
            {
                OssClient client = GetInstance();
                using (var fs = File.OpenRead(Path.Combine(path, fileName)))
                {
                    var ret = client.PutObject(Global.OssBucket, ossDir + newFileName, fs);
                    if(ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ss = Global.OssUrl + ossDir + fileName;
                    }
                }
                
            }
            catch
            {
                
            }

            return ss;
        }

        private static string GetCallbackResponse(PutObjectResult putObjectResult)
        {
            string callbackResponse = null;
            using (var stream = putObjectResult.ResponseStream)
            {
                var buffer = new byte[4 * 1024];
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                callbackResponse = Encoding.Default.GetString(buffer, 0, bytesRead);
            }
            return callbackResponse;
        }
    }
}
