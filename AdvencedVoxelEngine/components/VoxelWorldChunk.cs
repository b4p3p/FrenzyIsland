using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.components.abstracts;
using Assets.entities;

namespace Assets.components { 

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VoxelWorldChunk : MonoBehaviour {
    
        private Voxel[,,] _basket;
        private int _voxelCount = 0;
        private int _size;
        private bool _updateNotified = true;
        private MeshFilter _filter;
        private MeshCollider _collider;
        private int _originX;
        private int _originY;
        private int _originZ;


        public void Awake() {
            this._filter = this.GetComponent<MeshFilter>();
            this._collider = this.GetComponent<MeshCollider>();
        }

        public void Update() {
            if (this._updateNotified) {
                this._updateNotified = false;
                this.redrawChunk();
            }
        }

        public void update() {
            this._updateNotified = true;
        }

        public void addVoxelAt(Voxel voxel, int x, int y, int z) {
            this._basket[x, y, z] = voxel;
        }

        public void removeVoxelAt(int x, int y, int z) {
            this._basket[x, y, z] = null;
        }

        public bool hasVoxelAt(int x, int y, int z) {
            return this._basket[x,y,z]!=null;
        }

        public Voxel getVoxelAt(int x, int y, int z) {
            return this._basket[x, y, z];
        }

        public int count {
            get {
                return this._voxelCount;
            }
        }


        internal int size {
            get {
                return this._size;
            }

            set {
                this._size = value;
                this._basket = new Voxel[this._size, this._size, this._size];
            }
        }

        internal int originX {
            get {
                return this._originX;
            }

            set {
                this._originX = value;
            }
        }

        internal int originY {
            get {
                return this._originY;
            }

            set {
                this._originY = value;
            }
        }

        internal int originZ {
            get {
                return this._originZ;
            }

            set {
                this._originZ = value;
            }
        }


        private void redrawChunk() {
            VoxelWorld world = this.transform.parent.GetComponent<VoxelWorld>();
            VoxelGeometryDrawer drawer = this.transform.parent.gameObject.GetComponent<VoxelGeometryDrawer>();
            VoxelPrototypeFactory factory = this.transform.parent.gameObject.GetComponent<VoxelPrototypeFactory>();
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();
            VoxelMeshData meshData = new VoxelMeshData();
            
            //estraggo le informazioni di geometria dai voxel...
            if (drawer) {
                for (int x = 0; x < this._basket.GetLength(0); x++) {
                    for (int y = 0; y < this._basket.GetLength(1); y++) {
                        for (int z = 0; z < this._basket.GetLength(2); z++) {
                            Voxel voxel = this._basket[x, y , z ];

                            //TODO con la modifica di originX etc le facce vengono visualizzate bene

                            if (voxel != null) {
                                //comunichiamo al drawer le coordinate globali intere
                                meshData = drawer.draw(voxel, world, x + originX, y + originY, z + originZ, meshData);
                            }
                        }
                    }
                }
            }

            //e le assegno ad una mesh
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.vertices.ToArray();
            mesh.triangles = meshData.triangles.ToArray();
            mesh.uv = meshData.uvs.ToArray();
            mesh.subMeshCount = meshData.submeshCount;

            //ora assegno i materiali in modo da rispecchiare l'ordine delle sottomesh
            Dictionary<int, List<int>> subTriangles = meshData.subTriangles;
            Material[] allMaterials = new Material[meshData.submeshCount];
            int i = 0;
            foreach (int voxelPrototypeIndex in subTriangles.Keys) {
                List<int> submesh = subTriangles[voxelPrototypeIndex];
                Material material = factory.prototypes[voxelPrototypeIndex].material;
                mesh.SetTriangles(submesh.ToArray(), i);
                allMaterials[i] = material;
                i++;
            }
            renderer.materials = allMaterials;

            //ricalcolo le normali della mesh
            mesh.RecalculateNormals();

            //assegno la mesh al filter e al collider
            this._filter.mesh = mesh;
            this._collider.sharedMesh = mesh;
        }

    }

}