using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private new Transform transform;
    public float speed = 4000.0f;

    public GameObject expEffect;


    void Start()
    {
        transform = GetComponent<Transform>();
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    void OnCollisionEnter()
    {
        Debug.Log("여기!!");
        GameObject obj = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 3.0f);
        // Destroy(this.gameObject);
    }
    
}
