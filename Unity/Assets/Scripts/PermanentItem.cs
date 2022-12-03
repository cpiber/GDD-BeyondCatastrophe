using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PermanentItem : Item
{
    public override bool IsReusable () {
        return true;
    }
}
