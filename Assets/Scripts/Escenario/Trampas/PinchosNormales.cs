using UnityEngine;

public class PinchosNormales : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Collider2D colliderPinchos;

    private void Awake()
    {
        colliderPinchos = GetComponent<Collider2D>();
    }

    /*private void OnTriggerEnter2D(Collider2D elOtro)
    {
        print("antes " + elOtro.gameObject.tag);
        if (elOtro.gameObject.CompareTag("Weapon") && elOtro.bounds.min.y > colliderPinchos.bounds.max.y)
        {
            print("contacta");
            PlayerController playerController = elOtro.GetComponentInParent<PlayerController>();
            if (playerController != null)
            {
                playerController.ImpulsarArribaPorRebote();
            }
        }
    }*/
}
