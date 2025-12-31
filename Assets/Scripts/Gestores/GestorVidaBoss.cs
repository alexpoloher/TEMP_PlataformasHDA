using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GestorVidaBoss : MonoBehaviour
{

    [Header("ElementosDelCanvas")]
    [SerializeField] Image iconoVidaBoss;
    [SerializeField] Image barraVidaBoss;
    [SerializeField] Image barraVidaTraseraMovil;
    private float speedBarraTrasera = 0.5f;
    private bool activandoBarraVida;

    public void ActualizarVida(float vidaActual, float vidaMaxima)
    {
        barraVidaBoss.fillAmount = vidaActual / vidaMaxima;
    }

    private void Update()
    {
        if (barraVidaTraseraMovil.fillAmount > barraVidaBoss.fillAmount)
        {
            barraVidaTraseraMovil.fillAmount = Mathf.MoveTowards(barraVidaTraseraMovil.fillAmount, barraVidaBoss.fillAmount, speedBarraTrasera * Time.deltaTime);
        }

        if (activandoBarraVida) { 
            barraVidaBoss.fillAmount = Mathf.MoveTowards(barraVidaBoss.fillAmount, 1.0f, speedBarraTrasera * Time.deltaTime);
            if(barraVidaBoss.fillAmount >= 1.0f)
            {
                activandoBarraVida = false;
                barraVidaTraseraMovil.fillAmount = 1.0f;    //Cuando se rellena la barra de vida, se pone al máximo la trasera
            }
        }


    }

    public void MostrarBarraVidaBoss()
    {
        iconoVidaBoss.gameObject.SetActive(true);
        barraVidaBoss.fillAmount = 0.0f;
        barraVidaTraseraMovil.fillAmount = 0.0f;
        activandoBarraVida = true;

    }

    public void OcultarBarraVida() {
        iconoVidaBoss.gameObject.SetActive(false);
    }

}
