using Unity.Properties;
using UnityEngine;

public class ControlScheme : MonoBehaviour
{
    private bool keyboardControlsP1 = true;
    private bool keyboardControlsP2 = true;
    private int numPlayersUsingGamepad = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public bool UsingKeyboardControls(int player)
    {
        if (player == 1)
        {
            return keyboardControlsP1;
        }
        return keyboardControlsP2;
    }

    public void FlipControlScheme(int player, bool usingKeyboard)
    {
        if (player == 1)
        {
            keyboardControlsP1 = usingKeyboard;
            return;
        }
        keyboardControlsP2 = usingKeyboard;
    }

    public int NumPlayersUsingGamepad()
    {
        numPlayersUsingGamepad = 0;
        if (!keyboardControlsP1)
        {
            ++numPlayersUsingGamepad;
        }
        if (!keyboardControlsP2)
        {
            ++numPlayersUsingGamepad;
        }
        return numPlayersUsingGamepad;
    }
}
