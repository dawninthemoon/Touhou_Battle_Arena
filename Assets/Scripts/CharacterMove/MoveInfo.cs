using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MSLIMA.Serializer;

namespace Moves {
    public struct MoveInfo {
        public string moveID;
        public string moveName;
        public int cost;
        public int value;
        public string description;
        public bool isRelativeForCharacter;
        public Dictionary<string, string[]> commandsInfo;
        public string productionName;

        public static byte[] Serialize(object moveObject) {
            MoveInfo moveInfo = (MoveInfo)moveObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize(moveInfo.moveID, ref bytes);
            Serializer.Serialize(moveInfo.moveName, ref bytes);
            Serializer.Serialize(moveInfo.cost, ref bytes);
            Serializer.Serialize(moveInfo.value, ref bytes);
            Serializer.Serialize(moveInfo.description, ref bytes);
            Serializer.Serialize(moveInfo.isRelativeForCharacter, ref bytes);
            Serializer.Serialize(moveInfo.productionName, ref bytes);

            return bytes;
        }

        public static object Deserialize(byte[] bytes) {
            MoveInfo moveInfo = new MoveInfo();
            int offset = 0;

            moveInfo.moveID = Serializer.DeserializeString(bytes, ref offset);
            moveInfo.moveName = Serializer.DeserializeString(bytes, ref offset);
            moveInfo.cost = Serializer.DeserializeInt(bytes, ref offset);
            moveInfo.value = Serializer.DeserializeInt(bytes, ref offset);
            moveInfo.description = Serializer.DeserializeString(bytes, ref offset);
            moveInfo.isRelativeForCharacter = Serializer.DeserializeBool(bytes, ref offset);
            moveInfo.productionName = Serializer.DeserializeString(bytes, ref offset);
        
            return moveInfo;
        }

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
    
            sb.Append("Commands: ");
            foreach (var command in commandsInfo) {
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