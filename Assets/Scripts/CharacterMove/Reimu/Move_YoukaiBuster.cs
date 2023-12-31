using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Moves {
    public class Move_YoukaiBuster : MoveBase {
        private static readonly string DamageVariableKey = "d1";
        private static readonly string PaybackAmountKey = "payback";
        private EffectConfig _cachedEffectConfig;

        public Move_YoukaiBuster(MoveInfo info) : base(info) {
            InitializeExecutionArea();
            _cachedEffectConfig = new EffectConfig();
        }

        public override void InitializeExecutionArea() {
             _executionAreas.Add(new ExecutionArea());
            _executionAreas[0].Add(new Rowcol(-1, 1));
            _executionAreas[0].Add(new Rowcol(-1, 0));
            _executionAreas[0].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[1].Add(new Rowcol(1, 1));
            _executionAreas[1].Add(new Rowcol(1, 0));
            _executionAreas[1].Add(new Rowcol(0, 1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[2].Add(new Rowcol(1, -1));
            _executionAreas[2].Add(new Rowcol(1, 0));
            _executionAreas[2].Add(new Rowcol(0, -1)); 

            _executionAreas.Add(new ExecutionArea());
            _executionAreas[3].Add(new Rowcol(-1, -1));
            _executionAreas[3].Add(new Rowcol(-1, 0));
            _executionAreas[3].Add(new Rowcol(0, -1)); 
        }

        protected override async UniTask Execute(TeamColor caster, int areaIndex, Rowcol origin, SharedData sharedData) {
            ExecutionArea area = _executionAreas[areaIndex];
            int damage = int.Parse(Info.variables[DamageVariableKey][0]);
            int paybackAmount = int.Parse(Info.variables[PaybackAmountKey][0]);

            _cachedEffectConfig.AreaIndex = areaIndex;

            bool enemyHit = false;
            foreach (Rowcol rc in area.Rowcols) {
                Rowcol target = origin + rc;

                PlayerCharacter obj = sharedData.GridCtrl.GetObject(caster.GetOpponent(), target) as PlayerCharacter;
                EffectTarget effectTarget = new EffectTarget(obj, sharedData.GridCtrl.RowcolToPoint(target));
                _cachedEffectConfig.Add(effectTarget);

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
