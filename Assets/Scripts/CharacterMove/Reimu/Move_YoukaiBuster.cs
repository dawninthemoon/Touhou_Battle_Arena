using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_YoukaiBuster : MoveBase {
        private static readonly string DamageVariableKey = "d1";
        private static readonly string PaybackAmountKey = "payback";

        public Move_YoukaiBuster(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            _executionAreas.Add(new ExecutionArea());
            _executionAreas[0].Add(new Rowcol(1, 0));
            _executionAreas[0].Add(new Rowcol(1, 1));
            _executionAreas[0].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[1].Add(new Rowcol(-1, 0));
            _executionAreas[1].Add(new Rowcol(-1, 1));
            _executionAreas[1].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[2].Add(new Rowcol(-1, 0));
            _executionAreas[2].Add(new Rowcol(-1, -1));
            _executionAreas[2].Add(new Rowcol(0, -1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[3].Add(new Rowcol(1, 0));
            _executionAreas[3].Add(new Rowcol(1, -1));
            _executionAreas[3].Add(new Rowcol(0, -1)); 
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);
            int paybackAmount = int.Parse(Info.variables[PaybackAmountKey][0]);

            bool enemyHit = false;
            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;

                bool hit = AttackAt(caster, target, damage, sharedData.GridCtrl, sharedData.CharcaterCtrl);
                if (hit) {
                    enemyHit = hit;
                }
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);
            }
            if (enemyHit) {
                sharedData.CharcaterCtrl.GainEnergy(paybackAmount, caster);
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
