using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [Header("Intensidade")]
    public float intensidadeMin = 0.8f;
    public float intensidadeMax = 1.5f;

    [Header("Velocidade do Flicker")]
    public float velocidadeMin = 10f;
    public float velocidadeMax = 20f;

    private Light pontoDeLuz;
    private float velocidadeAtual;
    private float tempoDecorrido;

    void Start()
    {
        pontoDeLuz = GetComponent<Light>();

        // Escolhe uma velocidade aleatória inicial
        velocidadeAtual = Random.Range(velocidadeMin, velocidadeMax);
    }

    void Update()
    {
        tempoDecorrido += Time.deltaTime * velocidadeAtual;

        // Perlin Noise dá um flickering mais natural que Sin()
        float ruido = Mathf.PerlinNoise(tempoDecorrido, 0f);
        pontoDeLuz.intensity = Mathf.Lerp(intensidadeMin, intensidadeMax, ruido);

        // Muda a velocidade periodicamente para parecer mais orgânico
        if (Random.value < 0.01f)
        {
            velocidadeAtual = Random.Range(velocidadeMin, velocidadeMax);
        }
    }
}