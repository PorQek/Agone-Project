using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprites; //zbiera sprity ikony obrażeń

    public float lifetime; //jak długo ikona będzie wyświetlana

    private void Start()
    {
        Invoke("Destruction", lifetime);
    }
    public void Setup (int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageSprites[damage - 1];
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
