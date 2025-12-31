using UnityEngine;

public class PlataformaGiratoriaObjeto : MonoBehaviour
{


    protected virtual void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            elOtro.transform.SetParent(this.transform);
        }

    }

    protected virtual void OnCollisionExit2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            elOtro.transform.SetParent(null);
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
