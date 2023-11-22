using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class ExecutionArea {
        public HashSet<Rowcol> Rowcols;
        public int Size {
            get { return Rowcols.Count; }
        }
        
        public ExecutionArea() {
            Rowcols = new HashSet<Rowcol>();
        }

        public void Add(Rowcol rowcol) {
            Rowcols.Add(rowcol);
        }

        public bool Contains(Rowcol rowcol) {
            return Rowcols.Contains(rowcol);
        }
    }
}