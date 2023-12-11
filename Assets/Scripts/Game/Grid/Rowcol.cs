public struct Rowcol {
    public int row;
    public int column;
    public Rowcol(int r = 0, int c = 0) {
        row = r;
        column = c;
    }

    // clockwise
    public static readonly Rowcol[] Directions = new Rowcol[] {
        new Rowcol(-1, 0),
        new Rowcol(-1, 1),
        new Rowcol(0, 1),
        new Rowcol(1, 1),
        new Rowcol(1, 0),
        new Rowcol(1, -1),
        new Rowcol(0, -1),
        new Rowcol(-1, -1)
    };

    public static readonly Rowcol Zero = new Rowcol(0, 0);

    public static Rowcol operator -(Rowcol rc) => new Rowcol(-rc.row, -rc.column);

    public static Rowcol operator+(Rowcol rc1, Rowcol rc2)
        => new Rowcol(rc1.row + rc2.row, rc1.column + rc2.column);

    public static Rowcol operator-(Rowcol rc1, Rowcol rc2) 
        => rc1 + (-rc2);

    public static Rowcol operator*(Rowcol rc, int scalar)
        => new Rowcol(rc.row * scalar, rc.column * scalar);

    public static Rowcol operator*(int scalar, Rowcol rc)
        => rc * scalar;

    public bool Equals(Rowcol other) {
        return (row == other.row) && (column == other.column);
    }
    public override string ToString() {
        return "(" + row.ToString() + ", " + column.ToString() + ")";
    }
}
