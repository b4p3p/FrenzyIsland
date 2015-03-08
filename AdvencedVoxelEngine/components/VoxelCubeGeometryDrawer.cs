using Assets.components.abstracts;
using Assets.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.components {
    public class VoxelCubeGeometryDrawer : VoxelGeometryDrawer {

        public override void draw(Voxel voxel, VoxelWorld world, int x, int y, int z, VoxelMeshData meshData) {
            if (!world.hasVoxelAt(x, y + 1, z)) {
                this.FaceDataUp(voxel, x, y, z, meshData);
            }

            if (!world.hasVoxelAt(x, y - 1, z)){
                this.FaceDataDown(voxel, x, y, z, meshData);
            }

            if (!world.hasVoxelAt(x, y, z + 1)) {
                this.FaceDataNorth(voxel, x, y, z, meshData);
            }

            if (!world.hasVoxelAt(x, y, z - 1)) {
                this.FaceDataSouth(voxel, x, y, z, meshData);
            }

            if (!world.hasVoxelAt(x + 1, y, z)) {
                this.FaceDataEast(voxel, x, y, z, meshData);
            }

            if (!world.hasVoxelAt(x - 1, y, z)) {
                this.FaceDataWest(voxel, x, y, z, meshData);
            }
        }


        protected void FaceDataUp(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.addUV(new Vector2(0.0f, 1.0f));
            meshData.addUV(new Vector2(0.25f, 1.0f));
            meshData.addUV(new Vector2(0.25f, 0.75f));
            meshData.addUV(new Vector2(0.0f, 0.75f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }

        protected void FaceDataDown(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.addUV(new Vector2(0.25f, 1.0f));
            meshData.addUV(new Vector2(0.50f, 1.0f));
            meshData.addUV(new Vector2(0.50f, 0.75f));
            meshData.addUV(new Vector2(0.25f, 0.75f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }



        protected void FaceDataNorth(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.addUV(new Vector2(0.25f, 0.50f));
            meshData.addUV(new Vector2(0.25f, 0.75f));
            meshData.addUV(new Vector2(0.50f, 0.75f));
            meshData.addUV(new Vector2(0.50f, 0.50f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }

        protected void FaceDataEast(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.addUV(new Vector2(0.50f, 0.75f));
            meshData.addUV(new Vector2(0.50f, 1.0f));
            meshData.addUV(new Vector2(0.75f, 1.0f));
            meshData.addUV(new Vector2(0.75f, 0.75f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }

        protected void FaceDataSouth(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.addUV(new Vector2(0.75f, 0.75f));
            meshData.addUV(new Vector2(0.75f, 1.0f));
            meshData.addUV(new Vector2(1.0f, 1.0f));
            meshData.addUV(new Vector2(1.0f, 0.75f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }

        protected void FaceDataWest(Voxel voxel, int x, int y, int z, VoxelMeshData meshData) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.addUV(new Vector2(0.0f, 0.50f));
            meshData.addUV(new Vector2(0.0f, 0.75f));
            meshData.addUV(new Vector2(0.25f, 0.75f));
            meshData.addUV(new Vector2(0.25f, 0.50f));
            this.addTriangles(voxel.prototype.subMaterialIndex, meshData);
        }

        protected void addTriangles(int voxelPrototypeId, VoxelMeshData meshData) {
            int vCount = meshData.vertexCount;
            meshData.addTriangle(voxelPrototypeId, vCount-4, vCount-3, vCount-2);
            meshData.addTriangle(voxelPrototypeId, vCount-4, vCount-2, vCount-1);
        }
        

    }
}
