using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MenuScreenProof
{
    class GlobalValues
    {
        //Screen Values
        public const int BackBufferWidth = 1250;
        public const int BackBufferHeight = 750;
        public const float ScreenCenterX = BackBufferWidth / 2.0f;
        public const float ScreenCenterY = BackBufferHeight / 2.0f;
        public const int GroundHeight = 650;//temp

        //enums
        public enum ActiveScreen { MainMenu, GamePlay, Options, Pause,}
        public enum GameState { Loading, Running, Paused, Exit,}
        public enum MenuState { Running, Hidden,}
        public enum ButtonState { Idle, Highlighted,}

       

        


    }
}
