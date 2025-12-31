using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorCanvasPausa : MonoBehaviour
{

    [SerializeField] Image botonContinuar;
    [SerializeField] Image botonSalir;
    [SerializeField] TextMeshProUGUI textoContinuar;
    [SerializeField] TextMeshProUGUI textoSalir;
    [SerializeField] Sprite imagenBotonSinPulsar;
    [SerializeField] Sprite imagenBotonPulsado;

    [SerializeField] GameObject pantallaPausa;
    [Header("Controls")]
    [SerializeField] InputActionReference pause;


    public bool puedePausa = true;
    public bool estaEnPausa = false;

    private void OnEnable()
    {
        //Se habilita en este momento la lectura de estos inputs
        pause.action.Enable();
        pause.action.started += OnPause;

    }

    private void OnDisable()
    {
        pause.action.started -= OnPause;
    }



    private void OnPause(InputAction.CallbackContext obj)
    {
        if (puedePausa && pantallaPausa.activeSelf == false)
        {
            Pausar();
        }
        else {
            if (puedePausa)
            {
                DesPausar();
            }
        }
    }

    private void Pausar() {
        pantallaPausa.SetActive(true);
        estaEnPausa = true;
        Time.timeScale = 0f;    //Pausa el juego
    }

    private void DesPausar()
    {
        Time.timeScale = 1f;    //Despausa el juego
        estaEnPausa = false;
        botonContinuar.sprite = imagenBotonSinPulsar;
        textoContinuar.rectTransform.anchoredPosition = new Vector2(textoContinuar.rectTransform.anchoredPosition.x, 1.0f);
        botonSalir.sprite = imagenBotonSinPulsar;
        textoSalir.rectTransform.anchoredPosition = new Vector2(textoSalir.rectTransform.anchoredPosition.x, 1.0f);

        pantallaPausa.SetActive(false);
    }

    public void onClickContinuar()
    {
        if (pantallaPausa != null)
        {
            botonContinuar.sprite = imagenBotonPulsado;
            textoContinuar.rectTransform.anchoredPosition = new Vector2(textoContinuar.rectTransform.anchoredPosition.x, -15.0f);

            pantallaPausa.SetActive(false);
            DesPausar();
        }

    }

    public void onClickSalir()
    {
        botonSalir.sprite = imagenBotonPulsado;
        textoSalir.rectTransform.anchoredPosition = new Vector2(textoSalir.rectTransform.anchoredPosition.x, -15.0f);

        DesPausar();
        GestorScene gestorEscenas = FindFirstObjectByType<GestorScene>();
        if (gestorEscenas != null)
        {
            gestorEscenas.VolverAMenuPrincipal();
        }
    }


}
