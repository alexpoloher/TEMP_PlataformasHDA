using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textoMonedas;
    [SerializeField] TextMeshProUGUI textoIntentos;
    
    [Header("ElementosDelCanvasMuerte")]
    [SerializeField] GameObject fondoMuerte;
    [SerializeField] TextMeshProUGUI textoMuerte;
    [SerializeField] float tiempoEsperarSalir = 3.0f;

    [Header("ElementosDelCanvasVictoria")]
    [SerializeField] GameObject fondoVictoria;
    [SerializeField] TextMeshProUGUI textoVictoria;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textoMonedas.text = GestorPlayer.Instance.monedasRecogidasEnEsteNivel + "/" + GestorPlayer.Instance.monedasTotalesNivel;
        int intentos = GestorPlayer.Instance.intentos;
        if (intentos >= 0) {
            textoIntentos.text = "x" + intentos;
        }

    }

    // Update is called once per frame
    void Update()
    {
        textoMonedas.text = GestorPlayer.Instance.monedasRecogidasEnEsteNivel + "/" + GestorPlayer.Instance.monedasTotalesNivel;
        textoIntentos.text = "x" + GestorPlayer.Instance.intentos;
    }

    public void MostrarPantallaMuerte()
    {

        Color opacidad = textoMuerte.color;
        opacidad.a = 0;
        textoMuerte.color = opacidad;       //Se pone a 0 la opacidad
        fondoMuerte.gameObject.SetActive(true);
        StartCoroutine(MostrarMensajeMuerte());
    }

    IEnumerator MostrarMensajeMuerte()
    {
        while (textoMuerte.color.a < 1f)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            //Se va aumentando gradualmente la opacidad, para que aparezca el texto poco a poco
            Color opacidad = textoMuerte.color;
            opacidad.a = opacidad.a + 0.05f;
            textoMuerte.color = opacidad;
        }

        yield return new WaitForSeconds(tiempoEsperarSalir);
        GestorScene gestorEscenas = FindFirstObjectByType<GestorScene>();
        if (gestorEscenas != null)
        {
            gestorEscenas.VolverAMenuPrincipal();
        }
    }


    public void MostrarPantallaJuegoCompletado()
    {

        Color opacidad = textoVictoria.color;
        opacidad.a = 0;
        textoVictoria.color = opacidad;       //Se pone a 0 la opacidad
        fondoVictoria.gameObject.SetActive(true);
        StartCoroutine(MostrarMensajeVictoria());
    }

    IEnumerator MostrarMensajeVictoria()
    {
        while (textoVictoria.color.a < 1f)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            //Se va aumentando gradualmente la opacidad, para que aparezca el texto poco a poco
            Color opacidad = textoVictoria.color;
            opacidad.a = opacidad.a + 0.05f;
            textoVictoria.color = opacidad;
        }

        yield return new WaitForSeconds(tiempoEsperarSalir);
        GestorScene gestorEscenas = FindFirstObjectByType<GestorScene>();
        if (gestorEscenas != null)
        {
            gestorEscenas.VolverAMenuPrincipal();
        }
    }



}

