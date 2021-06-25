using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    public GameObject[] Blocks;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Instantiate(Blocks[UnityEngine.Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);
    }
}
