using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // config params
    [SerializeField] Paddle paddle;
    [SerializeField] float vel;
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float screenHeightInUnits = 12f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    //state
    Vector2 paddleToBallVector;
    bool hasStarted;
    public bool aimStarted;
    float yVel;
    float xVel;

    //cached component refs
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        aimStarted = false;
        paddleToBallVector = transform.position - paddle.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasStarted && !aimStarted)
        {
            LockBallToPaddle();
            AimOnRightClick();
        }
        else if(!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void AimOnRightClick()
    {
        if(Input.GetMouseButtonDown(1))
        {
            aimStarted = true;
        }
    }

    private void LaunchOnMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            float mousePosXInUnits = Input.mousePosition.x / Screen.width * screenWidthInUnits;
            float mousePosYInUnits = Input.mousePosition.y / Screen.height * screenHeightInUnits;
            float xDiff = mousePosXInUnits - transform.position.x;
            float yDiff = Mathf.Clamp(mousePosYInUnits - transform.position.y, 1f, screenHeightInUnits);
            float totalDiff = (float)Math.Sqrt(Math.Pow(xDiff, 2f) + Math.Pow(yDiff, 2f));
            float ratio = vel / totalDiff;
            xVel = xDiff * ratio;
            yVel = yDiff * ratio;
            myRigidBody2D.velocity = new Vector2(xVel, yVel);
            hasStarted = true;
            aimStarted = false;
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (UnityEngine.Random.Range(0f, randomFactor), 
            UnityEngine.Random.Range(0f, randomFactor));
        if(hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;
        }
        if(collision.gameObject.name == "Paddle")
        {
            ResetVelocityMagnitude();
        }
    }

    private void ResetVelocityMagnitude()
    {
        float currentVelMagnitude = (float)Math.Sqrt(Math.Pow(myRigidBody2D.velocity.x, 2f) + Math.Pow(myRigidBody2D.velocity.y, 2f));
        Vector2 currentVelDir = new Vector2(myRigidBody2D.velocity.x / currentVelMagnitude, myRigidBody2D.velocity.y / currentVelMagnitude);
        float velMagnitude = (float)Math.Sqrt(Math.Pow(xVel, 2f) + Math.Pow(yVel, 2f));
        myRigidBody2D.velocity = currentVelDir * velMagnitude;
    }
}
