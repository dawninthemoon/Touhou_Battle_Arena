using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuSkill2 : MoveBase {
        private static readonly string DamageVariableKey = "d1";

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

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                AttackAt(caster, target, damage, sharedData.GridCtrl, sharedData.CharcaterCtrl);
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
