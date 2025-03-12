using UnityEngine;

public class StoneMining : Industry
{
    internal override void Produce()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            if (gameManager.peopleCount < 100)
                gameManager.money += income;
            else
                gameManager.money += income * (gameManager.peopleCount / 100);
            gameManager.UpdateResources();
        }
    }
}
