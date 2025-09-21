using TMPro;
using UnityEngine;

public class PlayerCoinCollector : MonoBehaviour
{

    private int numCoins = 0;

    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCoin()
    {
        numCoins++;
        text.SetText(numCoins.ToString());
    }
}
