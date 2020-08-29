using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public const int None = 0;
    public const int Up = 1;
    public const int Down = 2;
    public const int Left = 3;
    public const int Right = 4;

    public static int Dir(Vector2Int f, Vector2Int to)
    {
        return Dir(f.x, f.y, to.x, to.y);
    }

    public static int Dir(int fx, int fy, int tox, int toy)
    {
        if (toy > fy)
        {
            return Up;
        }
        else if (toy < fy)
        {
            return Down;
        }

        if (tox > fx)
        {
            return Right;
        }
        else if (tox < fx)
        {
            return Left;
        }

        return None;
    }

    public static Quaternion mQuaternionUp = Quaternion.Euler(0, 0, 0);
    public static Quaternion mQuaternionLeft = Quaternion.Euler(0, 0, 90);
    public static Quaternion mQuaternionDown = Quaternion.Euler(0, 0, 180);
    public static Quaternion mQuaternionRight = Quaternion.Euler(0, 0, 270);


    public static Quaternion Rotation(int dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return mQuaternionUp;
            case Direction.Down:
                return mQuaternionDown;
            case Direction.Left:
                return mQuaternionLeft;
            case Direction.Right:
                return mQuaternionRight;
        }
        return Quaternion.identity;
    }
}
