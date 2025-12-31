using UnityEngine;

public class EnemigoCorredor : EnemyGeneral
{
    [SerializeField] Transform posEliminar;
    protected override void Update()
    {
        if (!isDead)
        {
            rb.linearVelocity = Vector3.left * speed;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D elOtro)
    {
        base.OnTriggerEnter2D(elOtro);
        if (elOtro.gameObject.CompareTag("Despawner")) {
            Destroy(gameObject);
        }
        if (elOtro.gameObject.CompareTag("Player"))
        {
            Morir();
        }
    }

    /*private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Weapon") && !isDamaged && !isDead)
        {
            Morir();
        }
        else if (elOtro.gameObject.CompareTag("Despawner"))
        {
            Destroy(gameObject);
        } else if (elOtro.gameObject.CompareTag("Player"))
        {
            Morir();
        }
    }*/
}
