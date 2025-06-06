using NativeWebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WebsocketClient : MonoBehaviour {
    private WebSocket socket;
    private Dictionary<string, List<ListenerData>> listeners;

    private bool connected;
    private bool connecting;

    public string Host = "127.0.0.1";
    public int Port = 1234;

    private class ListenerData {
        public ListenerData(MonoBehaviour listener, Action<JObject> action) {
            Listener = listener;
            ListenAction = action;
        }

        public MonoBehaviour Listener { get; }
        public Action<JObject> ListenAction { get; }
    }

    private void Awake() {
        connected = false;

        listeners = new();
        socket = new WebSocket($"ws://{Host}:{Port}");
        socket.OnMessage += Socket_OnMessage;
        socket.OnOpen += Socket_OnOpen;
        socket.OnClose += Socket_OnClose;
        socket.OnError += Socket_OnError;
    }

    private void Socket_OnError(string errorMsg) {
        Debug.Log("Socket error: " + errorMsg);

        if (connecting) {
            connecting = false;
            return;
        }

        socket.Close();
        connected = false;
    }

    private void Socket_OnClose(WebSocketCloseCode closeCode) {
        Debug.Log("Closed sadge");
        connected = false;
        connecting = false;
    }

    public void Listen<T>(string channel, IPacketListener<T> listener) {
        if (listener is not MonoBehaviour monoListener) {
            throw new Exception("Listener must be monobehaviour");
        }

        if (!listeners.TryGetValue(channel, out var list)) {
            listeners[channel] = list = new();
        }

        list.Add(new ListenerData(monoListener, (obj) => {
            var data = obj.ToObject<T>();
            listener.HandlePacket(data);
        }));
    }

    private void Update() {
    #if !UNITY_WEBGL || UNITY_EDITOR
            socket.DispatchMessageQueue();
    #endif

        if (connected == false && connecting == false) {
            socket.Connect();
            connecting = true;
        }
    }

    private void Socket_OnOpen() {
        Debug.Log("Connected!");
        connected = true;
        connecting = false;
    }

    private HashSet<string> warnedListeners;

    private void Socket_OnMessage(byte[] data) {
        string str = Encoding.UTF8.GetString(data);
        var obj = JObject.Parse(str);
        var channel = obj["Channel"].ToString();
        if (!listeners.TryGetValue(channel, out var list)) {
            if (!warnedListeners.Contains(channel)) {
                warnedListeners.Add(channel);
                Debug.Log($"No listener for channel: {channel}");
            }
        }
        foreach (var inst in list) {
            if (inst.Listener == null) {
                return;
            }
            inst.ListenAction((JObject)obj["Data"]);
        }
    }

}
