using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    
    public Renderer BuildingRenderer;
    public Vector2Int Size = new Vector2Int();
    public Vector2Int? PreviousPosition = null;
    public Cost cost;
    internal bool isPlaced = false;
    internal GameManager gameManager;


    public virtual void SetTransparent(bool available)
    {
        if (BuildingRenderer != null)
        {
            if (available)
            {
                BuildingRenderer.material.color = new Vector4(0, 1, 0, 0.2f);
            }
            else
            {
                BuildingRenderer.material.color = new Vector4(1, 0, 0, 0.2f);
            }
        }
    }

    public void Rotate()
    {
        this.transform.Rotate(0, 90, 0);
        Vector2Int temp = Size;
        Size.x = temp.y;
        Size.y = temp.x;
    }

    public virtual void SetNormal()
    {
        if (BuildingRenderer != null)
        {   
            BuildingRenderer.sharedMaterial.color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 centerOffset = new Vector3(Size.x * 0.5f, 0, Size.y * 0.5f);
        for (int x = 0; x < Size.x; x++)
        {
            for (int z = 0; z < Size.y; z++)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                Gizmos.DrawCube(transform.position - centerOffset + new Vector3(x + 0.5f, 0, z + 0.5f), new Vector3(1, 0.1f, 1));
            }
        }
    }
}
