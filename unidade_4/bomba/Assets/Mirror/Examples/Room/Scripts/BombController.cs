using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BombController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCountdownTimeChanged))]
    public float countdownTime = 180.0f; // 3 minutes

    public string code = "TESTE";
    public Text countdownText;
    public InputField codeInput;
    private List<string> validSequences; // Lista das sequências válidas

    public GameObject explosionPrefab; // Prefab de explosão

    private Coroutine countdownCoroutine;

    private int explosionCount = 0;

    public override void OnStartServer()
    {
        base.OnStartServer();

        countdownTime = 10.0f; // Reinicia o contador para 3 minutos
        countdownCoroutine = StartCoroutine(Countdown());
    }

    void Start()
    {
        validSequences = new List<string>()
        {
            "AFD","EBC","BCF", "CBE", 
            "FDC","CED","EAB","EDA",
            "DFC","BAC","FAD","DCE",
            "CBA","BCD","DEF", "AFD",
            "EFA","BAF","CBA", "DCB"
        };
    }

    void Update()
    {
        if (!isServer)
            return;

        code = "TESTE";
        countdownText.text = Mathf.Round(countdownTime).ToString();

        Debug.Log(codeInput.text);
        Debug.Log(code);

        if (countdownTime <= 0.0f)
        {
            Explode();
        }
        else if (!string.IsNullOrEmpty(codeInput.text))
        {
            if (IsValidSequence(codeInput.text))
            {
                Disarm();
            }
        }
    }

    [Server]
    void OnCountdownTimeChanged(float oldTime, float newTime)
    {
        countdownTime = newTime;

        if (countdownTime <= 0.0f)
        {
            Explode();
        }
    }

    IEnumerator Countdown()
    {
        while (countdownTime > 0.0f)
        {
            yield return null;

            if (isServer)
                countdownTime -= Time.deltaTime;
        }
    }

    bool IsValidSequence(string sequence)
    {
        return validSequences.Contains(sequence);
    }

    void Explode()
    {
        if (explosionCount < 20)
        {
            // Instancia o prefab de explosão
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

            // TODO: implement explosion logic
            Debug.Log("BOOM!");

            explosionCount++;
            countdownTime = 180.0f; // Reinicia o contador para 3 minutos

            StartCoroutine(IntervalBetweenExplosions());
        }
        else
        {
            Debug.Log("Final do teste de explosões.");
            Destroy(gameObject);
        }
    }

    IEnumerator IntervalBetweenExplosions()
    {
        yield return new WaitForSeconds(0.5f);
        Explode();
    }

    void Disarm()
    {
        // TODO: implement disarm logic
        Debug.Log("Disarmed!");
        Destroy(gameObject);
    }

    public void ResetBomb()
    {
        explosionCount = 0;
        countdownTime = 180.0f;
    }
}
