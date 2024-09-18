public class ChangeMoveTypePacket : ServerPacket
{
    public int Owner { get; private set; }
    public bool Running { get; private set; }

    public int EntityId { get; private set; }

    public ChangeMoveTypePacket(byte[] d) : base(d)
    {
        Parse();
    }

    public override void Parse()
    {
        Owner = ReadI();
        Running = ReadI() == 1;
    }
}