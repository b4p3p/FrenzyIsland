﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FloodFill
{
    class Shape
    {
        public List<Vector3> ListVertex { get; set; }

        public int Vertex;

        public Shape(List<Vector3> listVertex)
        {
            this.ListVertex = listVertex;
            this.Vertex = listVertex.Count;
        }

        public Vector3 GetCenter()
        {
            throw new NotImplementedException("GetCenter");
        }

        public Vector3 CenterShape {
            get
            {
                if ( _centerShape.x == -1 ) 
                {
                    _centerShape = GetCenter();
                }
                return _centerShape;
            }
            set { _centerShape = value; }
        }
        private Vector3 _centerShape = new Vector3(-1,-1,-1);

        public bool IsMainIsland { get;set; } 
        
        public bool Contains(float x, float z)
        {
            return Contains(ListVertex, new Vector2(x, z));
        }

        private bool Contains( List<Vector3> listVertex, Vector2 pnt)
        {
            float max_x = listVertex.Max(p => p.x);
            float min_x = listVertex.Min(p => p.x);
            float max_y = listVertex.Max(p => p.z);
            float min_y = listVertex.Min(p => p.z);

            if (pnt.x < min_x || pnt.x > max_x) return false;
            if (pnt.x < min_y || pnt.x > max_y) return false;
            
            return true;
        }

        //private bool Contains(List<Vector3> poly, Vector2 p)
        //{
        //    Vector2 p1, p2;
        //    bool inside = false;

        //    if (poly.Count < 3) return false;

        //    Vector2 oldPoint = new Vector2(
        //        poly[poly.Count - 1].x, poly[poly.Count - 1].z);

        //    for (int i = 0; i < poly.Count; i++)
        //    {
        //        Vector2 newPoint = new Vector2(poly[i].x, poly[i].z);

        //        if (newPoint.x > oldPoint.y)
        //        {
        //            p1 = oldPoint;
        //            p2 = newPoint;
        //        }

        //        else
        //        {
        //            p1 = newPoint;
        //            p2 = oldPoint;
        //        }

        //        if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
        //            && (p.y - (long)p1.y) * (p2.x - p1.x)
        //            < (p2.y - (long)p1.y) * (p.x - p1.x))
        //        {
        //            inside = !inside;
        //        }
        //        oldPoint = newPoint;
        //    }
        //    return inside;
        //}
    }
}
