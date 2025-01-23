using UnityEngine;

public class TriggerPlayerZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(this.name);
        switch (this.name)
        {
            case "Zone0":
                if (collision.name == "Player1Tele")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerOneZone = 0;
                    if (PlayerGlobals.PlayerTwoZone > PlayerGlobals.PlayerOneZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                        Debug.Log("Can Teleport!");
                    }
                }
                else if (collision.name == "Player2Tele")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerTwoZone = 0;
                    if (PlayerGlobals.PlayerOneZone > PlayerGlobals.PlayerTwoZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                        Debug.Log("Can Teleport!");
                    }
                }
                break;

            case "Zone1":
                if (collision.name == "Player1Tele")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerOneZone = 1;
                    if (PlayerGlobals.PlayerTwoZone > PlayerGlobals.PlayerOneZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                        Debug.Log("Can Teleport!");
                    }
                }
                else if (collision.name == "Player2Tele")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerTwoZone = 1;
                    if (PlayerGlobals.PlayerOneZone > PlayerGlobals.PlayerTwoZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                        Debug.Log("Can Teleport!");
                    }
                }
                break;

          /*  case "Zone2":
                if (collision.name == "Player1")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerOneZone = 2;
                    if (PlayerGlobals.PlayerTwoZone > PlayerGlobals.PlayerOneZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                    }
                }
                else if (collision.name == "Player2")
                {
                    Debug.LogWarning(collision.name);
                    Debug.LogWarning(this.name);
                    PlayerGlobals.PlayerTwoZone = 2;
                    if (PlayerGlobals.PlayerOneZone > PlayerGlobals.PlayerTwoZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                    }
                }
                break;

            case "Zone3":
                if (collision.name == "Player1")
                {
                    PlayerGlobals.PlayerOneZone = 3;
                    if (PlayerGlobals.PlayerTwoZone > PlayerGlobals.PlayerOneZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                    }
                }
                else if (collision.name == "Player2")
                {
                    PlayerGlobals.PlayerTwoZone = 3;
                    if (PlayerGlobals.PlayerOneZone > PlayerGlobals.PlayerTwoZone)
                    {
                        PlayerGlobals.SetCanTeleport();
                    }
                }
                break;*/
        }
    }
}
