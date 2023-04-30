public interface IPacketListener<T> {
    public void HandlePacket(T packet);
}
