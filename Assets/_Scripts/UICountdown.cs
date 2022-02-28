using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountdown : MonoBehaviour
{
    [SerializeField] public List<Sprite> sprites;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
    }

    public void ChangeSprite(int i)
    {
        image.sprite = sprites[i - 1];
    }
}
