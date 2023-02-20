using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class PREFABS
    {
        public const string TUTORIAL = "Prefabs/TutorialCanvas";
        public const string PAUSE = "Prefabs/PauseCanvas";
        public const string HUD = "Prefabs/HUDCanvas";
        public const string EVSYS = "Prefabs/EventSystem";
        public const string FINALE = "Prefabs/FinaleCanvas";
        public const string LOADING = "Prefabs/LoadingCanvas";
    }

    public enum SceneType
    {
        MainMenu, Demo1, Demo2, Demo3, Browse
    }

    public enum SceneState
    {
        None, Loading, Tutorial, Playing, Paused, Endgame
    }

    public enum Mode
    {
        EPSelector, // Endpoint selector
        Nav,        // Navigation mode
        Edit,       // Edit mode
        Plan        // Plan mode
    }

    public enum AccItemType
    {
        Ramp, Stairlift, DoorButton
    }

    /// <summary>
    /// Rotation direction
    /// </summary>
    public enum RotDir
    {
        Cw, //Clockwise
        CCw //CounterClockwise
    }

    /// <summary>
    /// Translation direction
    /// </summary>
    public enum TranDir
    {
        Fwd,    //Forward
        Bwd,    //Backward
        Lt,     //Left
        Rt      //Right
    }

    public enum FileFormat
    {
        TXT=0, JSON=1
    }
}
