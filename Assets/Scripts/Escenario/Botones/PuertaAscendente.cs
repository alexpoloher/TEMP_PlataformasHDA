using UnityEngine;

public class PuertaAscendente : MonoBehaviour, IElementoActivable
{

    private bool estaActivo = false;
    [SerializeField] float speed = 20.0f;
    [SerializeField] Transform posFinal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (estaActivo && transform.position != posFinal.position) {
            transform.position = Vector2.MoveTowards(transform.position, posFinal.position, speed * Time.deltaTime);
        }
    }

    public void Activar()
    {
        estaActivo = true;
    }

}
