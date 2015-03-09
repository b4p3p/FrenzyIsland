using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.entities;

namespace Assets.components.abstracts{

    public abstract class VoxelGeometryDrawer : MonoBehaviour{

        public abstract VoxelMeshData draw(Voxel voxel, VoxelWorldChunk chunk, VoxelWorld world, int x, int y, int z, VoxelMeshData data);

    }
}
