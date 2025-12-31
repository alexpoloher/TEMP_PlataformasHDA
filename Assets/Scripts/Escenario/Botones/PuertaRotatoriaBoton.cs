using UnityEngine;

public class PuertaRotatoriaBoton : MonoBehaviour, IElementoActivable
{

    private bool estaActivo = false;
    [SerializeField] float velocidadRotacion = 20.0f;
    [SerializeField] float rotacionMaxima = 90.0f;
    private Vector3 direccionRotacion = new Vector3(0, 0, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (estaActivo && transform.rotation != Quaternion.Euler(0,0,rotacionMaxima))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,rotacionMaxima), velocidadRotacion * Time.deltaTime);
        }
    }

    public void Activar()
    {
        estaActivo = true;
    }
}
