using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveWorldPacket : ServerPacket
{

    public LeaveWorldPacket(byte[] d) : base(d)
    {
        Parse();
    }

    public override void Parse()
    {
    }
}