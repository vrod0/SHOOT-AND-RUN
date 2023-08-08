using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float health = 100.0f;

    [SerializeField] TextMeshProUGUI healthTXT;

    public void TakeDamage(float damage)
    {
        health -= Mathf.Abs(damage);

        healthTXT.SetText("Enemy Life: "+ health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}