using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Paddle : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Ball ball;
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    [SerializeField] float velResponse = 10f;

    //state
    float xVel;

    //cached ref
    GameSession theGameSession;
    Ball theBall;

    // Start is called before the first frame update
    void Start()
    {
        theGameSession = FindObjectOfType<GameSession>();
        theBall = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!ball.aimStarted)
        {
            float cursorPosXInUnits = Mathf.Clamp(GetXPos(), minX, maxX);
            float xDiff = cursorPosXInUnits - transform.position.x;
            xVel = xDiff * velResponse;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    private float GetXPos()
    {
        if(theGameSession.IsAutoPlayEnabled())
        {
            return theBall.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }

}
