using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MSLIMA.Serializer;

namespace Moves {
    public struct MoveConfig {
        public string moveID;
        public int executionAreaIndex;

        public static byte[] Serialize(object moveObject) {
            MoveConfig moveInfo = (MoveConfig)moveObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize(moveInfo.moveID, ref bytes);
            Serializer.Serialize(moveInfo.executionAreaIndex, ref bytes);

            return bytes;
        }

        public static object Deserialize(byte[] bytes) {
            MoveConfig moveInfo = new MoveConfig();
            int offset = 0;

            moveInfo.moveID = Serializer.DeserializeString(bytes, ref offset);
            moveInfo.executionAreaIndex = Serializer.DeserializeInt(bytes, ref offset);
            
            return moveInfo;
        }
    }
}