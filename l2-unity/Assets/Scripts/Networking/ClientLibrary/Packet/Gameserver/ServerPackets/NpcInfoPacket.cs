using System;
using UnityEngine;

public class NpcInfoPacket : ServerPacket
{
    public NetworkIdentity Identity { get; private set; }
    public NpcStatus Status { get; private set; }
    public Stats Stats { get; private set; }
    public bool Running { get; set; }

    public NpcInfoPacket(byte[] d) : base(d)
    {
        Identity = new NetworkIdentity();
        Status = new NpcStatus();
        Stats = new Stats();
        Parse();
    }

    public override void Parse()
    {
        try
        {
            Identity.Id = ReadI();
            Identity.NpcId = ReadI();
            Identity.Heading = ReadF();
            Identity.SetPosX(ReadF());
            Identity.SetPosY(ReadF());
            Identity.SetPosZ(ReadF());
            // Stats
            Stats.RunSpeed = ReadI();
            Stats.WalkSpeed = ReadI();
            Stats.PAtkSpd = ReadI();
            Stats.MAtkSpd = ReadI();
            // Status
            Stats.Level = ReadI();
            Status.Hp = ReadI();
            Stats.MaxHp = ReadI();

            Running = ReadI() == 1;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
