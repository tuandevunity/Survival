using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform enemy = null;
        enemy = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)[0].transform;
        transform.up = (enemy.position - transform.position).normalized;
        Debug.Log(transform.up);
    }
}
