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
        public const string MODE = "Prefabs/ModesCanvas";
        public const string EVSYS = "Prefabs/EventSystem";
    }

    public enum SceneType
    {
        MainMenu, Demo1, Demo2, Demo3, Browse
    }

    public enum SceneState
    {
        None, Loading, Tutorial, Playing, Paused
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
}
