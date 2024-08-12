using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class BalaScript : MonoBehaviourPun
{
    private Rigidbody2D rb;
    public float damage = 5;
    private bool collided = false;
    public PhotonView photonbala;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonbala = GetComponent<PhotonView>();
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (!collided)
        {
            if (photonbala != null && photonbala.IsMine)
            {
                photonbala.RPC("DestroyBullet", RpcTarget.AllBuffered);
            }
            else
            {
                Destroy(gameObject);
            }
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
        if (other.CompareTag("Ground"))
        {
            HandleCollision();
            return;
        }

        string bulletTag = gameObject.tag;
        string playerTag = other.gameObject.tag;

        if (other.CompareTag("BalaAzul") || other.CompareTag("BalaVermelha") || other.CompareTag("BalaRosa") || other.CompareTag("BalaVerde") || other.CompareTag("BalaAmarela") || other.CompareTag("BalaBranca"))
        {
            HandleCollision();
            return;
        }

        switch (bulletTag)
        {
            case "BalaAzul":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerVermelho", "PlayerRosa", "PlayerVerde", "PlayerAmarelo", "PlayerBranco"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            case "BalaVermelha":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerAzul", "PlayerRosa", "PlayerVerde", "PlayerAmarelo", "PlayerBranco"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            case "BalaRosa":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerVermelho", "PlayerAzul", "PlayerVerde", "PlayerAmarelo", "PlayerBranco"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            case "BalaVerde":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerVermelho", "PlayerRosa", "PlayerAzul", "PlayerAmarelo", "PlayerBranco"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            case "BalaAmarela":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerVermelho", "PlayerRosa", "PlayerVerde", "PlayerAzul", "PlayerBranco"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            case "BalaBranca":
                if (IsPlayerAllowedToBeHit(playerTag, "PlayerVermelho", "PlayerRosa", "PlayerVerde", "PlayerAmarelo", "PlayerAzul"))
                {
                    HandleCollision();
                    DamagePlayer(other.gameObject);
                }
                break;

            default:
                break;
        }
    }

    bool IsPlayerAllowedToBeHit(string playerTag, params string[] allowedTags)
    {
        return allowedTags.Contains(playerTag);
    }

    [PunRPC]
    void HandleCollision()
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
        }
    }

    void DamagePlayer(GameObject player)
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.photonView.RPC("ReduceHealth", RpcTarget.AllBuffered, damage);
            ph.photonView.RPC("UpdateHealthUI", RpcTarget.AllBuffered, ph.health, ph.maxHealth);
        }
    }

}

