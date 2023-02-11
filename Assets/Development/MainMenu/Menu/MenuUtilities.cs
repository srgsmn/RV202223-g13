using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuUtilities
{
    public static class CONSTS {
        public const string BACK_OPT = "BACK";
        public const string MAIN_OPT = "MAIN";
        public const string START_OPT = "START";
        public const string REPORTS_OPT = "REPORTS";
        public const string SETTINGS_OPT = "SETTINGS";
        public const string ABOUT_OPT = "ABOUT";
        public const string QUIT_OPT = "QUIT";

        public const string ANIM_FLAG = "isShown";
    }

    public enum MainMenuScreen
    {
        None, Main, Start, Reports, Settings, About, Quit, Prev
    }

    [Serializable]
    public struct MainMenuItem
    {
        public MainMenuScreen type;
        public GameObject gameObject;
    }

    public enum SubMenuDetail
    {
        None, Demo1, Demo2, Demo3, Browsed, Back, Report1, Report2, Report3, Project, Team, Assets, Audio, Controllers
    }

    public enum PlayScene
    {
        Startup, Demo1, Demo2, Demo3, Browse
    }

    public enum PopUpOpt
    {
        Move, Delete
    }

    public enum SettingType
    {
        Volume, TextReader
    }
}

//$"{GetType().Name}.cs > Changing screen from {_activePanel} to {screen}"
