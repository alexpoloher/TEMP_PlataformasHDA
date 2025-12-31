using System.Collections;
using UnityEngine;

public class EnemigoEsqueletoController : EnemyGeneral
{
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform puntoDisparo;
    [SerializeField] float delayDisparos = 3.0f;
    private bool puedeDisparar = true;

    protected override void Start()
    {
        base.Start();
        if (tipoEnemigo.Equals(enumTipoEnemigo.Static) && transform.localScale.x < 0)
        {
            animator.SetTrigger("PerformAttack");
            puntoDisparo.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    protected override void Update()
    {

        if (tipoEnemigo.Equals(enumTipoEnemigo.Static) && puedeDisparar)
        {
            puedeDisparar = false;
            animator.SetTrigger("PerformAttack");
        } else if (tipoEnemigo.Equals(enumTipoEnemigo.Patrulla)) {

            if (transform.localScale.x < 0)
            {
                puntoDisparo.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                puntoDisparo.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (!isWaiting)
            {

                 //Ver si ya ha llegado al punto final
                if (!isRight)
                {
                    if (Vector2.Distance(transform.position, puntoA.position) < 0.2f && puedeDisparar)
                    {
                        puedeDisparar = false;
                        animator.SetTrigger("PerformAttack");
                    }
                }
                else  //Si va hacia B
                {
                    if (Vector2.Distance(transform.position, puntoB.position) < 0.2f && puedeDisparar)
                    {
                        puedeDisparar = false;
                        animator.SetTrigger("PerformAttack");
                    }
                }
            }
        }

            base.Update();
    }

    public void GenerarProyectil()
    {
        Instantiate(prefabProyectil, puntoDisparo.position,puntoDisparo.rotation);
        StartCoroutine(RecargarProyectil());
    }
    IEnumerator RecargarProyectil() {

        yield return new WaitForSeconds(delayDisparos);
        puedeDisparar = true;
    }

}
