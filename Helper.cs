﻿using System;
using Microsoft.Xna.Framework;

namespace Slip
{
    public static class Helper
    {
        public static Vector2 PointwiseMult(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }

        public static void Clamp(ref int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            if (value > max)
            {
                value = max;
            }
        }

        public static void GetDirection(Vector2 velocity, ref Direction direction)
        {
            if (velocity == Vector2.Zero)
            {
                return;
            }
            if (velocity.X == 0f)
            {
                direction = velocity.Y > 0f ? Direction.Down : Direction.Up;
            }
            else if (velocity.Y == 0f)
            {
                direction = velocity.X > 0f ? Direction.Right : Direction.Left;
            }
            else if (velocity.X > 0f)
            {
                direction = velocity.Y > 0f ? Direction.DownRight : Direction.UpRight;
            }
            else
            {
                direction = velocity.Y > 0f ? Direction.DownLeft : Direction.UpLeft;
            }
        }

        public static float DirectionToRotation(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return 0f;
                case Direction.DownRight:
                    return (float)Math.PI * 0.25f;
                case Direction.Down:
                    return (float)Math.PI * 0.5f;
                case Direction.DownLeft:
                    return (float)Math.PI * 0.75f;
                case Direction.Left:
                    return (float)Math.PI;
                case Direction.UpLeft:
                    return (float)Math.PI * 1.25f;
                case Direction.Up:
                    return (float)Math.PI * 1.5f;
                case Direction.UpRight:
                    return (float)Math.PI * 1.75f;
                default:
                    return 0f;
            }
        }

        public static Vector2 DirectionToVector2(Direction direction)
        {
            Vector2 vector = Vector2.Zero;
            if (direction == Direction.UpLeft || direction == Direction.Up || direction == Direction.UpRight)
            {
                vector.Y = -1f;
            }
            if (direction == Direction.DownLeft || direction == Direction.Down || direction == Direction.DownRight)
            {
                vector.Y = 1f;
            }
            if (direction == Direction.UpLeft || direction == Direction.Left || direction == Direction.DownLeft)
            {
                vector.X = -1f;
            }
            if (direction == Direction.UpRight || direction == Direction.Right || direction == Direction.DownRight)
            {
                vector.X = 1f;
            }
            vector.Normalize();
            return vector;
        }

        public static Vector2 AngleToVector2(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float Vector2ToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Color FromHSB(float h, float s, float b)
        {
            Color color = Color.White;
            float section = (h - (int)h) * 6f;
            float f = section - (int)section;
            float p = b * (1f - s);
            float q = b * (1f - s * f);
            float t = b * (1f - s * (1f - f));
            switch ((int)section)
            {
                case 0:
                    color.R = (byte)(b * 255f + 0.5f);
                    color.G = (byte)(t * 255f + 0.5f);
                    color.B = (byte)(p * 255f + 0.5f);
                    break;
                case 1:
                    color.R = (byte)(q * 255f + 0.5f);
                    color.G = (byte)(b * 255f + 0.5f);
                    color.B = (byte)(p * 255f + 0.5f);
                    break;
                case 2:
                    color.R = (byte)(p * 255f + 0.5f);
                    color.G = (byte)(b * 255f + 0.5f);
                    color.B = (byte)(t * 255f + 0.5f);
                    break;
                case 3:
                    color.R = (byte)(p * 255f + 0.5f);
                    color.G = (byte)(q * 255f + 0.5f);
                    color.B = (byte)(b * 255f + 0.5f);
                    break;
                case 4:
                    color.R = (byte)(t * 255f + 0.5f);
                    color.G = (byte)(p * 255f + 0.5f);
                    color.B = (byte)(b * 255f + 0.5f);
                    break;
                case 5:
                    color.R = (byte)(b * 255f + 0.5f);
                    color.G = (byte)(p * 255f + 0.5f);
                    color.B = (byte)(q * 255f + 0.5f);
                    break;
            }
            return color;
        }
    }
}
