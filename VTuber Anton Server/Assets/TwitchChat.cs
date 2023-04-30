using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexone.UnityTwitchChat;
using System;

public class TwitchChat : MonoBehaviour
{
    public WebsocketServer Server;

    private void Start() {
        IRC.Instance.OnChatMessage += OnChatMessage;
    }

    private void OnChatMessage(Chatter obj) {
        Server.BroadcastData("twitch_chat", new TwitchChatData(obj.message, obj.tags.displayName));
    }

    private class TwitchChatData {
        public TwitchChatData(string message, string user) {
            Message = message;
            User = user;
        }

        public string Message { get; set; }
        public string User { get; set; }

    }
}
