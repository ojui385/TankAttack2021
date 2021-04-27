using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankCtrl : MonoBehaviour
{
    private new Transform transform;
    public float speed = 30.0f;
    private PhotonView pv;


    void Start()
    {
        transform = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = transform.Find("CamPivot").transform;
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            transform.Translate(Vector3.forward * Time.deltaTime * speed * v);
            transform.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);
        }
    }
}
