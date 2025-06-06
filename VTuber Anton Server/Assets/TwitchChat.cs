using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lexone.UnityTwitchChat;
using System;
using VTuberAnton.Common.Packets;

public class TwitchChat : MonoBehaviour
{
    public WebsocketServer Server;

    private void Start() {
        IRC.Instance.OnChatMessage += OnChatMessage;
    }

    private void OnChatMessage(Chatter obj) {
        Server.BroadcastData("twitch_chat", new TwitchChatPacket(obj.message, obj.tags.displayName));
    }

}
