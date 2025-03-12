using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float voxelSize = 0.1f;
    public int money = 100;
    public Text moneyTxt = null;
    public int wood = 100;
    public Text woodTxt = null;
    public int stone = 100;
    public Text stoneTxt = null;
    public int metal = 100;
    public Text metalTxt = null;
    public int peopleCount = 100;
    public Text peopleCountTxt = null;

    public void UpdateResources()
    {
        moneyTxt.text = "������: " + money.ToString();
        woodTxt.text = "������:: " + wood.ToString();
        stoneTxt.text = "������: " + stone.ToString();
        metalTxt.text = "������: " + metal.ToString();
        peopleCountTxt.text = "���-�� �����: " + peopleCount.ToString();
    }

    private void Start()
    {
        UpdateResources();
    }
}
