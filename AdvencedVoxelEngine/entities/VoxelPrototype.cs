using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class VoxelPrototype {
    public String name;
    public Material material;
    public bool destructable;

    [NonSerialized]
    public int subMaterialIndex;

    public Voxel instantiateVoxel() {
        Voxel retval = new Voxel(this);
        return retval;
    }
}
