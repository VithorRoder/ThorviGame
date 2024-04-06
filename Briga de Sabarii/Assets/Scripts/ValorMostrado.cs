using UnityEngine;
using UnityEngine.UI;

public class MostrarValorTexto : MonoBehaviour
{
    public Text textoOrigem;
    public Text textoDestino;

    public void MostrarValor()
    {
        textoDestino.text = textoOrigem.text;
    }
}