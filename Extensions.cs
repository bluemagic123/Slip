﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slip
{
    public static class Extensions
    {
        public static Vector2 Center(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
            Color color, Vector2 origin, bool flip = false)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0f, origin, 1f,
                flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
        }

        public static void DrawCenteredString(this SpriteBatch spriteBatch, SpriteFont spriteFont,
            string text, Vector2 position, Color color)
        {
            Vector2 size = spriteFont.MeasureString(text);
            spriteBatch.DrawString(spriteFont, text, position - size / 2f, color);
        }

        public static void DrawBorderString(this SpriteBatch spriteBatch, SpriteFont spriteFont,
            string text, Vector2 position, Color color, Color borderColor, int borderSize = 1)
        {
            for (int x = -borderSize; x <= borderSize; x++)
            {
                for (int y = -borderSize; y <= borderSize; y++)
                {
                    spriteBatch.DrawString(spriteFont, text, position + new Vector2(x, y), borderColor);
                }
            }
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public static void DrawCenteredBorderString(this SpriteBatch spriteBatch, SpriteFont spriteFont,
            string text, Vector2 position, Color color, Color borderColor, int borderSize = 1)
        {
            Vector2 size = spriteFont.MeasureString(text);
            spriteBatch.DrawBorderString(spriteFont, text, position - size / 2f, color, borderColor, borderSize);
        }
    }
}
