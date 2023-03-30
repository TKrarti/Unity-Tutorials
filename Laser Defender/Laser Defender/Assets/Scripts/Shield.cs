using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int startingShieldHealth = 400;
    [SerializeField] float shieldDuration = 10f;
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] Player player;

    int shieldHealth;
    float timeBetweenSprites;
    float healthBetweenSprites;
    bool isTimeToChangeSprite;
    int spriteIndex;

    // Start is called before the first frame update
    void Start()
    {
        shieldHealth = startingShieldHealth;
        timeBetweenSprites = shieldDuration / spriteList.Count;
        healthBetweenSprites = startingShieldHealth / spriteList.Count;
        isTimeToChangeSprite = false;
        spriteIndex = spriteList.Count;
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            ProcessHit(damageDealer);
        }
    }

    void Update()
    {
        UpdateShield();
    }

    private void UpdateShield()
    {
        gameObject.transform.position = player.transform.position;
        shieldDuration = ChangeSprite(shieldDuration, Time.deltaTime, timeBetweenSprites);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int damage = damageDealer.GetDamage();
        //Add SFX and VFX
        shieldHealth = (int)ChangeSprite(shieldHealth, damage, healthBetweenSprites);
        Debug.Log("Shield Health: " + shieldHealth);
        damageDealer.Hit();
    }

    private float ChangeSprite(float metric, float subtractedIncrement, float incrementBetweenSprites)
    {
        metric -= subtractedIncrement;

        if (metric <= spriteIndex * incrementBetweenSprites)
        {
            spriteIndex -= 1;
            isTimeToChangeSprite = true;
        }

        if (metric <= 0)
        {
            KillShield();
        }

        if (isTimeToChangeSprite && spriteIndex >= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[spriteIndex];
            isTimeToChangeSprite = false;
        }

        return metric;
    }

    private void KillShield()
    {
        Destroy(gameObject);
    }

    public void ResetShieldHealth()
    {
        shieldHealth = startingShieldHealth;
        spriteIndex = spriteList.Count;
        isTimeToChangeSprite = true;
    }
}
