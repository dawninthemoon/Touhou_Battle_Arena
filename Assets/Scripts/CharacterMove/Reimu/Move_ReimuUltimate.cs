using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuUltimate : MoveBase {
        public Move_ReimuUltimate(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
             ExecutionArea area = new ExecutionArea();
            area.Add(Rowcol.Zero);
            for (int directionIdx = 0; directionIdx < Rowcol.directions.Length; ++directionIdx) {
                area.Add(Rowcol.directions[directionIdx]);
            }

            _executionAreas.Add(area);
        }

        public override async UniTask Execute(TeamColor caster, int areaIndex, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
                sharedData.GridCtrl.HighlightTile(rc);
                sharedData.GridCtrl.HighlightObject(rc);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.25));

            foreach (Rowcol rc in area.Rowcols) {
                sharedData.GridCtrl.RemoveHighlightTile(rc);
                sharedData.GridCtrl.RemoveHighlightObject(rc);
            }
        }
    }
}
