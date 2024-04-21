using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BalaScript : MonoBehaviourPun
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float damage = 5;
    private float destroyTime = 5f;
    private bool collided = false;
    private Vector3 direction;
    public PhotonView pv;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);

        if (!collided)
        {
            this.GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.velocity = transform.up;
        }
    }

    [PunRPC]
    void DestroyBullet()
    {
        if (!collided)
        {
            collided = true;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Player") || other.CompareTag("Bala"))
        {
            if (!collided)
            {
                collided = true;
                PhotonView photonView = this.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
                }
                else
                {
                    Destroy(gameObject);
                }
                if (other.CompareTag("Player"))
                {
                    PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
                    if (ph != null)
                    {
                        ph.photonView.RPC("ReduceHealth", RpcTarget.AllBuffered, damage);
                        ph.photonView.RPC("UpdateHealthUI", RpcTarget.AllBuffered, ph.health, ph.maxHealth);
                    }
                }
            }
        }
    }

}



