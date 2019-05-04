using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
        if (other.GetComponent<TrashMan>())
        {
            other.GetComponent<TrashMan>().AddScore(5);
        }
    }
}
