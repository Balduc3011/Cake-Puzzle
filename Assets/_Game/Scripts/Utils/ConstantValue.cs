using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantValue
{
    // String
    public static string STR_BLANK = "";
    public static string STR_SPACE = " ";
    public static string STR_PERCENT = "%";
    public static string STR_DOT = ".";
    public static string STR_2DOT = ":";
    public static string STR_ZERO = "0";
    public static string STR_HOUR = "h";
    public static string STR_MINUTE = "m";
    public static string STR_SECOND = "s";
    public static string STR_PERSECOND = "/s";
    public static string STR_FREE = "Free";
    public static string STR_WATCH = "Watch";
    public static string STR_UNLOCK = "Unlock";
    public static string STR_DEFPRICE = "$0.01";
    public static string STR_DEFCURRENCY = "$";
    public static string STR_SLASH = "/";
    public static string STR_MUL = "x";
    public static string STR_ADD = "+";
    public static string STR_TAKEONE = "/1";
    public static string STR_REMAINTIME = "Remaining Times: ";
    public static string STR_MAX = "MAX";
    public static string STR_LEVEL = "LEVEL ";
    public static string STR_FLOAT_ONE = "F1";
    public static string STR_FURNITURE = "Furniture";
    public static string STR_EXP = "exp";
    public static string STR_POINT = "p";

    // VECTOR3
    public static Vector3 VEC3_VECTOR3_1 = new Vector3(1, 1, 1);

    // VALUE
    public static int VAL_COIN_BOOSTER = 100;
    public static float VAL_MAX_EXCEED = 1000000000000;
    public static float VAL_REVICE_PRICE = 750f;
    public static float VAL_DEFAULT_EXP = 10f;
    public static int VAL_DEFAULT_TROPHY = 10;
    public static float VAL_X2BOOSTER_TIME = 2.5f;

    // Animation

    // IE
    public static WaitForSeconds WAIT_SEC01 = new WaitForSeconds(0.1f);
    public static WaitForSeconds WAIT_SEC025 = new WaitForSeconds(0.25f);
    public static WaitForSeconds WAIT_SEC05 = new WaitForSeconds(0.5f);
    public static WaitForSeconds WAIT_SEC1 = new WaitForSeconds(1f);
    public static WaitForSeconds WAIT_SEC2 = new WaitForSeconds(2f);

    // UnlockLevel
    public static int skeleton_unlock_level = 5;
    public static int muscle_unlock_level = 10;
}

public enum VersionStatus
{
    Publish,
    Cheat,
    NoCheat,
    Normal
}

public enum SoundId
{
    None,
    SFX_Base
}
