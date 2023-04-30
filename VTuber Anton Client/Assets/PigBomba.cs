using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBomba : MonoBehaviour, IPacketListener<PigBomba.TwitchChatData>
{
    public WebsocketClient Client;
    public GameObject PigBombaTemplate;

    void Start() {
        Client.Listen("twitch_chat", this);
    }

    void IPacketListener<TwitchChatData>.HandlePacket(TwitchChatData packet) {
        var lower = packet.Message.ToLower();
        if (!lower.Contains("pig bomba") && !lower.Contains("pigbomba")) {
            return;
        }

        var obj = Instantiate(PigBombaTemplate);
        obj.SetActive(true);
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
