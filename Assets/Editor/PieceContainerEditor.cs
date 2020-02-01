

    using UnityEditor;

    [CustomEditor(typeof(PieceContainer))]
    public class PieceContainerEditor : Editor
    {
        private PieceContainer _this {get{return target as PieceContainer;}}
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            for (int i = 0; i < _this.Count; i++)
            {
                EditorGUILayout.ObjectField(_this.TakePiece(i),typeof(Piece),true);
            }
        }
    }
