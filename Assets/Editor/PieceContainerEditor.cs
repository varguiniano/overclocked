

    using UnityEditor;

    [CustomEditor(typeof(PieceContainer))]
    public class PieceContainerEditor : Editor
    {
        private PieceContainer _this {get{return target as PieceContainer;}}
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
