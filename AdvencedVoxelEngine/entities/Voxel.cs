using UnityEngine;
using System.Collections;

public class Voxel {

    private VoxelPrototype _prototype;
    
    public Voxel(VoxelPrototype p){
        this._prototype = p;
    }

    public VoxelPrototype prototype {
        get {
            return this._prototype;
        }
    }
}
