using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FloodFill
{
    class FloodFiller
    {
        internal LinkedList<Node> list_entry_point = new LinkedList<Node>();
        internal Node[,] matrix_entry_point;

        private LinkedList<Node> list_open = new LinkedList<Node>();
        private int cont_visited = 0;
        
        private int[,] matrix_discrete;
        private float[,] matrix_original;

        internal float correctionValue;
        internal float waterLevel;

        private int rows;
        private int cols;
        private int target;
        
        public FloodFiller(float[,] matrixToScan, float waterLevel, int startRow, int startCol, float correctionValue)
        {
            rows = matrixToScan.GetLength(0);
            cols = matrixToScan.GetLength(1);
            
            this.matrix_original = matrixToScan;
            this.waterLevel = waterLevel;
            this.correctionValue = correctionValue;

            PreProcessing(matrixToScan, waterLevel);
            
            target = matrix_discrete[startRow, startCol];

            FloodFillAlgorithm( SetValue ( new Node(startRow, startCol) ) );
        }

        private void FloodFillAlgorithm(Node startNode )
        {

            cont_visited = 0;
            list_open.AddLast(startNode);

            while( list_open.Count > 0 )
            {
                Node n = list_open.Last.Value;
                list_open.RemoveLast();

                cont_visited++;
                
                if (n.DiscreteValue==-1)   continue;
                if (IsChecked(n))  continue;

                SetChecked(n);

                if (n.DiscreteValue != target) {
                    AddEntryPoint(n);
                } else {
                    list_open.AddLast( SetValue(n.Left() ));
                    list_open.AddLast( SetValue(n.Bottom()) );
                    list_open.AddLast( SetValue(n.Right()) );
                    list_open.AddLast( SetValue(n.Top()) );
                }

                if (list_open.Count > 512 * 512 * 3)
                {
                    Debug.LogError("something is wrong...");
                    return;
                }

            }
        }

        private void AddEntryPoint(Node n)
        {
            if ( n.ListEntryPointParents.Count == 0 )
                list_entry_point.AddLast(matrix_entry_point[n.row, n.col]);
            matrix_entry_point[n.row, n.col].AddEntryPoint(n.Parent);
        }

        private void PreProcessing(float[,] _matrixToScan, float waterLevel)
        {
            matrix_discrete = new int[_matrixToScan.GetLength(0), 
                                    _matrixToScan.GetLength(1)];

            matrix_entry_point = new Node[_matrixToScan.GetLength(0),
                                          _matrixToScan.GetLength(1)];

            for (int r = 0; r < matrix_discrete.GetLength(0); r++)
            {
                for (int c = 0; c < matrix_discrete.GetLength(1); c++)
                {
                    if (_matrixToScan[r, c] < waterLevel + 0.5) 
                        matrix_discrete[r, c] = 0;
                    else
                        matrix_discrete[r, c] = 1;
                }
            }
        }

        internal bool IsOutOfBounds(int row , int col)
        {
            if (col < 0 || col >= cols) return true;
            if (row < 0 || row >= rows) return true;
            return false;
        }

        private bool IsChecked(Node n)
        {
            if (matrix_entry_point[n.row, n.col] == null) 
                return false;
            return true;
        }

        private void SetChecked(Node n)
        {
            matrix_entry_point[n.row, n.col] = n;
        }

        private Node SetValue(Node node)
        {
            if ( IsOutOfBounds(node.row , node.col) )
            {
                node.DiscreteValue = -1;
                node.Value = -1;
                return node;
            }
            else
            {
                node.DiscreteValue = matrix_discrete[node.row, node.col];
                node.Value = matrix_original[node.row, node.col];
                return node;
            }
        }

    }
}
