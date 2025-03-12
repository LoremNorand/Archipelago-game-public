using NUnit.Framework;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;

public class Industry : Building
{
    [Header("Доход в у.е.")]
    [SerializeReference] internal int income = 10;
    public float timeToProduce = 180f;
    internal float timer = 0f;
    internal bool isProducing = false;
    private void Update()
    {
        if (isProducing)
        {
            timer += Time.deltaTime;
            if (timer >= timeToProduce)
            {
                Produce();
                isProducing = false;
            }
        }
        else
        {
            isProducing = true;
            timer = 0f;
        }
    }
    internal virtual void Produce()
    {
    }

}
