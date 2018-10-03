using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ACBC.Buss;
using ACBC.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.Cache.Redis;

namespace ACBC.Controllers
{
    public class SocketController
    {
        public const int BufferSize = 4096;
        public string basestringjson = string.Empty;
        WebSocket socket;

        SocketController(WebSocket socket)
        {
            this.socket = socket;
        }

        async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (this.socket.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                string receivemsg = Encoding.UTF8.GetString(buffer, 0, incoming.Count);

                if (receivemsg.StartsWith("getPayState"))
                {
                    Task task = Task.Run(() => GetPayState(receivemsg.Split(':')[1]));
                }
            }

        }

        async Task GetPayState(string scanCode)
        {
            while (this.socket.State == WebSocketState.Open)
            {
                WsPayState state = Utils.GetCache<WsPayState>(scanCode);
                if (state == null)
                {
                    Thread.Sleep(500);
                    continue;
                }
                Utils.DeleteCache(scanCode);
                string userMsg = "Success";
                byte[] x = Encoding.UTF8.GetBytes(userMsg);
                var outgoing = new ArraySegment<byte>(x);
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
                break;
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;
            try
            {
                var socket = await hc.WebSockets.AcceptWebSocketAsync();
                var h = new SocketController(socket);
                await h.EchoLoop();
            }
            catch
            { }
            
        }

        /// <summary>
        /// branches the request pipeline for this SocketHandler usage
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketController.Acceptor);
        }
    }

    
}