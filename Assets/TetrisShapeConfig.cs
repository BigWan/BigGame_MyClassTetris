namespace BWTetris {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public struct TetrisShape {
        public string name;              // name
        public Vector2 pivotPosition;    // pivot
        public int[] blockPositions;     // poses
        public Color blockColor;         // Color
    }

    [CreateAssetMenu(fileName = "TetrisShapeData",menuName ="创建TetrisData")]
    public class TetrisShapeConfig : ScriptableObject {

        public List<TetrisShape> shapeList;
    }
}
