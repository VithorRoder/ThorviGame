using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    private Camera mainCamera;
    private Vector3 mousePosition;
    private Vector3 lastRotation;
    public GameObject balaPrefab;
    public Transform balaTransform;
    public bool canFire;
    private float timer;
    public float timeBFire;
    public float force;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (lastRotation != rotation)
        {
            lastRotation = rotation;
            photonView.RPC("SyncRotation", RpcTarget.All, rotZ);
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBFire)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            ShootBullet(rotZ);
        }
    }

    [PunRPC]
    void SyncRotation(float rotZ)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    void ShootBullet(float rotZ)
    {
        GameObject newBullet = Instantiate(balaPrefab, balaTransform.position, Quaternion.Euler(0, 0, rotZ));
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        Vector3 direction = mousePosition - balaTransform.position;
        rb.velocity = direction.normalized * force;

        photonView.RPC("NetworkShootBullet", RpcTarget.Others, balaTransform.position, Quaternion.Euler(0, 0, rotZ));
    }

    [PunRPC]
    void NetworkShootBullet(Vector3 position, Quaternion rotation)
    {
        GameObject newBullet = Instantiate(balaPrefab, position, rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        Vector3 direction = mousePosition - position;
        rb.velocity = direction.normalized * force;
    }
}
