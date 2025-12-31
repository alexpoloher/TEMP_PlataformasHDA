using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlDialogos : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textoPantalla;
    private Animator animator;
    private Queue<string> colaDialogos = new Queue<string>();   //Una cola para gestionar por orden los textos de los diálogos
    Textos texto;
    public Action cerrarCuadro;
    private bool escribiendoTexto = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivarCartel(Textos textoObjeto)
    {
        colaDialogos.Clear();
        animator.SetBool("AbrirCartel", true);
        texto = textoObjeto;
    }

    public void ActivaTexto()
    {
        colaDialogos.Clear();
        foreach(string textoGuardar in texto.arrayTextos)
        {
            colaDialogos.Enqueue(textoGuardar);
        }
        SiguienteFrase();
    }

    public void SiguienteFrase() {

        if (escribiendoTexto == false)
        {
            if (colaDialogos.Count == 0)
            {
                CerrarCuadroDialogo();
            }
            else
            {
                string fraseActual = colaDialogos.Dequeue();    //Se saca el siguiente texto de la cola                                              //textoPantalla.text = fraseActual;
                StartCoroutine(MostrarCaracteres(fraseActual));
            }
        }

        
    }

    public void CerrarCuadroDialogo()
    {
        animator.SetBool("AbrirCartel", false);
        cerrarCuadro?.Invoke();
    }

    IEnumerator MostrarCaracteres(string texto)
    {
        textoPantalla.text = "";
        escribiendoTexto = true;
        foreach(char character in texto.ToCharArray())
        {
            textoPantalla.text += character;
            yield return new WaitForSeconds(0.02f);
        }
        escribiendoTexto = false;
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
