using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Engine.Other
{
    class GizmosManager
    {
        private static System.Random random = new System.Random();

        private static Color SelectColor ( int seed )
        {
            switch (seed)
            {
                case 0:
                    return Color.red;
                case 1:
                    return Color.gray;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.cyan;
                case 4:
                    return Color.green;
                case 5:
                    return Color.magenta;
                case 6:
                    return Color.yellow;
                case 7:
                    return Color.green;
                case 8:
                    return Color.red;
                case 9:
                    return Color.white;

            }
            return Color.black;
        }
    }
}
