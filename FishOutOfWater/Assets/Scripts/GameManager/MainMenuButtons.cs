using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlTextP1;
    [SerializeField] private TextMeshProUGUI controlTextP2;
    [SerializeField] private GameObject BubbleTransition;
    [SerializeField] private float DelayedLoadTime;
    [SerializeField] private ControlScheme cs;
    private bool keyboardControlP1 = true;
    private bool keyboardControlP2 = true;

    public void StartGameClick()
    {
        BubbleTransition.gameObject.SetActive(true);
        StartCoroutine(DelayedLoad(DelayedLoadTime));
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
    IEnumerator DelayedLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }
}
