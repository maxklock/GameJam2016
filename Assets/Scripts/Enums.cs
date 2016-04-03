namespace Assets.Scripts
{
    public enum PlayerId
    {
        None = 0,
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4
    }

    public enum InputType
    {
        None = 0,
        Joystick1 = 1,
        Joystick2 = 2,
        Joystick3 = 3,
        Joystick4 = 4,
        Keyboard = 5
    }

    public enum GameState
    {
        PlayerSelection = 0,
        InGame = 1
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}