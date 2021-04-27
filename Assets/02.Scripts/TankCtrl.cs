using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCtrl : MonoBehaviour
{
    private new Transform transform;
    public float speed = 30.0f;


    void Start()
    {
        transform = GetComponent<Transform>();
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * v);
        transform.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);
    }
}
