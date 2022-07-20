using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, false);
    }
}
