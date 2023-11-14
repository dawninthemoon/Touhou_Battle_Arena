using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves {
    public class ExecutionArea {
        public List<Rowcol> Rowcols;
        
        public ExecutionArea() {
            Rowcols = new List<Rowcol>();
        }

        public void Add(Rowcol rowcol) {
            Rowcols.Add(rowcol);
        }
    }
}