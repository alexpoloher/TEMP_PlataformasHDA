using UnityEngine;

[System.Serializable]   //Indica que el script no va a estar asociado a ningún objeto, solo se le va a llamar desde otro script
public class Textos 
{
    [TextArea(2, 4)]    //Indica las líneas mínimas y máximas de un  cuadro de texto
    public string[] arrayTextos;
}
