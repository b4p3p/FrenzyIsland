﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;
    public string worldName = "world";

    public int newChunkX;
    public int newChunkY;
    public int newChunkZ;

    public bool genChunk;

	void Start () {

	}
	
	void Update () 
    {
        if (genChunk)
        {
            genChunk = false;
            WorldPos chunkPos = new WorldPos(newChunkX, newChunkY, newChunkZ);
            Chunk chunk = null;

            if (chunks.TryGetValue(chunkPos, out chunk))
            {
                DestroyChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
            else
            {
                CreateChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
        }   
	}

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }

    public void CreateChunk(int x, int y, int z, int[,] heights =  null )
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        if (heights == null)
        {
            CreateBase(newChunk);
        }
        else
        {
            SetChunk(newChunk, heights);
        }

        //var terrainGen = new TerrainGen();
        //newChunk = terrainGen.ChunkGen(newChunk);

        newChunk.SetBlocksUnmodified();
        Serialization.Load(newChunk);
    }

    private void CreateBase(Chunk chunk)
    {
        //Base
        for (int x = 0; x < Chunk.chunkSize; x++) {
            for (int z = 0; z < Chunk.chunkSize; z++) {
                chunk.SetBlock(x, 0, z, new Block());
            }
        }

        //Air
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            for (int z = 0; z < Chunk.chunkSize; z++)
            {
                for (int y = 1; y < Chunk.chunkSize; y++)
                {
                    chunk.SetBlock(x, y, z, new BlockAir());
                }
            }
        }

    }

    private void SetChunk(Chunk chunk, int[,] heights)
    {
        for (int x = 0; x < heights.GetLength(0); x++)
        {
            for (int z = 0; z < heights.GetLength(1); z++)
            {
                for (int y = 0; y < Chunk.chunkSize; y++)
                {
                    if ( heights[x,z] - 1 > chunk.pos.y + y )
                        chunk.SetBlock(x, y, z, new Block());
                    else
                        if (heights[x, z] - 1 == chunk.pos.y + y)
                            chunk.SetBlock(x, y, z, new BlockGrass());
                        else
                            chunk.SetBlock(x, y, z, new BlockAir());
                }
            }
        }
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);    //Add this line to the function
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk containerChunk = null;

        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);

        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }
        else
        {
            return new BlockAir();
        }

    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }



}
