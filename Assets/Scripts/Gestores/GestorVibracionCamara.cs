using Unity.Cinemachine;
using UnityEngine;

public class GestorVibracionCamara : MonoBehaviour
{

    [Header("Vibración Cámara")]
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private float vibracionEjeX;
    [SerializeField] private float vibracionEjeY;

    private PlayerController playerController;


    private void OnEnable()
    {
        PlayerController.playerGolpeado += VibrarCamara;
        BossScript.BossGolpeado += VibrarCamaraBoss;
        CajaRompible.cajaGolpeada += VibrarCamaraGolpeoCaja;
    }

    private void OnDisable()
    {
        PlayerController.playerGolpeado -= VibrarCamara;
        BossScript.BossGolpeado -= VibrarCamaraBoss;
        CajaRompible.cajaGolpeada -= VibrarCamaraGolpeoCaja;
    }

    private void VibrarCamara(float vidaPlayer)
    {
        float velocidadAleatoriaX = Random.Range(-vibracionEjeX, vibracionEjeX);
        float velocidadAleatoriaY = Random.Range(-vibracionEjeY, vibracionEjeY);
        cinemachineImpulseSource.GenerateImpulse(new Vector2(velocidadAleatoriaX, velocidadAleatoriaY));
    }

    private void VibrarCamaraBoss()
    {
        float velocidadAleatoriaX = Random.Range(-vibracionEjeX/2, vibracionEjeX/2);
        float velocidadAleatoriaY = Random.Range(-vibracionEjeY/2, vibracionEjeY/2);
        cinemachineImpulseSource.GenerateImpulse(new Vector2(velocidadAleatoriaX, velocidadAleatoriaY));
    }

    private void VibrarCamaraGolpeoCaja()
    {
        float velocidadAleatoriaX = Random.Range(-vibracionEjeX / 2, vibracionEjeX / 2);
        float velocidadAleatoriaY = Random.Range(-vibracionEjeY / 2, vibracionEjeY / 2);
        cinemachineImpulseSource.GenerateImpulse(new Vector2(velocidadAleatoriaX, velocidadAleatoriaY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
