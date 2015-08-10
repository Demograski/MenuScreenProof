using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MenuScreenProof
{
    class Items
    {

        public enum ItemType
        {
            AllowFriend,
            EnemySpawn
        }

        #region Item Variables

        static Texture2D t2dItem;
        Texture2D t2dIcon;

        ItemType itemType;

        Vector2 v2Position;
        Vector2 v2IconPosition;

        public bool IsPickedUp
        {
            get { return isPickedUp; }
            set { isPickedUp = value; }
        }
        bool isPickedUp = false;

        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }
        bool showIcon = false;

        public Texture2D ItemTexture
        {
            get { return t2dItem; }
        }

        public Vector2 Position
        {
            get { return v2Position; }
        }

        #endregion


        #region Item Methods

        public Items(Vector2 Position, ItemType theItemType)
        {
            v2Position = Position;
            v2IconPosition = new Vector2(v2Position.X, v2Position.Y - 50);
            itemType = theItemType;
        }

        public void LoadContent(ContentManager contentManager)
        {
            t2dItem = contentManager.Load<Texture2D>("Sprites/TempItem");
            t2dIcon = contentManager.Load<Texture2D>("Sprites/TempIcon");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isPickedUp)
            {
                spriteBatch.Draw(t2dItem, v2Position, Color.White);
            }

            if (showIcon)
            {
                spriteBatch.Draw(t2dIcon, v2IconPosition, Color.White);
            }
        }

        public void IconFloat()
        {
            v2IconPosition.Y++;
            if (v2IconPosition.Y >= v2Position.Y - 30)
            {
                v2IconPosition.Y = GlobalValues.GroundHeight - 50;
            }
        }

        #endregion
    }
}
