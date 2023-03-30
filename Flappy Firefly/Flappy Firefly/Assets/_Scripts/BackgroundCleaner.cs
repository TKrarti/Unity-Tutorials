using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundCleaner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Background")
        {
            float widthOfObject = ((BoxCollider2D)collision).size.x;

            Vector3 position = collision.transform.position;
            position.x += widthOfObject * 1.99f;
            collision.transform.position = position;
        }
    }
}
