using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Transform target;


    [Header("Settings")]
    [SerializeField] private Vector2 maxMinXY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target attached !");
            return;
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = -10;
        targetPosition.y = Mathf.Clamp(targetPosition.y, -maxMinXY.y, maxMinXY.y);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -maxMinXY.x, maxMinXY.x);
        transform.position = targetPosition;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
