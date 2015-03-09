using Assets.components.abstracts;
using Assets.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.components {
    public class VoxelCubeGeometryDrawer : VoxelGeometryDrawer {

        public override VoxelMeshData draw(Voxel voxel, VoxelWorldChunk chunk, VoxelWorld world, int x, int y, int z, VoxelMeshData meshData) {

            //TODO viene richiamata ma x,y,z sono sempre 0,0,0
            // grassy a x=0 muddy x=1

            //Debug.Log("draw x=" + x + " y=" + y + " z=" + z);

            Voxel topVoxel = world.getVoxelAt(chunk.originX + x, chunk.originY + y + 1, chunk.originZ + z);
            Voxel bottomVoxel = world.getVoxelAt(chunk.originX + x, chunk.originY + y - 1, chunk.originZ + z);
            Voxel backVoxel = world.getVoxelAt(chunk.originX + x, chunk.originY + y, chunk.originZ + z + 1);
            Voxel frontVoxel = world.getVoxelAt(chunk.originX + x, chunk.originY + y, chunk.originZ + z - 1);
            Voxel rightVoxel = world.getVoxelAt(chunk.originX + x + 1, chunk.originY + y, chunk.originZ + z);
            Voxel leftVoxel = world.getVoxelAt(chunk.originX + x - 1, chunk.originY + y, chunk.originZ + z);
            bool isSolid = voxel.prototype.solid;

            //ciascuna faccia viene disegnata se non è presente un voxel adiacente
            //oppure se è presente (con una solidità differente da quella del voxel
            //da disegnare) e trasparente.

            if (topVoxel == null) {
                this.FaceDataUp(voxel, x, y, z, meshData);
            } else { 
                if(topVoxel.prototype.solid != isSolid){
                    if (topVoxel.prototype.solid == false) {
                        this.FaceDataUp(voxel, x, y, z, meshData);
                    }
                }
            }

            if (bottomVoxel == null) {
                this.FaceDataDown(voxel, x, y, z, meshData);
            } else {
                if (bottomVoxel.prototype.solid != isSolid) {
                    if (bottomVoxel.prototype.solid == false) {
                        this.FaceDataDown(voxel, x, y, z, meshData);
                    }
                }
            }

            if (backVoxel == null){
                this.FaceDataNorth(voxel, x, y, z, meshData);
            }else{
                if(backVoxel.prototype.solid != isSolid){
                    if (backVoxel.prototype.solid == false) {
                        this.FaceDataNorth(voxel, x, y, z, meshData);
                    }
                }
            }

            if (frontVoxel == null) {
                this.FaceDataSouth(voxel, x, y, z, meshData);
            } else { 
                if(frontVoxel.prototype.solid != isSolid){
                    if (frontVoxel.prototype.solid == false) {
                        this.FaceDataSouth(voxel, x, y, z, meshData);
                    }
                }
            }

            if (rightVoxel == null){
                this.FaceDataEast(voxel, x, y, z, meshData);
            }else{
                if(rightVoxel.prototype.solid != isSolid){
                    if (rightVoxel.prototype.solid == false) {
                        this.FaceDataEast(voxel, x, y, z, meshData);
                    }
                }
            }

            if (leftVoxel == null) {
                this.FaceDataWest(voxel, x, y, z, meshData);
            } else {
                if (leftVoxel.prototype.solid != isSolid) {
                    if (leftVoxel.prototype.solid == false) {
                        this.FaceDataWest(voxel, x, y, z, meshData);
                    }
                }
            }

            return meshData;
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
