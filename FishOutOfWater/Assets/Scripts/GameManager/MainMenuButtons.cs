using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlTextP1;
    [SerializeField] private TextMeshProUGUI controlTextP2;
    [SerializeField] private ControlScheme cs;
    private bool keyboardControlP1 = true;
    private bool keyboardControlP2 = true;

    public void StartGameClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGameClick()
    {
        Application.Quit();
    }

    public void ChangeControlsClickP1()
    {
        keyboardControlP1 = !keyboardControlP1;
        cs.FlipControlScheme(1, keyboardControlP1);
        if (keyboardControlP1)
        {
            controlTextP1.text = "Keyboard";
        }
        else
        {
            controlTextP1.text = "Controller";
        }
    }

    public void ChangeControlsClickP2()
    {
        keyboardControlP2 = !keyboardControlP2;
        cs.FlipControlScheme(2, keyboardControlP2);
        if (keyboardControlP2)
        {
            controlTextP2.text = "Keyboard";
        }
        else
        {
            controlTextP2.text = "Controller";
        }
    }
}
