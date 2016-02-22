﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slip
{
    public class Player
    {
        public static Texture2D texture;
        public const int size = 20;
        public Vector2 position;
        public int life = 1;
        public int maxLife = 1;

        public Vector2 TopLeft
        {
            get
            {
                return position - 0.5f * new Vector2(size, size);
            }
            set
            {
                position = value + 0.5f * new Vector2(size, size);
            }
        }

        public Hitbox hitbox
        {
            get
            {
                return new Hitbox(position, size, size);
            }
        }

        public float radius
        {
            get
            {
                return size / 2f;
            }
        }

        public void Update(Room room)
        {
            Move(room);
            Hitbox box = hitbox;
            int left = (int)Math.Floor(box.topLeft.X / Tile.tileSize);
            int right = (int)Math.Ceiling(box.topRight.X / Tile.tileSize);
            int top = (int)Math.Floor(box.topLeft.Y / Tile.tileSize);
            int bottom = (int)Math.Ceiling(box.bottomLeft.Y / Tile.tileSize);
            for (int x = left; x < right; x++)
            {
                for (int y = left; y < right; y++)
                {
                    Puzzle puzzle = room.tiles[x, y].puzzle;
                    if (puzzle != null)
                    {
                        puzzle.OnPlayerCollide(room, x, y, this);
                    }
                }
            }
        }

        public void Move(Room room)
        {
            Vector2 velocity = Vector2.Zero;
            if (Main.IsKeyPressed(Keys.Up))
            {
                velocity.Y -= 1f;
            }
            if (Main.IsKeyPressed(Keys.Down))
            {
                velocity.Y += 1f;
            }
            if (Main.IsKeyPressed(Keys.Left))
            {
                velocity.X -= 1f;
            }
            if (Main.IsKeyPressed(Keys.Right))
            {
                velocity.X += 1f;
            }
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                velocity *= 4f;
                TopLeft = Collision.MovePos(TopLeft, size, size, velocity, room);
            }
        }

        public void Draw(Main main)
        {
            main.spriteBatch.Draw(texture, main.Center(), null, Color.White, texture.Center());
        }

        public static void LoadContent(ContentManager loader)
        {
            texture = loader.Load<Texture2D>("Player");
        }
    }
}
