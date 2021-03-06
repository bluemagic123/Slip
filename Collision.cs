﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Slip
{
    public static class Collision
    {
        public const int tileSize = Tile.tileSize;

        public static Vector2 MovePos(Vector2 position, float width, float height, Vector2 velocity, Room room, out bool collided)
        {
            collided = false;
            if (velocity == Vector2.Zero)
            {
                return position;
            }
            while (velocity != Vector2.Zero)
            {
                Hitbox box = new Hitbox(position, width, height);
                CollideLine? collision = GetNextCollision(box, velocity, room);
                float distance = collision.HasValue ? collision.Value.distance : velocity.Length();
                Vector2 move = velocity;
                move.Normalize();
                move *= distance;
                position += move;
                velocity -= move;
                if (collision.HasValue)
                {
                    switch (collision.Value.type)
                    {
                        case CollideType.X:
                            velocity.X = 0f;
                            break;
                        case CollideType.Y:
                            velocity.Y = 0f;
                            break;
                    }
                    collided = true;
                }
            }
            return position;
        }

        private static CollideLine? GetNextCollision(Hitbox box, Vector2 velocity, Room room)
        {
            CollideLine? xLine = NextXCollision(box, velocity, room);
            CollideLine? yLine = NextYCollision(box, velocity, room);
            if (!xLine.HasValue)
            {
                return yLine;
            }
            if (!yLine.HasValue)
            {
                return xLine;
            }
            return xLine.Value.distance < yLine.Value.distance ? xLine : yLine;
        }

        private static CollideLine? NextXCollision(Hitbox box, Vector2 velocity, Room room)
        {
            if (velocity.X > 0f)
            {
                int x = (int)Math.Ceiling(box.topRight.X / tileSize);
                x = Math.Max(x, 0);
                int end = (int)Math.Floor((box.topRight.X + velocity.X) / tileSize);
                while (x <= end && x < room.width)
                {
                    float xDistance = tileSize * x - box.topRight.X;
                    float yDistance = xDistance / velocity.X * velocity.Y;
                    int y = (int)Math.Floor((box.topRight.Y + yDistance) / tileSize);
                    y = Math.Max(y, 0);
                    int yEnd = (int)Math.Ceiling((box.bottomRight.Y + yDistance) / tileSize) - 1;
                    while (y <= yEnd && y < room.height)
                    {
                        if (SolidTile(room.tiles[x, y]))
                        {
                            return new CollideLine
                            {
                                type = CollideType.X,
                                position = x * tileSize,
                                start = y * tileSize,
                                end = (y + 1) * tileSize,
                                distance = (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance)
                            };
                        }
                        y++;
                    }
                    x++;
                }
            }
            else if (velocity.X < 0f)
            {
                int x = (int)Math.Floor(box.topLeft.X / tileSize);
                x = Math.Min(x, room.width);
                int end = (int)Math.Ceiling((box.topLeft.X + velocity.X) / tileSize);
                while (x >= end && x > 0)
                {
                    float xDistance = tileSize * x - box.topLeft.X;
                    float yDistance = xDistance / velocity.X * velocity.Y;
                    int y = (int)Math.Floor((box.topLeft.Y + yDistance) / tileSize);
                    y = Math.Max(y, 0);
                    int yEnd = (int)Math.Ceiling((box.bottomLeft.Y + yDistance) / tileSize) - 1;
                    while (y <= yEnd && y < room.height)
                    {
                        if (SolidTile(room.tiles[x - 1, y]))
                        {
                            return new CollideLine
                            {
                                type = CollideType.X,
                                position = x * tileSize,
                                start = y * tileSize,
                                end = (y + 1) * tileSize,
                                distance = (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance)
                            };
                        }
                        y++;
                    }
                    x--;
                }
            }
            return null;
        }

        private static CollideLine? NextYCollision(Hitbox box, Vector2 velocity, Room room)
        {
            if (velocity.Y > 0f)
            {
                int y = (int)Math.Ceiling(box.bottomLeft.Y / tileSize);
                y = Math.Max(y, 0);
                int end = (int)Math.Floor((box.bottomLeft.Y + velocity.Y) / tileSize);
                while (y <= end && y < room.height)
                {
                    float yDistance = tileSize * y - box.bottomLeft.Y;
                    float xDistance = yDistance / velocity.Y * velocity.X;
                    int x = (int)Math.Floor((box.bottomLeft.X + xDistance) / tileSize);
                    x = Math.Max(x, 0);
                    int xEnd = (int)Math.Ceiling((box.bottomRight.X + xDistance) / tileSize) - 1;
                    while (x <= xEnd && x < room.width)
                    {
                        if (SolidTile(room.tiles[x, y]))
                        {
                            return new CollideLine
                            {
                                type = CollideType.Y,
                                position = y * tileSize,
                                start = x * tileSize,
                                end = (x + 1) * tileSize,
                                distance = (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance)
                            };
                        }
                        x++;
                    }
                    y++;
                }
            }
            else if (velocity.Y < 0f)
            {
                int y = (int)Math.Floor(box.topLeft.Y / tileSize);
                y = Math.Min(y, room.height);
                int end = (int)Math.Ceiling((box.topLeft.Y + velocity.Y) / tileSize);
                while (y >= end && y > 0)
                {
                    float yDistance = tileSize * y - box.topLeft.Y;
                    float xDistance = yDistance / velocity.Y * velocity.X;
                    int x = (int)Math.Floor((box.bottomLeft.X + xDistance) / tileSize);
                    x = Math.Max(x, 0);
                    int xEnd = (int)Math.Ceiling((box.bottomRight.X + xDistance) / tileSize) - 1;
                    while (x <= xEnd && x < room.width)
                    {
                        if (SolidTile(room.tiles[x, y - 1]))
                        {
                            return new CollideLine
                            {
                                type = CollideType.Y,
                                position = y * tileSize,
                                start = x * tileSize,
                                end = (x + 1) * tileSize,
                                distance = (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance)
                            };
                        }
                        x++;
                    }
                    y--;
                }
            }
            return null;
        }

        private static bool SolidTile(Tile tile)
        {
            return tile.Wall > 0 || (tile.puzzle != null && tile.puzzle.SolidCollision());
        }

        public static bool SectorCollides(Vector2 sectorCenter, float sectorRadius, float sectorAngle1, float sectorAngle2,
            Vector2 targetCenter, float targetRadius)
        {
            float distance = Vector2.Distance(sectorCenter, targetCenter);
            if (distance > sectorRadius + targetRadius)
            {
                return false;
            }
            if (sectorAngle1 > (float)Math.PI)
            {
                sectorAngle1 -= 2f * (float)Math.PI;
            }
            while (sectorAngle2 < sectorAngle1)
            {
                sectorAngle2 += 2f * (float)Math.PI;
            }
            Vector2 normal1 = Helper.AngleToVector2(sectorAngle1 + 0.5f * (float)Math.PI);
            Vector2 normal2 = Helper.AngleToVector2(sectorAngle2 - 0.5f * (float)Math.PI);
            Vector2 testOffset1 = targetCenter + targetRadius * normal1 - sectorCenter;
            Vector2 testOffset2 = targetCenter + targetRadius * normal2 - sectorCenter;
            return Vector2.Dot(normal1, testOffset1) >= 0 && Vector2.Dot(normal2, testOffset2) >= 0;
        }
    }

    enum CollideType
    {
        X,
        Y
    }

    struct CollideLine
    {
        public CollideType type;
        public float position;
        public float start;
        public float end;
        public float distance;
    }

    public struct Hitbox
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
        public float width;
        public float height;

        public Hitbox(Vector2 position, float width, float height)
        {
            this.topLeft = position;
            this.topRight = position;
            this.topRight.X += width;
            this.bottomLeft = position;
            this.bottomLeft.Y += height;
            this.bottomRight = this.bottomLeft;
            this.bottomRight.X += width;
            this.width = width;
            this.height = height;
        }
    }
}
