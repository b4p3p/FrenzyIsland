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
            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 1; y++) {
                    for (int z = 0; z < 10; z++) {
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
            VoxelPrototype waterPrototype = this._factory.getPrototype(3);

            int chunkSize = this._world.chunkSize;
            for (int x = 0; x < this._world.chunkSize; x++) {
                for (int z = 0; z < this._world.chunkSize; z++) {
                    for (int y = 0; y < 4; y++) {
                        Voxel voxel;
                        switch (y) {
                            case 0:
                            case 1:
                                voxel = sandyPrototype.instantiateVoxel();
                                break;
                            default:
                                voxel = waterPrototype.instantiateVoxel();
                                break;
                        }
                        chunk.addVoxelAt(voxel, x, y, z);
                    }
                }
            }

            for (int x = 5; x < 11; x++) {
                for (int z = 5; z < 11; z++) {
                    for (int y = 0; y < 6; y++) {
                        Voxel voxel;
                        if (y == 5) {
                            voxel = grassyPrototype.instantiateVoxel();
                        } else {
                            voxel = muddyPrototype.instantiateVoxel();
                        }
                        chunk.addVoxelAt(voxel, x, y, z);
                    }
                }
            }
            
            chunk.update();
        }
    }
}
