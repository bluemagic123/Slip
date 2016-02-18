using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slip
{
    // This is the general class for turrets. All other turrets are descendants of this class. 
    public class Turret : Enemy
    {
		// All turrets, no matter what kind, have some kind of texture, a position, and a "dead" bool (they can be active or nonactive)
        public static Texture2D texture;
        public bool dead;

        public Turret(Vector2 pos) : base(pos) { }

        public override void Update(Room room)
        {
            
        }

        public override void Draw(GameScreen screen, Main main)
        {
            main.spriteBatch.Draw(texture, screen.DrawPos(main, position), null, Color.White, texture.Center());
        }
    }
}