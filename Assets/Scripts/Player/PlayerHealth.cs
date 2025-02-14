using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        // avoid negative health by damage
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        UpdateUI();


        if (health <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        Debug.Log("dieeee !!!");
        SceneManager.LoadScene(0);
    }

    private void UpdateUI()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }
}
