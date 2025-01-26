using UnityEngine;

public class ActivateControlMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetActive()
    { 
        this.gameObject.SetActive(!this.gameObject.active);
    }
}
