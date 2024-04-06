using UnityEngine;
using UnityEngine.UI;

public class TrocarPersonagens : MonoBehaviour
{
    public Image imagemDoPersonagem;
    public Sprite[] spritesDosPersonagens;
    public string[] nomesDosPersonagens;
    public int indiceAtual = 0;

    public Text textFieldNomePersonagem;

    public void TrocarParaProximoPersonagem()
    {
        indiceAtual = (indiceAtual + 1) % spritesDosPersonagens.Length;
        imagemDoPersonagem.sprite = spritesDosPersonagens[indiceAtual];
        textFieldNomePersonagem.text = nomesDosPersonagens[indiceAtual];
        
        // Encontra o GameObject com o nome 'AttIndice'
        GameObject objetoComAttIndice = GameObject.Find("AttIndice");
        
        // Obtém o componente TrocarPersonagens do GameObject
        TrocarPersonagens trocarPersonagens = objetoComAttIndice.GetComponent<TrocarPersonagens>();

        // Verifica se o componente foi encontrado
        if (trocarPersonagens != null)
        {
            // Altera o índice atual do outro GameObject
            trocarPersonagens.indiceAtual = indiceAtual;
        }
        else
        {
            Debug.LogError("Componente TrocarPersonagens não encontrado no GameObject.");
        }
    }
}
