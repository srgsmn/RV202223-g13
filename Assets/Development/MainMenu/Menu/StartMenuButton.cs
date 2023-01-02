using System;
using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;

public class StartMenuButton : MenuButton
{
    [SerializeField] private Environments environment;

    public delegate void EnvSelectionEv(Environments environment);
    public static EnvSelectionEv OnEnvSelect;

    public void CallDescription()
    {
        OnEnvSelect?.Invoke(environment);
    }
}
