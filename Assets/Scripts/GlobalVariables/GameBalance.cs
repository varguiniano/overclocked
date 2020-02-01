using UnityEngine;

namespace GlobalVariables
{
    [CreateAssetMenu(menuName = "Whatever/Game balancing", fileName = "Game balancing")]
    public class GameBalance : ScriptableObject
    {
        public bool GodMode;
        
        public AnimationCurve SpawnSpeedRate;

        public AnimationCurve ConveyorSpeedRate;

        public AnimationCurve BrokenPiecesRate;
    }
}