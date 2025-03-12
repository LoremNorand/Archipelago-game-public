using Unity.VisualScripting;
using UnityEngine;

public class Residential : Building
{
    [Header("���-�� ����� ������� �������   � ������")]
    [SerializeField] private int countOfPeople = 10;

    public override void SetNormal()
    {
        if (BuildingRenderer != null)
        {
            BuildingRenderer.sharedMaterial.color = Color.white;
        }
        if (isPlaced == false)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.peopleCount += countOfPeople;
                gameManager.UpdateResources();
            }
        }
    }
}
