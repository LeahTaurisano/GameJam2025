using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] GameObject windBox;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    public bool touched = false;
    private bool isOn = false;

    private SpriteRenderer leverSprite;
    void Start()
    {
        leverSprite = GetComponent<SpriteRenderer>();

    }  
    private void Update()
    {
        if(touched)
        {

            if (isOn == false)
            {
                leverSprite.sprite = onSprite;
                windBox.SetActive(true);
                isOn = true;

            }
            else if (isOn == true)
            {
                leverSprite.sprite = offSprite;
               windBox.SetActive(false);
                 isOn = false;

            }
            touched = false;

        }
    }


}
