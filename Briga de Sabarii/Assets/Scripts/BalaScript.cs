using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BalaScript : MonoBehaviourPun
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float force;
    private float destroyTime = 5f;
    private bool collided = false;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);


        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);

        if (!collided)
        {
            //photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
            this.GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
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
                    Destroy(gameObject); // Destruir localmente se não for possível RPC
                }
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar a posição e a direção da bala para os outros jogadores
            stream.SendNext(transform.position);
            stream.SendNext(direction);
        }
        else if (stream.IsReading)
        {
            // Receber a posição e a direção da bala dos outros jogadores
            transform.position = (Vector3)stream.ReceiveNext();
            direction = (Vector3)stream.ReceiveNext();
        }
    }
}
