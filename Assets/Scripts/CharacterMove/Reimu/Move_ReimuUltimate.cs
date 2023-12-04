using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuUltimate : MoveBase {
        private static readonly string DamageVariableKey = "d1";
        private static readonly string DamageVariable2Key = "d2";
        private static readonly string AnimTriggerKey = "at";

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

        public override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage1 = int.Parse(Info.variables[DamageVariableKey][0]);
            int damage2 = int.Parse(Info.variables[DamageVariable2Key][0]);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                int finalDamage = rc.Equals(Rowcol.Zero) ? damage2 : damage1;
                AttackAtWithTriggerName(
                    caster,
                    target, 
                    finalDamage, 
                    sharedData.GridCtrl, 
                    sharedData.CharcaterCtrl,
                    Info.variables[AnimTriggerKey][0]
                );
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
