using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace MenuScreenProof
{
    class Enemy
    {

        #region Enemy Variables

        Texture2D t2dEnemy;

        public Texture2D GetEnemeyTexture
        {
            get { return t2dEnemy; }
        }
        Vector2 v2Position;

        public float EnemyPositionX
        {
            get { return v2Position.X; }
            set { v2Position.X = value; }

        }

        int iAttackPower = 25;
        public int GetAttackPower
        {
            get { return iAttackPower; }
        }

        int iSeekSpeed = 3;
        public int SeekSpeed
        {
            get { return iSeekSpeed; }
        }

        bool isEnemyNear = false;
        public bool IsEnemyNear
        {
            get { return isEnemyNear; }
            set { isEnemyNear = value; }
        }

        bool isEnemySummoned = false;
        public bool IsEnemySummoned
        {
            get { return isEnemySummoned; }
            set { isEnemySummoned = value; }
        }

        #endregion

        #region Enemy Methods

        public Enemy()
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
            t2dEnemy = contentManager.Load<Texture2D>("Sprites/TempEnemy");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isEnemySummoned && isEnemyNear)
            {
                spriteBatch.Draw(t2dEnemy, v2Position, Color.Red);
            }
            else if (isEnemySummoned)
            {
                spriteBatch.Draw(t2dEnemy, v2Position, Color.White);
            }
        }

        public void SpawnEnemy()
        {
            v2Position = new Vector2((GlobalValues.ScreenCenterX), GlobalValues.GroundHeight);
        }

        #endregion
    }
}
