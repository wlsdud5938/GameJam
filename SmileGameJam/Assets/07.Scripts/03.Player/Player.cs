using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [Header("UI")]
    public Image healthBarImg;
    public Text healthText;
    public Text coinText;

    [Header("Information")]
    public int maxHealthPoint = 100;
    public int healthPoint = 100;
    public int coin = 0;

    private PlayerAttack playerAttack;
    private PlayerMove playerMove;

    private void Start()
    {
        healthBarImg = GameObject.Find("HealthBar").GetComponent<Image>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();

        playerAttack = transform.GetChild(0).GetComponent<PlayerAttack>();
        playerMove = transform.GetChild(0).GetComponent<PlayerMove>();

        SetInfo();
    }

    private void SetInfo()
    {
        healthBarImg.fillAmount = (float)healthPoint / maxHealthPoint;
        healthText.text = healthPoint.ToString();

        coinText.text = coin.ToString();
    }

    public void TakeDamage(Monster owner, int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
        {
            Debug.Log("Death");
        }
        SetInfo();
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
        SetInfo();
    }

    public void GetCoin(int coin)
    {
        this.coin += coin;
        SetInfo();
    }
}
