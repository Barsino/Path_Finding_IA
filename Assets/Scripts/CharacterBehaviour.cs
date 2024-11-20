using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Locomotion))]
    public class CharacterBehaviour: MonoBehaviour
    {
        
        protected Locomotion LocomotionController;
        protected AbstractPathMind PathController;
        public BoardManager BoardManager { get; set; }
        protected CellInfo currentTarget;

        private void Start()
        {
            PathController = GetComponentInChildren<AbstractPathMind>();
            PathController.SetCharacter(this);
            LocomotionController = GetComponent<Locomotion>();
            LocomotionController.SetCharacter(this);

            if(PathController != null) { Debug.Log(PathController.GetComponent<AbstractPathMind>().name); }
            if(LocomotionController == null) { Debug.Log("Locomotion Null"); }
        }

        void LateUpdate()
        {
            if (BoardManager == null) return;
            if (LocomotionController.MoveNeed)
            {

                var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();
                LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone, LocomotionController.CurrentEndPosition(), new[] { this.currentTarget }));
            }
        }



        public void SetCurrentTarget(CellInfo newTargetCell)
        {
            this.currentTarget = newTargetCell;
        }
    }
}

