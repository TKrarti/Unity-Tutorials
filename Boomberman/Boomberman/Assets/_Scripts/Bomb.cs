using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem mainParticle;
    public PlayerBrain ownerBrain;

    public GameManager gameManager;
    public StateManager stateManager;

    public FloatVariable bombTimerLength;
    public float explosionTick;
    bool exploded = true;

    public AudioSource audioSource;
    public AudioClip beapingSound;
    public AudioClip explosionSound;

    private void OnEnable()
    {
        gameManager = stateManager.gameManager;

        explosionTick = Time.time + bombTimerLength.value;

        var particle = mainParticle.main;
        var particles = mainParticle.subEmitters;

        particle.startLifetime = bombTimerLength.value;

        for(int i = 0; i < particles.subEmittersCount; i++)
        {
            var emitter = particles.GetSubEmitterSystem(i).main;
            emitter.startDelay = bombTimerLength.value;
        }

        mainParticle.Play();
        exploded = false;

        if(ownerBrain)
        {
            ownerBrain.AddActiveBomb(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!exploded && Time.time > explosionTick)
        {
            exploded = true;
            HandleExplosion();
        }
    }

    void HandleExplosion()
    {
        audioSource.PlayOneShot(explosionSound);

        RaycastHit hit;

        if(Utils.RoundedVector3(gameManager.player1.playerGO.transform.position) == transform.position)
        {
            gameManager.OnPlayerKilled(gameManager.player1);
        }
        if (gameManager.player2.playerGO && Utils.RoundedVector3(gameManager.player2.playerGO.transform.position) == transform.position)
        {
            gameManager.OnPlayerKilled(gameManager.player2);
        }
        if(gameManager.gameMode == GameMode.Singleplayer)
        {
            for (int i = 0; i < gameManager.enemyManager.spawnedEnemies.Count; i++)
            {
                if (Utils.RoundedVector3(gameManager.enemyManager.spawnedEnemies[i].transform.position) == transform.position)
                {
                    Destroy(gameManager.enemyManager.spawnedEnemies[i].gameObject);
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            transform.rotation = Quaternion.Euler(0, 90 * i, 0);
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.green, 60f);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                float timeLag = Vector3.Distance(hit.transform.position, transform.position);
                timeLag -= 0.5f;
                timeLag /= 10f;

                if (hit.transform.tag == "Player")
                {
                    gameManager.OnPlayerKilled(hit.transform.GetComponent<Player>().playerBrain);
                }
                else if (hit.transform.tag == "BreakableBlock")
                {
                    Block blockBroken = hit.transform.GetComponent<Block>();
                    ParticleSystem explosionEffect = Instantiate(blockBroken.particles) as ParticleSystem;
                    explosionEffect.transform.position = blockBroken.transform.position;
                    var explosionEff = explosionEffect.main;
                    explosionEff.startDelay = timeLag;
                    explosionEffect.Play();

                    Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
                    DestroyGO(hit, timeLag);
                }
                else if (hit.transform.tag == "Enemy")
                {
                    DestroyGO(hit, timeLag);
                }
            }
        }
        Destroy(gameObject, 0.6f);
    }

    private void OnDestroy()
    {
        if(ownerBrain)
        {
            ownerBrain.RemoveBomb(this);
        }
    }

    void DestroyGO(RaycastHit hit, float timeDelay)
    {
        if (timeDelay <= 0)
        {
            Destroy(hit.transform.gameObject);
        }
        else
        {
            Destroy(hit.transform.gameObject, timeDelay);
        }
    }
}
