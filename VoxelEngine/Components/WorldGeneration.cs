using UnityEngine;
using System.Collections;
using DiamondSquare;
using FloodFill;
using System;

[RequireComponent(typeof( World ))]
[RequireComponent(typeof( DiamondSeeder ))]

public class WorldGeneration : MonoBehaviour {

    public int Size = 128;
    public int MaxHeight = 16;
    public float Roughness = 0.1f;
    public float WaterPercentage = 0.2f;
    public GameObject Water;

    private World world;
    private DiamondSeeder diamondSeeder;
    private DiamondMatrix diamondMatrix;
    private float[,] heights;
    private float waterLevel;

    private FloodFiller floodFillerWater;
    private FloodFiller floodFillerField;
    private Scanner scannerWater;
    private Scanner scannerField;

	// Use this for initialization
	void Start () {
        
        waterLevel = (float) Math.Ceiling( MaxHeight * WaterPercentage );
        Debug.Log(waterLevel);

        Water.transform.localPosition = new Vector3( Water.transform.localPosition.x, waterLevel , Water.transform.localPosition.z );  
        world = GetComponent<World>();
        
        diamondSeeder = GetComponent<DiamondSeeder>();
        diamondMatrix = new DiamondMatrix(Size, Roughness, diamondSeeder);
        diamondMatrix.Elaborate();

        int[,] matrixDiscrete = diamondMatrix.ToDiscreteMatrix(MaxHeight, 0, 0, Size);
        float[,] _matrixDiscrete = new float[matrixDiscrete.GetLength(0), matrixDiscrete.GetLength(1)];
        
        Array.Copy(matrixDiscrete, _matrixDiscrete, matrixDiscrete.Length);

        floodFillerField = new FloodFiller(_matrixDiscrete, waterLevel, Size / 2, Size / 2, 0.5f);
        floodFillerWater = new FloodFiller(_matrixDiscrete, waterLevel, 0, 0, 0.5f);

        scannerField = new Scanner(floodFillerField);
        scannerWater = new Scanner(floodFillerWater);

        foreach (Node node in floodFillerWater.list_entry_point)
        {
            Debug.DrawLine(new Vector3(node.row, 0, node.col), new Vector3(node.row, 30, node.col), Color.blue, 1000);
        }
        foreach (Node node in floodFillerField.list_entry_point)
        {
            Debug.DrawLine(new Vector3(node.row, 0, node.col), new Vector3(node.row, 30, node.col), Color.green, 1000);
        }

        int cont = 0;

        for (int x = 0; x < diamondMatrix.Size / Chunk.chunkSize; x += 1)
        {
            for (int z = 0; z < diamondMatrix.Size / Chunk.chunkSize; z += 1)
            {
                for (int y = 0; y <= MaxHeight / Chunk.chunkSize ; y++)
                {
                    int[,] subMatrix = diamondMatrix.ToDiscreteMatrix( 
                        MaxHeight, x * Chunk.chunkSize, 
                        z * Chunk.chunkSize, Chunk.chunkSize);
                    
                    world.CreateChunk(x * Chunk.chunkSize, y * Chunk.chunkSize, z * Chunk.chunkSize, 
                                      subMatrix );
                    cont++;
                }
            }
        }

        Debug.Log("Ho creato la bellezza di " + cont + " chunk");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < scannerField.MainIsland.Vertex - 1; i++)
        {
            Vector3 a = scannerField.MainIsland.ListVertex[i];
            Vector3 b = scannerField.MainIsland.ListVertex[i + 1];
            if (Vector3.Distance(a, b) < 2)
                Gizmos.DrawLine(a, b);
        }
    }

}
