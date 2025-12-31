using UnityEngine;

public class LayoutCorazones : MonoBehaviour
{

    [SerializeField] CorazonVida[] losCorazones;
    private PlayerController playerController;


    private void ActualizarCorazones(float vidaPlayer) { 
   
        int indice = 0;
        foreach(CorazonVida corazon in losCorazones)
        {
            if(indice < vidaPlayer) //Si el indice es 1 (el segundo corazón), y la vida es 2, entonces tiene dos corazones, asique ese corazón hay que activarlo
            {
                if(corazon.activo == false)
                {
                    corazon.AddCorazon();
                }
            }
            else
            {
                if (corazon.activo == true)
                {
                    corazon.QuitarCorazon();
                }
            }
            indice++;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            PlayerController.playerGolpeado += ActualizarCorazones;
            PlayerController.playerCuraVida += ActualizarCorazones;
        }

        ActualizarCorazones(GestorPlayer.Instance.vidaMaxima);
    }

    private void OnDisable()
    {
        PlayerController.playerGolpeado -= ActualizarCorazones;
        PlayerController.playerCuraVida -= ActualizarCorazones;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
