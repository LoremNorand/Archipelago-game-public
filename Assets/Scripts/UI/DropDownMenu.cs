using UnityEngine;
using System.Collections;


public class DropdownMenu : MonoBehaviour
{
    public RectTransform menuPanel; // Панель, содержащая меню
    public float expandHeight = 200f; // Высота в развернутом состоянии
    public float animationTime = 0.5f; // Время анимации
    public GameObject openButton;

    private bool isExpanded = false;
    private float originalHeight;

    void Start()
    {
        originalHeight = menuPanel.sizeDelta.y; // Исходная высота панели
    }

    public void ToggleMenu()
    {
        
        isExpanded = !isExpanded;
        if(isExpanded)
        {
            openButton.SetActive(false);
        }
        else
            openButton.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(AnimateMenu(isExpanded ? expandHeight : originalHeight));
    }

    private IEnumerator AnimateMenu(float targetHeight)
    {
        float currentHeight = menuPanel.sizeDelta.y;
        float elapsedTime = 0;
        
        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            float newHeight = Mathf.Lerp(currentHeight, targetHeight, elapsedTime / animationTime);
            menuPanel.sizeDelta = new Vector2(menuPanel.sizeDelta.x, newHeight);
            yield return null;
        }

        menuPanel.sizeDelta = new Vector2(menuPanel.sizeDelta.x, targetHeight);
    }
}
