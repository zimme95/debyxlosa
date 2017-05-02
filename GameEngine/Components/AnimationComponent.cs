﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components
{
    public class AnimationComponent : EntityComponent
    {
        public Texture2D SpriteSheet { get; set; }
        public Point FrameSize { get; set; }
        public Point CurrentFrame { get; set; }
        public Point SheetSize { get; set; }
        public int TimeSinceLastFrame { get; set; }
        public int MillisecondsPerFrame { get; set; }

        public AnimationComponent()
        {
            TimeSinceLastFrame = 0;
            CurrentFrame = new Point(0,0);
        }

        public void setValues(Texture2D spriteSheet, Point frameSize, Point sheetSize, int millisecondsPerFrame)
        {
            SpriteSheet = spriteSheet;
            FrameSize = frameSize;
            SheetSize = sheetSize;
            MillisecondsPerFrame = millisecondsPerFrame;
        }
    }
}