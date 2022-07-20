using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{
    public int currentState_net;
    
    public enum Net { Wait, ReConnect, Master, Room }
}
