using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private List<GameObject> windBoxes;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private bool isOn = false;
    public bool touched = false;
    private SpriteRenderer leverSprite;
    void Start()
    {
        leverSprite = GetComponent<SpriteRenderer>();
        if (isOn)
        {
            leverSprite.sprite = onSprite;
        }
        else
        {
            foreach (GameObject wind in windBoxes)
            {
                wind.transform.parent.GetComponent<Animator>().speed = 0;
                wind.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (touched)
        {
            if (!isOn)
            {
                leverSprite.sprite = onSprite;
                foreach (GameObject wind in windBoxes)
                {
                    wind.SetActive(true);
                    wind.transform.parent.GetComponent<Animator>().speed = 1;
                }
                isOn = true;
            }
            else if (isOn)
            {
                leverSprite.sprite = offSprite;
                foreach (GameObject wind in windBoxes)
                {
                    wind.transform.parent.GetComponent<Animator>().speed = 0;
                    wind.SetActive(false);
                }
                isOn = false;

            }
            touched = false;
        }
    }
}