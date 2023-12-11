using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Moves {
    public class Move_HomingAmulet : MoveBase {
        public static int NumOfAmulets = 5;
        private static readonly string DamageVariableKey = "d1";
        private EffectConfig _cachedEffectConfig;

        public Move_HomingAmulet(MoveInfo info) : base(info) {
            InitializeExecutionArea();
            _cachedEffectConfig = new EffectConfig();
        }

        public override void InitializeExecutionArea() {
            ExecutionArea area = new ExecutionArea();
            area.Add(new Rowcol(1, 1));
            area.Add(new Rowcol(-1, 1));
            area.Add(Rowcol.Zero);
            area.Add(new Rowcol(1, -1));
            area.Add(new Rowcol(-1, -1));

            _executionAreas.Add(area);
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);

            _cachedEffectConfig.AreaIndex = areaIndex;
            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                if (sharedData.GridCtrl.IsValidRowcol(target)) {
                    GridObject obj = sharedData.GridCtrl.GetObject(caster.GetOpponent(), target);
                    Vector3 pos = sharedData.GridCtrl.RowcolToPoint(target);
                    _cachedEffectConfig.Add(new EffectTarget(obj as PlayerCharacter, pos));
                }

                AttackAt(caster, target, damage, sharedData.GridCtrl, sharedData.CharcaterCtrl);
                sharedData.GridCtrl.HighlightTile(target);
                sharedData.GridCtrl.HighlightObjectExcept(caster, target);
            }

            PlayerCharacter p = sharedData.CharcaterCtrl.GetCharacterByColor(caster);
            await sharedData.EffectCtrl.StartExecuteEffect(_effectName, p, _cachedEffectConfig, sharedData);

            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;
                sharedData.GridCtrl.RemoveHighlightTile(target);
                sharedData.GridCtrl.RemoveHighlightObjectExcept(caster, target);
            }

            _cachedEffectConfig.Reset();
        }
    }
}
