using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_ReimuSkill1 : MoveBase {
        private static readonly string DamageVariableKey = "d1";

        public Move_ReimuSkill1(MoveInfo info) : base(info) {
            InitializeExecutionArea();
        }

        public override void InitializeExecutionArea() {
            ExecutionArea area = new ExecutionArea();
            area.Add(Rowcol.Zero);
            area.Add(new Rowcol(1, 1));
            area.Add(new Rowcol(-1, 1));
            area.Add(new Rowcol(1, -1));
            area.Add(new Rowcol(-1, -1));

            _executionAreas.Add(area);

        }

        public override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                DamageAt(caster, target, damage, sharedData.GridCtrl);
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.25));

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObjectExcept(caster, target);
            }
        }
    }
}
