using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MSLIMA.Serializer;

namespace Moves {
    public struct MoveConfig {
        public string moveID;
        public int executionAreaIndex;
        public Rowcol origin;

        public MoveConfig(string id, int index, Rowcol rc) {
            moveID = id;
            executionAreaIndex = index;
            origin = rc;
        }

        public static byte[] Serialize(object moveObject) {
            MoveConfig moveInfo = (MoveConfig)moveObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize(moveInfo.moveID, ref bytes);
            Serializer.Serialize(moveInfo.executionAreaIndex, ref bytes);
            Serializer.Serialize(moveInfo.origin.row, ref bytes);
            Serializer.Serialize(moveInfo.origin.column, ref bytes);

            return bytes;
        }

        public static object Deserialize(byte[] bytes) {
            MoveConfig moveInfo = new MoveConfig();
            int offset = 0;

            moveInfo.moveID = Serializer.DeserializeString(bytes, ref offset);
            moveInfo.executionAreaIndex = Serializer.DeserializeInt(bytes, ref offset);
            moveInfo.origin.row = Serializer.DeserializeInt(bytes, ref offset);
            moveInfo.origin.column = Serializer.DeserializeInt(bytes, ref offset);
            
            return moveInfo;
        }
    }
}