using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour
{
    public float countdownTime = 180.0f; // 3 minutes
    public string code = "TESTE";
    public Text countdownText;
    public InputField codeInput;

    void Start()
    {
        StartCoroutine(Countdown());
        codeInput.ActivateInputField();
    }

    void Update()
    {   
        code = "TESTE";
        countdownText.text = Mathf.Round(countdownTime).ToString();
        
        Debug.Log(codeInput.text);
        Debug.Log(code);

        if (countdownTime <= 0.0f)
        {
            Explode();
        }
        else if (codeInput.text == code)
        {
            Disarm();
        }
    }

    IEnumerator Countdown()
    {
        while (countdownTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1.0f;
        }
    }

    void Explode()
    {
        // TODO: implement explosion logic
        Debug.Log("BOOM!");
        Destroy(gameObject);
    }

    void Disarm()
    {
        // TODO: implement disarm logic
        Debug.Log("Disarmed!");
        Destroy(gameObject);
    }
}
