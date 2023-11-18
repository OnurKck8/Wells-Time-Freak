using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndıcator : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);
    }
}
