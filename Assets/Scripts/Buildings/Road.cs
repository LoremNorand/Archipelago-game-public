using UnityEngine;

public class 
    Road : Building
{
    public override void SetTransparent(bool available)
    {
        if (available)
        {
            BuildingRenderer.sharedMaterial.color = new Vector4(0, 1, 0, 0.2f);
        }
        else
        {
            BuildingRenderer.sharedMaterial.color = new Vector4(1, 0, 0, 0.2f);
        }
    }
}
