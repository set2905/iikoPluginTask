using Resto.Front.Api;
using System;
using System.Threading;
using WebSocketSharp;

namespace iikoPluginTask.DTO.Constructor
{
    internal class WebSocketClient
    {
        private static WebSocketClient _instance;

        public string url = "ws://iiko.restoplace.cc/";

        public WebSocket ws;

        public int TimerCallBackTimeOut = 20000;

        public Timer CheckConnection;

        public bool IsConnected { get; set; }

        public WebSocketClient()
        {
            ws = new WebSocket(url);
            StartWs();
            PluginContext.Log.Info("WebSocketLogic instance constructed");
        }

        public static WebSocketClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WebSocketClient();
            }
            return _instance;
        }

        public void StartWs()
        {
            try
            {
                ws.Connect();
                TimerCallback tm = ConnectionCheck;
                CheckConnection = new Timer(tm, null, 10000, TimerCallBackTimeOut);
                if (ws.IsAlive)
                {
                    PluginContext.Log.Info("Connection successed");
                    return;
                }
                PluginContext.Log.Warn("Couldn't connect to socket " + url);
                CheckConnection.Change(TimerCallBackTimeOut / 2, TimerCallBackTimeOut / 2);
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error($"Error in WebSocket logic {ex}");
            }
        }

        public void SendAndLog(string methodName, string requestBody)
        {
            try
            {
                PluginContext.Log.Info("\n ************************************** \n IIKO=>RESTOPLACE " + methodName + ": \n " + requestBody + " \n ************************************** \n ");
                ws.Send(requestBody);
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error(ex.Message);
            }
        }
        private void ConnectionCheck(object obj)
        {
            try
            {
                if (ws.IsAlive)
                {
                    CheckConnection.Change(TimerCallBackTimeOut, TimerCallBackTimeOut);
                    return;
                }
                PluginContext.Log.Info("Connection to websocket  " + url + "  lost =(");
                PluginContext.Log.Info("Trying to connect...");
                ws.Connect();
                if (ws.IsAlive)
                {
                    PluginContext.Log.Info("Connection restored!");
                    CheckConnection.Change(TimerCallBackTimeOut, TimerCallBackTimeOut);
                }
                else
                {
                    CheckConnection.Change(TimerCallBackTimeOut / 2, TimerCallBackTimeOut / 2);
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error($"Error in connection recovery {ex}");
            }
        }
    }
}
