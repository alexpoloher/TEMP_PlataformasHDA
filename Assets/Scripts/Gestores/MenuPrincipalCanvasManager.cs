using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalCanvasManager : MonoBehaviour
{


    [SerializeField] Image botonNuevaPartida;
    [SerializeField] Image botonSalir;
    [SerializeField] TextMeshProUGUI textoNuevaPartida;
    [SerializeField] TextMeshProUGUI textoSalir;

    [SerializeField] Sprite imagenBotonSinPulsar;
    [SerializeField] Sprite imagenBotonPulsado;

    [Header("Controls")]
    [SerializeField] InputActionReference salir;


    private void OnEnable()
    {
        //Se habilita en este momento la lectura de estos inputs
        salir.action.Enable();
        salir.action.started += OnExit;
    }

    private void OnDisable()
    {
        //Se deshabilita en este momento la lectura de estos inputs
        salir.action.started -= OnExit;
        salir.action.Disable();

    }


    //Al pulsar escape se cierra el juego
    private void OnExit(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }


    public void OnClickNuevaPartida()
    {
        botonNuevaPartida.sprite = imagenBotonPulsado;
        textoNuevaPartida.rectTransform.anchoredPosition = new Vector2(textoNuevaPartida.rectTransform.anchoredPosition.x, -15.0f);
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickSalir()
    {
        botonSalir.sprite = imagenBotonPulsado;
        textoSalir.rectTransform.anchoredPosition = new Vector2(textoSalir.rectTransform.anchoredPosition.x, -15.0f);
        Application.Quit();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        GestorSonido.Instance.DetenerSonidos();
        GestorSonido.Instance.IniciarMusicaMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
