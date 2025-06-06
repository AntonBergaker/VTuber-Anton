namespace VTuberAnton.Common.Packets;
public class TwitchChatPacket {
    public TwitchChatPacket(string message, string user) {
        Message = message;
        User = user;
    }

    public string Message { get; set; }
    public string User { get; set; }

}
