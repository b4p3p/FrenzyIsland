using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.entities {
    
    public class VoxelMeshData {

        private List<int> _triangles;
        private List<Vector3> _vertices;
        private List<Vector2> _uvs;
        private Dictionary<int, List<int>> _subtriangles;

        public VoxelMeshData() {
            this._triangles = new List<int>();
            this._vertices = new List<Vector3>();
            this._uvs = new List<Vector2>();
            this._subtriangles = new Dictionary<int, List<int>>();
        }

        public void addTriangle(int type, int firstIndex, int secondIndex, int thirdIndex) {
            if (!this._subtriangles.ContainsKey(type)) {
                this._subtriangles.Add(type, new List<int>());
            }
            List<int> subTrianglesList = this._subtriangles[type];
            subTrianglesList.Add(firstIndex);
            subTrianglesList.Add(secondIndex);
            subTrianglesList.Add(thirdIndex);

            this._triangles.Add(firstIndex);
            this._triangles.Add(secondIndex);
            this._triangles.Add(thirdIndex);
        }

        public void addVertex(Vector3 vertex) {
            this._vertices.Add(vertex);
        }

        public void addUV(Vector2 uv) {
            this._uvs.Add(uv);
        }

        public Dictionary<int, List<int>> subTriangles {
            get {
                return this._subtriangles;
            }
        }

        public List<int> triangles {
            get {
                return this._triangles;
            }
        }

        public List<Vector2> uvs {
            get {
                return this._uvs;
            }
        }

        public List<Vector3> vertices {
            get {
                return this._vertices;
            }
        }

        

        public int vertexCount {
            get {
                return this._vertices.Count;
            }
        }

        public int submeshCount {
            get {
                return this._subtriangles.Keys.Count;
            }
        }

        public int[] submeshIndices {
            get {
                return this._subtriangles.Keys.ToArray();
            }
        }
    }


}
