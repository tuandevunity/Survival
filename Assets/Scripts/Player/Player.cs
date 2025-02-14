using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    private PlayerHealth playerHealth;
    // Start is called before the first frame update

    
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
}
