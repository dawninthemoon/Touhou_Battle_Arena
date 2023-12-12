using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCommands {
    public abstract class BattleCommand {
        public abstract void Apply(PlayerCharacter character, string[] variables, SharedData sharedData);
    }
}