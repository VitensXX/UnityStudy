using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node
{
    public abstract BaseNode Next();
    public abstract void Run(System.Action finished);
}
