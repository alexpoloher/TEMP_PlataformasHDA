using UnityEngine;

public class PlataformaCambiaEstado : MonoBehaviour
{

    [SerializeField] float tiempoParaCambiar;
    [SerializeField] bool empiezaActivada;
    private bool estaActiva = false;
    private float tiempoAcumulado;
    private SpriteRenderer spriteRenderer;
    private Collider2D colliderPLataforma;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderPLataforma = GetComponent<Collider2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (empiezaActivada)
        {
            estaActiva = true;
            ActivarPlataforma();
        }
        else
        {
            estaActiva = false;
            DesactivarPlataforma();
        }
    }

    // Update is called once per frame
    void Update()
    {
        tiempoAcumulado += Time.deltaTime;
        if (tiempoAcumulado >= tiempoParaCambiar)
        {
            if (estaActiva)
            {
                DesactivarPlataforma();
            }
            else
            {
                ActivarPlataforma();
            }
            tiempoAcumulado = 0.0f;
        }
    }

    //Al actovarla se activa el collider y se le da su aspecto original
    private void ActivarPlataforma() {
        estaActiva = true;
        colliderPLataforma.enabled = true;
        spriteRenderer.color = new Color32(255, 255, 255, 255);
    }

    //Al desactivarla se desactiva el collider y se le da un aspecto apagado
    private void DesactivarPlataforma()
    {
        estaActiva = false;
        colliderPLataforma.enabled = false;
        spriteRenderer.color = new Color32(70, 70, 70, 136);
    }


}
