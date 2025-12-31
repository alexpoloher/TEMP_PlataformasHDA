using UnityEngine;

public class PlataformaMovilPorBoton : PlataformaMovil, IElementoActivable
{

    private bool estaActivo = false;


    // Update is called once per frame
    protected override void Update()
    {
        if (estaActivo)
        {
            base.Update();
        } 
    }

    public void Activar() {
        estaActivo = true;
    }
}
