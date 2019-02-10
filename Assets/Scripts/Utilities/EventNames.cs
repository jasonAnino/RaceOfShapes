using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class EventNames
    {
        // Camera Related
        public const string CAMERA_CHANGE_FOCUS = "CAMERA_CHANGE_FOCUS";
        public const string CAMERA_CLEAR_FOCUS = "CAMERA_CLEAR_FOCUS";
        public const string CAMERA_MOUSE_SWITCH = "CAMERA_MOUSE_SWITCH";
        public const string CAMERA_VIEWMODE_MINIGAME = "CAMERA_VIEWMODE_MINIGAME";

        // User Interface
        public const string SET_UI_PLAYER_REFERENCE = "SET_UI_PLAYER_REFERENCE";
        public const string NOTIFY_PLAYER_INTERACTION = "NOTIFY_PLAYER_INTERACTION";
        public const string CHECK_NOTIFICATION = "CHECK_NOTIFICATION";
        public const string UPDATE_UNIT_HEALTH = "UPDATE_UNIT_HEALTH";
        public const string UPDATE_PLAYER_STAMINA = "UPDATE_PLAYER_STAMINA";
        public const string UPDATE_CONTROLLED_UNITS = "UPDATE_CONTROLLED_UNITS";
        public const string REMOVE_CONTROLLED_UNITS = "REMOVE_CONTROLLED_UNITS";
        // User Interface Skills
        public const string UPDATE_VISUAL_SKILLS = "UPDATE_VISUAL_SKILLS";
        public const string RESET_VISUAL_SKILLS = "RESET_VISUAL_SKILLS";
        // User Interaction
        public const string PLAYER_PICKUP_ITEM = "PLAYER_PICKUP_ITEM";
    }
}
