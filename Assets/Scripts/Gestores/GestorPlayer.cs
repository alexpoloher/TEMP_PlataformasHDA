using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GestorPlayer : MonoBehaviour
{

    public static GestorPlayer Instance;
    private GameObject player;
    private float tiempoEsperaMostrarPantallaMuerte = 1.0f;

    public float vidaActual;
    public float vidaMaxima = 3;
    public float vidaInicial = 3;
    public int monedasTotalesNivel;
    public int monedasRecogidasEnEsteNivel;

    //Control de power ups que se tienen
    private bool tieneLongPunch = false;
    private bool tieneDobleSalto = false;
    private bool tieneDownAttack = false;
    private bool tienepowerUpCombo = false;
    private bool mostrandoPantallaMuerte = false;



    public int intentos = 3;    //Son las vidas extra. Si mueres y tienes de estas, revives
    private int intentosMaximos = 3;
    private Vector3 PosPlayerCheckPoint;    //Posición donde reaparece el jugador si muere

    [SerializeField] float tiempoEsperaEntreEscenas = 3.0f;

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoVictoria;
    [SerializeField] AudioClip sonidoMuerte;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else
        {
            Destroy(gameObject);
        }


    }
    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }*/

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ReiniciarMonedas() {
        monedasRecogidasEnEsteNivel = 0;
        monedasTotalesNivel = 0;
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Objeto"))
        {
            if(item.GetComponent<CoinScript>() != null)
            {
                monedasTotalesNivel++;
            }        
        }
    }

    public bool JugadorTieneTodasMonedas()
    {
        return monedasRecogidasEnEsteNivel >= monedasTotalesNivel;
    }

    public void ObtenerMoneda() { 
        monedasRecogidasEnEsteNivel++;
    }


    public void PasarDeNivel() {
        ImpedirPausa();
        GestorSonido.Instance.EjecutarSonido(sonidoVictoria);
        StartCoroutine(LlamarACargarSiguienteEscena());
    }

    IEnumerator LlamarACargarSiguienteEscena()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas != null) {
            canvas.SetActive(false);
        }

        yield return new WaitForSeconds(tiempoEsperaEntreEscenas);
        GestorScene gestorEscenas = FindFirstObjectByType<GestorScene>();
        if (gestorEscenas != null) {
            gestorEscenas.PasarDeNivel();
        }
    }




    /*public void Victoria()
    {
        if (!estaMuerto)
        {
            StartCoroutine(MostrarVictoria());
        }


    }*/




    public void FinalizarJuego() {
        player.GetComponent<PlayerController>().JuegoCompletado();
        StartCoroutine(JuegoCompletado());

    }

    IEnumerator JuegoCompletado()
    {
        ImpedirPausa();
        GestorSonido.Instance.EjecutarSonido(sonidoVictoria);
        yield return new WaitForSeconds(5f);
        CanvasManager canvasManager = FindFirstObjectByType<CanvasManager>();
        canvasManager.MostrarPantallaJuegoCompletado();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Al comenzar el juego, se establecen los valores de inicio
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        player = GameObject.FindWithTag("Player");

        //Se establecen ciertos parámetros
        if (player!= null && player.GetComponent<PlayerController>() != null)
        {
            player.GetComponent<PlayerController>().RellenarParametrosIniciales(tieneLongPunch, tieneDobleSalto, tieneDownAttack, vidaMaxima, tienepowerUpCombo);
        }

        if (player != null) {
            ReiniciarMonedas();
            EstablecerParametrosInicialesNivel();
        }

        //Si se vuelve al inicio, se resetean ciertos parámetros para la siguiente partida
        if (scene.buildIndex == 0)
        {
            ReiniciarTodosParametros();
        }
        else {
            GestorSonido.Instance.IniciarMusicaNivel(scene.buildIndex);
            StartCoroutine(GestionarPantallaAlInicioDeEscena());
        }

    }



    IEnumerator GestionarPantallaAlInicioDeEscena() {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        GestorCanvasPausa gestorPausa = FindFirstObjectByType<GestorCanvasPausa>();
        if (gestorPausa != null) {
            gestorPausa.puedePausa = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
        if (gestorPausa != null)
        {
            gestorPausa.puedePausa = true;
        }
    }
    public void ImpedirPausa() { 
        GestorCanvasPausa gestorPausa = FindFirstObjectByType<GestorCanvasPausa>();
        gestorPausa.puedePausa = false;
    }

    public void PermitirPausa()
    {
        GestorCanvasPausa gestorPausa = FindFirstObjectByType<GestorCanvasPausa>();
        gestorPausa.puedePausa = true;
    }


    private void ReiniciarTodosParametros() {
        tieneDobleSalto = false;
        tieneLongPunch = false;
        tieneDownAttack = false;
        tienepowerUpCombo = false;
    }

    private void EstablecerParametrosInicialesNivel()
    {
        intentos = intentosMaximos;
        PosPlayerCheckPoint = player.transform.position;
        mostrandoPantallaMuerte = false;
    }

    public void ObtenerLongPunch()
    {
        tieneLongPunch = true;
    }
    public void ObtenerDobleSalto()
    {
        tieneDobleSalto = true;
    }
    public void ObtenerDownAttack()
    {
        tieneDownAttack = true;
    }

    public void ImpedirMovimiento()
    {
        player.GetComponent<PlayerController>().ImpedirMovimientos();
    }
    public void PermitirMovimiento()
    {
        player.GetComponent<PlayerController>().Permitirmovimientos();
    }




    public void QuitarIntento()
    {
        intentos--;
        if (intentos <= 0) {
            //Fin de la partida
            if (!mostrandoPantallaMuerte) {
                mostrandoPantallaMuerte = true;
                StartCoroutine(FinDePartida());
            }
        }
        else
        {
            //Si aún tiene intentos, se reaparece
            StartCoroutine(Reaparecer());
        }
    }

    IEnumerator FinDePartida() {
        ImpedirPausa();
        yield return new WaitForSeconds(tiempoEsperaMostrarPantallaMuerte);
        CanvasManager canvasManager = FindFirstObjectByType<CanvasManager>();
        canvasManager.MostrarPantallaMuerte();
        GestorSonido.Instance.DetenerSonidos();
        GestorSonido.Instance.EjecutarSonido(sonidoMuerte);
    }

    public void ObtenerIntento()
    {
        intentos++;
    }

    public void ObtenerCombo() {
        tienepowerUpCombo = true;
    }


    IEnumerator Reaparecer()
    {
        player.transform.position = PosPlayerCheckPoint;
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerController>().Reaparecer();
    }

    public void GuardarCheckPoint()
    {
        PosPlayerCheckPoint = player.transform.position;

    }




}
