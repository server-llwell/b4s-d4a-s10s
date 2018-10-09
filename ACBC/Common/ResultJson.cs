using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBC.Common
{
    /// <summary>
    /// 返回信息内容
    /// </summary>
    public class Message
    {
        public Message(CodeMessage code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public CodeMessage code;
        public string msg;
    }

    /// <summary>
    /// 返回结果外部封装对象
    /// </summary>
    public class ResultsJson
    {
        public ResultsJson(Message msg, object data)
        {
            if(msg != null)
            {
                this.success = false;
                this.msg = msg;
            }
            else
            {
                this.success = true;
                this.msg = new Message(CodeMessage.OK, "OK");
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "; " + "Message:" + this.msg.msg);
            this.data = data;
        }

        public bool success = true;
        public Message msg;
        public object data;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Message:{0}; data:{1}", msg.msg, JsonConvert.SerializeObject(data));
            string rets = builder.ToString();
            return rets;
        }
    }


}
