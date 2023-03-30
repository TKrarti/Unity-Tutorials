using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDealer : MonoBehaviour
{
    [SerializeField] int healthValue = 150;

    public int GetHealthValue() { return healthValue; }
}
