public struct Rowcol {
    public int row;
    public int column;
    public Rowcol(int r = 0, int c = 0) {
        row = r;
        column = c;
    }

    public static Rowcol operator -(Rowcol rc) => new Rowcol(-rc.row, -rc.column);

    public static Rowcol operator+(Rowcol rc1, Rowcol rc2)
        => new Rowcol(rc1.row + rc2.row, rc1.column + rc2.column);

    public static Rowcol operator-(Rowcol rc1, Rowcol rc2) => rc1 + (-rc2);

    public override string ToString() {
        return "(" + row.ToString() + ", " + column.ToString() + ")";
    }
}
