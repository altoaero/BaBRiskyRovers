using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverSpriteChanger : MonoBehaviour
{

    public Sprite[] roverSprites;
    public GameObject rover1;
    // Start is called before the first frame update
    void Start()
    {
        int selectedSprite = PlayerPrefs.GetInt("selectedCharacter");
        rover1.GetComponent<SpriteRenderer>().sprite = roverSprites[selectedSprite];
    }

    
}
