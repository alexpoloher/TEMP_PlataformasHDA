using System;
using UnityEngine;

public class CajaRompible : MonoBehaviour
{

    [SerializeField] private Rigidbody2D[] partesCaja;
    [SerializeField] private float golpesResisteCaja;
    [SerializeField] float fuerzaImpulso;
    [SerializeField] float fuerzaGiro;
    [SerializeField] GameObject[] objetosInterior;

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoGolpe;
    [SerializeField] AudioClip sonidoRoto;

    public static Action cajaGolpeada;

    /*[Header("GeneracionObjetos")]
    [SerializeField] private Vector2 rangoFuerza;
    [SerializeField] private float offsetEnY;
    [SerializeField] private Vector2 rangoOffsetEnX;*/

    public void CajaGolpeada()
    {
        golpesResisteCaja -= 1;
        cajaGolpeada?.Invoke();
        if (golpesResisteCaja <= 0)
        {
            RomperCaja();
        }
        else {
            GestorSonido.Instance.EjecutarSonido(sonidoGolpe);
        }


    }

    //Al romper la caja, se hace que las partes sesan propias y se activen, y se destruye la caja completa
    private void RomperCaja() {
        GestorSonido.Instance.EjecutarSonido(sonidoRoto);
        foreach (Rigidbody2D parte in partesCaja)
        {
            parte.transform.SetParent(null);
            parte.gameObject.SetActive(true);

            Vector2 direccionLanzamiento = parte.transform.position - transform.position;   //Dirección a la que saldrña disparada la caja. Restando la pos de la parte con la cjaa, se tiene dónde está situada
            parte.AddForce(direccionLanzamiento * fuerzaImpulso, ForceMode2D.Impulse);
            parte.AddTorque(Mathf.Sign(direccionLanzamiento.x) * fuerzaGiro, ForceMode2D.Impulse); //Para rotar las piezas de la caja. Torque lo rota alrededor de un eje

        }

        GenerarObjetoInterior();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Weapon")) {
            CajaGolpeada();
        }
    }

    private void GenerarObjetoInterior() {

        foreach (GameObject objeto in objetosInterior) {
            //float posXAleatoria = Random.Range(rangoOffsetEnX.x, rangoOffsetEnX.y);
            Instantiate(objeto, transform.position, Quaternion.identity);

            /*if (item.TryGetComponent(out Rigidbody2D rb)) { 

                float fuerzaAleatoria = Random.Range(rangoFuerza.x, rangoFuerza.y);
                Vector2 direccion = item.transform.position - transform.position;
                rb.AddForce(direccion * fuerzaAleatoria, ForceMode2D.Impulse);
            }*/

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
