using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Moves {
    public struct MoveInfo {
        public string moveID;
        public string characterKey;
        public string moveName;
        public int cost;
        public int value;
        public string description;
        public bool isRelativeForCharacter;
        public Dictionary<string, string[]> variables;
        public int buttonIndex;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID: ");
            sb.AppendLine(moveID);

            sb.Append("Name: ");
            sb.AppendLine(moveName);

            sb.Append("Cost: ");
            sb.AppendLine(cost.ToString());

            sb.Append("Value: ");
            sb.AppendLine(value.ToString());

            sb.Append("Desc: ");
            sb.AppendLine(description);

            sb.Append("IsRelative: ");
            sb.AppendLine(isRelativeForCharacter.ToString());
    
            sb.Append("Variables: ");
            foreach (var command in variables) {
                sb.Append(command.Key);
                sb.Append("(");
                for (int i = 0; i < command.Value.Length; ++i) {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(command.Value[i]);
                }
                sb.AppendLine(")");
            }
            return sb.ToString();
        }
    }
}