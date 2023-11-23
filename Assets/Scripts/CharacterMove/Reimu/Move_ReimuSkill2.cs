using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuSkill2 : MoveBase {
        public Move_ReimuSkill2(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            int maxLen = 6;
            for (int directionIdx = 0; directionIdx < Rowcol.directions.Length; ++directionIdx) {
                _executionAreas.Add(new ExecutionArea());
                for (int len = 0; len < maxLen; ++len) {
                    Rowcol rc = Rowcol.Zero + Rowcol.directions[directionIdx] * len;
                    _executionAreas[directionIdx].Add(rc);
                }
            }
        }

        public override async UniTask Execute(TeamColor caster, int areaIndex, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            CharacterTest c = sharedData.CharcaterCtrl.GetCharacterByColor(caster);
            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = c.Curr + rc;
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObject(target);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.25));

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = c.Curr + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObject(target);
            }
        }
    }
}
