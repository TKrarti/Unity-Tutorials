using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Enemy : MonoBehaviour
{
    public StateManager stateManager;
    public EnemyManager enemyManager;

    public Animator animator;
    public new Rigidbody rigidbody;
    public new Light light;

    public float speed = 3f;
    public float spawnTime = 3f;
    float timeToSpawn;

    bool spawnTimerStarted = false;
    bool ready = false;

    private void Awake()
    {
        enemyManager.AddEnemy(this);
    }

    private void Start()
    {
        if(stateManager.currentGameState == GameState.PreGame)
        {
            ready = true;
            light.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if(stateManager.currentGameState == GameState.GameActive)
        {
            if(ready)
            {
                HandleMovement();
            }
            else
            {
                if(!spawnTimerStarted)
                {
                    spawnTimerStarted = true;
                    timeToSpawn = Time.time + spawnTime;
                }
                else
                {
                    if (Time.time >= timeToSpawn)
                    {
                        ready = true;
                        light.enabled = true;
                    }
                }
            }
        }
    }

    void HandleMovement()
    {
        animator.SetBool("moving", true);

        rigidbody.velocity = transform.forward * speed;

        RaycastHit hit;

        for(int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, 90f*i, 0) * transform.forward, out hit, 10f, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "Player")
                {
                    transform.rotation = Quaternion.Euler(0, 90f*i, 0);
                }
                else if (i == 0 && hit.distance < 0.55f)
                {
                    int turnAmount = Random.Range(1, 5);
                    transform.rotation = Quaternion.Euler(0, 90 * turnAmount, 0);
                }
                else if (i == 0 && hit.distance - Utils.RoundToInt(hit.distance) <= 0.005f)
                {
                    int chance = Random.Range(1, 1000);
                    if (chance <= 15)
                    {
                        int turnAmount = Random.Range(1, 5);
                        transform.rotation = Quaternion.Euler(0, 90 * turnAmount, 0);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player" && ready)
        {
            stateManager.gameManager.OnPlayerKilled(collision.gameObject.GetComponent<Player>().playerBrain);
            animator.SetBool("moving", false);
        }
    }

    private void OnDestroy()
    {
        enemyManager.RemoveEnemy(this);
        stateManager.gameManager.OnEnemyKilled();
    }

}
