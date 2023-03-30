using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;

    public FloatVariable darkness;
    public FloatVariable gameTime;
    public BoolVariable playerAlive;
    public Image darknessImage;

    float inverseDarknessSpeed;

    float offsetX;
    
    // Start is called before the first frame update
    void Start()
    {
        offsetX = transform.position.x - playerTransform.position.x;

        darkness.value = 0;

        inverseDarknessSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = playerTransform.position.x + offsetX;
        transform.position = position;
    }

    private void FixedUpdate()
    {
        gameTime.value += Time.fixedDeltaTime;
        if (gameTime.value <= 20f)
        {
            inverseDarknessSpeed = Mathf.Lerp(20f, 9f, gameTime.value / 40f);
        }

        if(darkness.value < 0)
        {
            darkness.value = 0;
        }

        if (playerAlive.value)
        {
            darkness.value += (Time.fixedDeltaTime / inverseDarknessSpeed);
        }
        else
        {
            darkness.value = 0;
        }

        darknessImage.color = new Color(0, 0, 0, darkness.value); 
    }
}
