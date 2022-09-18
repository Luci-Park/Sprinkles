public enum Direction8 { middle, up, down, left, right, upLeft, upRight, downLeft, downRight, none };
public enum Direction4 { up, down, left, right};


public static class DirectionFunctions{
    private static Direction8[] UpBase = { Direction8.middle, Direction8.down, Direction8.up, Direction8.right, Direction8.left };
    private static Direction8[] DownBase = { Direction8.middle, Direction8.up, Direction8.down, Direction8.left, Direction8.right };
    private static Direction8[] LeftBase = { Direction8.middle, Direction8.right, Direction8.left, Direction8.up, Direction8.down };
    private static Direction8[] RightBase = { Direction8.middle, Direction8.left, Direction8.right, Direction8.down, Direction8.up };

    public static Direction8 GetRelativeDirection(Direction8 currentDirection, Direction8 NextDirection)
    {
        Direction8[] directions = DownBase;
        if(Direction8.up == currentDirection)
        {
            directions = UpBase;
        }
        else if(Direction8.left == currentDirection)
        {
            directions = LeftBase;
        }
        else if(Direction8.right == currentDirection)
        {
            directions = RightBase;
        }
        return directions[(int)NextDirection];

    }

}