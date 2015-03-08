using UnityEngine;
using System.Collections;
using DiamondSquare;
using FloodFill;

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
        
        waterLevel = MaxHeight * WaterPercentage;
        Water.transform.localPosition = new Vector3( Water.transform.localPosition.x, waterLevel , Water.transform.localPosition.z );  
        world = GetComponent<World>();
        
        diamondSeeder = GetComponent<DiamondSeeder>();
        diamondMatrix = new DiamondMatrix(Size, Roughness, diamondSeeder);
        diamondMatrix.Elaborate();

        int[,] matrixDiscrete = diamondMatrix.ToDiscreteMatrix(MaxHeight, 0, 0, Size);

        floodFillerWater = new FloodFiller(matrixDiscrete, waterLevel, 0, 0);
        floodFillerField = new FloodFiller(matrixDiscrete, waterLevel, Size / 2, Size / 2);


        Debug.Log(floodFillerWater.list_entry_point.Count);
        Debug.Log(floodFillerField.list_entry_point.Count);

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
                    //Debug.Log("x=" + x + " y=" + y + " z=" + z);
                    int[,] subMatrix = diamondMatrix.ToDiscreteMatrix( 
                        MaxHeight, x * Chunk.chunkSize, 
                        z * Chunk.chunkSize, Chunk.chunkSize);
                    
                    world.CreateChunk(x * Chunk.chunkSize, y * Chunk.chunkSize, z * Chunk.chunkSize, subMatrix );
                    cont++;
                }
            }
        }

        Debug.Log("Ho creato la bellezza di " + cont + " chunk");

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
