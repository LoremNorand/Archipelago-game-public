using System.Data.SqlTypes;
using UnityEngine;

public class Cost : MonoBehaviour
{
    public int money = 25;
    public int wood = 25;
    public int stone = 25;
    public int metal = 25;
    public GameManager gameManager;

    public bool IsCanBeBought()
    {
        gameManager = GameObject.FindGameObjectWithTag("gM").GetComponent<GameManager>();
        if (gameManager != null)
        {
            if (money <= gameManager.money && wood <= gameManager.wood && stone <= gameManager.stone && metal <= gameManager.metal)
            {
                return true;
            }
        }
        else
            Debug.LogAssertion("GameManager not found");
        return false;
    }

    public void BuyBuilding()
    {
        gameManager = null;
        gameManager = GameObject.FindGameObjectWithTag("gM").GetComponent<GameManager>();
        if (gameManager != null)
        {
            gameManager.money -= money;
            gameManager.wood -= wood;
            gameManager.stone -= stone;
            gameManager.metal -= metal;
            gameManager.UpdateResources();
        }
        else
        {
            Debug.LogAssertion("GameManager not found");
        }
    }
}
