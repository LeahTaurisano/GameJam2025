using UnityEngine;

public static class PlayerGlobals
{
    public static int PlayerOneZone;
    public static int PlayerTwoZone;
    public static bool canTeleport = false;


    public static void SetCanTeleport() { canTeleport = true; }
}
