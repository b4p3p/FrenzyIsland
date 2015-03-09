using UnityEngine;
using System.Collections;
using Assets.components;
using DiamondSquare;
using FloodFill;
using System;

[RequireComponent(typeof(VoxelWorld))]
[RequireComponent(typeof(VoxelPrototypeFactory))]
[RequireComponent(typeof(DiamondSeeder))]
public class FrenzyIslandGenerator : MonoBehaviour {

    private VoxelWorld _world;
    private VoxelPrototypeFactory _factory;

    public int Size = 128;
    public int MaxHeight = 16;
    public float Roughness = 0.1f;
    public float WaterPercentage = 0.2f;
    public GameObject Water;

    private DiamondSeeder _diamondSeeder;
    private DiamondMatrix _diamondMatrix;
    private float _waterLevel;

    private FloodFiller floodFillerWater;
    private FloodFiller floodFillerField;
    private Scanner scannerWater;
    private Scanner scannerField;

    void Awake()
    {
        this._world = this.GetComponent<VoxelWorld>();
        this._factory = this.GetComponent<VoxelPrototypeFactory>();
        this._diamondSeeder = GetComponent<DiamondSeeder>();
        this._waterLevel = (float)Math.Ceiling(MaxHeight * WaterPercentage);

        this.Water.transform.localPosition = new Vector3(Water.transform.localPosition.x, _waterLevel, Water.transform.localPosition.z);  
    }

	void Start () {
        CreateNewIsland();
	}

    private void CreateNewIsland()
    {
        _diamondMatrix = new DiamondMatrix(Size, Roughness, _diamondSeeder);
        _diamondMatrix.Elaborate();
        
        int[,] matrixDiscrete = _diamondMatrix.ToDiscreteMatrix(MaxHeight, 0, 0, Size);
        float[,] _matrixDiscrete = new float[matrixDiscrete.GetLength(0), matrixDiscrete.GetLength(1)];

        Array.Copy(matrixDiscrete, _matrixDiscrete, matrixDiscrete.Length);

        floodFillerField = new FloodFiller(_matrixDiscrete, _waterLevel, Size / 2, Size / 2, 0.5f);
        floodFillerWater = new FloodFiller(_matrixDiscrete, _waterLevel, 0, 0, 0.5f);

        scannerField = new Scanner(floodFillerField);
        scannerWater = new Scanner(floodFillerWater);

        int cont = 0;

        /*
        int chunkSize = _world.chunkSize;
        VoxelWorldChunk chunk = _world.getChunkAt( 0, 0, 0 );
        this.generateChunkTest(chunk, 0);

        VoxelWorldChunk chunk2 = _world.getChunkAt(chunk.size, 0, 0);
        this.generateChunkTest(chunk2, 0);
        */

        for (int x = 0; x < _diamondMatrix.Size / _world.chunkSize; x += 1) {
            for (int z = 0; z < _diamondMatrix.Size / _world.chunkSize; z += 1) {
                for (int y = 0; y <= MaxHeight / _world.chunkSize; y++) {
                    int chunkSize = _world.chunkSize;

                    int[,] subMatrix = _diamondMatrix.ToDiscreteMatrix(
                        MaxHeight,
                        x * chunkSize,
                        z * chunkSize,
                        _world.chunkSize);

                    VoxelWorldChunk chunk = _world.getChunkAt(
                            x * chunkSize, y * chunkSize, z * chunkSize
                        );

                    this.generateChunk(chunk, subMatrix);
                    //this.generateChunkTest(chunk, y * chunkSize);

                    cont++;
                }
            }
        }

        Debug.Log("Creati " + cont + " chunk");
    }

    private void generateChunk(VoxelWorldChunk chunk, int[,] subMatrix)
    {
        int offset_y = chunk.originY;

        VoxelPrototype grassyPrototype = this._factory.getPrototype(0);
        VoxelPrototype muddyPrototype = this._factory.getPrototype(1);
        VoxelPrototype sandyPrototype = this._factory.getPrototype(2);

        for (int x = 0; x < subMatrix.GetLength(0); x++)
        {
            for (int z = 0; z < subMatrix.GetLength(1); z++)
            {
                Voxel voxel = null;
                int valueChunk = subMatrix[x,z] - offset_y;  //prendo solo i valori che rientrano nel chunk

                if (valueChunk <= 0) 
                    continue;

                //sabbia solo per le colonne alte 1
                if (valueChunk == 1 && offset_y == 0)
                {
                    voxel = sandyPrototype.instantiateVoxel();
                    chunk.addVoxelAt(voxel, x, 0, z);
                    continue;
                }

                for (int y = 0; y < _world.chunkSize && y < valueChunk; y++)
                {
                    if ( subMatrix[x, z] == valueChunk + offset_y )
                    {
                        voxel = grassyPrototype.instantiateVoxel();

                    } 
                    else if ( subMatrix[x, z] < valueChunk + offset_y )
                    {
                        voxel = muddyPrototype.instantiateVoxel();
                    }

                    chunk.addVoxelAt(voxel, x, y, z);
                }

               
            }
        }

        
        chunk.update();

    }

    private void generateChunkTest(VoxelWorldChunk chunk, float y_offset)
    {
        VoxelPrototype grassyPrototype = this._factory.getPrototype(0);
        VoxelPrototype muddyPrototype = this._factory.getPrototype(1);
        VoxelPrototype sandyPrototype = this._factory.getPrototype(2);

        for (int x = 0; x < _world.chunkSize; x++)
        {
            for (int z = 0; z < _world.chunkSize; z++)
            {
                for (int y = 0; y < _world.chunkSize; y++)
                {
                    if (chunk.originX == 0)
                    {
                        Voxel voxelGrassy = grassyPrototype.instantiateVoxel();
                        chunk.addVoxelAt(voxelGrassy, x, y, z);
                    }
                    else
                    {
                        Voxel voxelGrassy = muddyPrototype.instantiateVoxel();
                        chunk.addVoxelAt(voxelGrassy, x, y, z);
                    }
                }

            }
        }
        Debug.Log("update");
        chunk.update();
    }

	// Update is called once per frame
	void Update () {
	
	}
}

