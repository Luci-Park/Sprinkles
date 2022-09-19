public enum Direction { middle, up, down, left, right, upLeft, upRight, downLeft, downRight, none };


public static class DirectionFunctions{
    private static Direction[] UpBase = { Direction.middle, Direction.down, Direction.up, Direction.right, Direction.left };
    private static Direction[] DownBase = { Direction.middle, Direction.up, Direction.down, Direction.left, Direction.right };
    private static Direction[] LeftBase = { Direction.middle, Direction.right, Direction.left, Direction.up, Direction.down };
    private static Direction[] RightBase = { Direction.middle, Direction.left, Direction.right, Direction.down, Direction.up };

    public static Direction GetRelativeDirection(Direction currentDirection, Direction NextDirection)
    {
        Direction[] directions = DownBase;
        if(Direction.up == currentDirection)
        {
            directions = UpBase;
        }
        else if(Direction.left == currentDirection)
        {
            directions = LeftBase;
        }
        else if(Direction.right == currentDirection)
        {
            directions = RightBase;
        }
        return directions[(int)NextDirection];

    }

}