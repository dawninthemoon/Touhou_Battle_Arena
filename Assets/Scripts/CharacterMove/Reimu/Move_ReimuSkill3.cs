using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuSkill3 : MoveBase {
        public Move_ReimuSkill3(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            ExecutionArea area = new ExecutionArea();
            area.Add(Rowcol.Zero);
            area.Add(new Rowcol(1, 0));
            area.Add(new Rowcol(-1, 0));

            _executionAreas.Add(area);
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5));

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObjectExcept(caster, target);
            }
        }
    }
}
