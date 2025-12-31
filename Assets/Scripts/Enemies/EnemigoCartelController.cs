using UnityEngine;

public class EnemigoCartel : EnemyGeneral
{

    protected override void OnTriggerEnter2D(Collider2D elOtro)
    {
        base.OnTriggerEnter2D(elOtro);
        if(tipoEnemigo.Equals(enumTipoEnemigo.Static))
        if (elOtro.gameObject.CompareTag("Player"))
        {
                animator.SetTrigger("PerformAttack");
        }
    }
}
