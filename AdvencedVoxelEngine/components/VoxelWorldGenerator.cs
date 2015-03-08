using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.components {

    [RequireComponent(typeof(VoxelWorld))]
    [RequireComponent(typeof(VoxelPrototypeFactory))]
    class VoxelWorldGenerator : MonoBehaviour{

        private VoxelWorld _world;
        private VoxelPrototypeFactory _factory;

        void Awake() {
            this._world = this.GetComponent<VoxelWorld>();
            this._factory = this.GetComponent<VoxelPrototypeFactory>();
        }

        void Start() {
            int chunkSize = _world.chunkSize;

            // 2 * 2 * 2 chunkSize
            for (int x = 0; x < 1; x++) {
                for (int y = 0; y < 1; y++) {
                    for (int z = 0; z < 1; z++) {
                        VoxelWorldChunk chunk = _world.getChunkAt(
                            x * chunkSize, y * chunkSize, z * chunkSize
                        );
                        this.generateChunk(chunk);
                    }
                }
            }
        }

        private void generateChunk(VoxelWorldChunk chunk) {
            VoxelPrototype grassyPrototype = this._factory.getPrototype(0);
            VoxelPrototype muddyPrototype = this._factory.getPrototype(1);
            VoxelPrototype sandyPrototype = this._factory.getPrototype(2);

            int chunkSize = this._world.chunkSize;
            for (int x = 0; x < 2; x++) {
                for (int z = 0; z < 1; z++) {
                    for (int y = 0; y < 3; y++) {
                        Voxel voxel;
                        switch (y) { 
                            case 0:
                                voxel = sandyPrototype.instantiateVoxel();
                                break;
                            case 1:
                                voxel = muddyPrototype.instantiateVoxel();
                                break;
                            default:
                                voxel = grassyPrototype.instantiateVoxel();
                                break;
                        }
                        chunk.addVoxelAt(voxel, x, y, z);
                    }
                }
            }
            
            chunk.update();
        }
    }
}
