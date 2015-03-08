using UnityEngine;
using System.Collections;
using System;

[Serializable] 
public class BlockGrass : Block
{

    public BlockGrass() : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 0;
                break;
            case Direction.down:
                tile.x = 1;
                tile.y = 0;
                break;
            default:
                tile.x = 3;
                tile.y = 0;
                break;
        }

        return tile;
    }
}