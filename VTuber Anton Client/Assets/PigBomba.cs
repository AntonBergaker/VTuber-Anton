using UnityEngine;
using VTuberAnton.Common.Packets;

public class PigBomba : MonoBehaviour, IPacketListener<TwitchChatPacket>
{
    public WebsocketClient Client;
    public GameObject PigBombaTemplate;

    void Start() {
        Client.Listen("twitch_chat", this);
    }

    void IPacketListener<TwitchChatPacket>.HandlePacket(TwitchChatPacket packet) {
        var lower = packet.Message.ToLower();
        if (!lower.Contains("pig bomba") && !lower.Contains("pigbomba")) {
            return;
        }

        var obj = Instantiate(PigBombaTemplate);
        obj.SetActive(true);
    }

}
