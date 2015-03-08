using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Assets.components {

    [Serializable]
    public class VoxelPrototypeFactory : MonoBehaviour {

        public List<VoxelPrototype> prototypes;

        void Awake() {
            for (int i = 0; i < this.prototypes.Count; i++) {
                this.prototypes[i].subMaterialIndex = i;
            }
        }

        void Start() {
        }

        void Update() {
        }

        public VoxelPrototype getPrototype(int index) {
            return this.prototypes[index];
        }

        public int count {
            get {
                return this.prototypes.Count;
            }
        }

    }

}