using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : MonoBehaviour { 
    private WebSocketServer server;
    private WebsocketService service;

    public int Port = 1234;
    public string Host = "127.0.0.1";

    private void Start() {
        server = new WebSocketServer(IPAddress.Parse(Host), Port);

        server.AddWebSocketService<WebsocketService>("/", (service) => this.service = service);

        server.Start();
    }

    public void BroadcastData<T>(string channel, T data) {
        if (this.service == null) {
            return;
        }
        service.BroadcastData(new WebsocketData<T>(channel, data));
    }

    private class WebsocketData<T> {
        public WebsocketData(string channel, T data) {
            Channel = channel;
            Data = data;
        }

        public string Channel { get; }
        public T Data { get; }
    }

    private class WebsocketService : WebSocketBehavior {
        protected override void OnMessage(MessageEventArgs e) {
            base.OnMessage(e);
            if (e.IsText == false) {
                Debug.Log("Did not recieve text monkaweird");
                return;
            }
        }

        public void BroadcastData<T>(T data) {
            string stringData = JsonConvert.SerializeObject(data);
            Sessions.Broadcast(stringData);
        }
    }
}
