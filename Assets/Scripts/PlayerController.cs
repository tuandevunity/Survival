using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Elements")]
    [SerializeField] private FloatingJoystick floatingJoystick;


    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = floatingJoystick.Direction * moveSpeed;
    }
}
