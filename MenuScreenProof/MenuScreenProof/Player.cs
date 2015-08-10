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
    class Player
    {
        enum PlayerState
        {
            Idle,
            Walking,
            Jumping,
            Summoning,
        }

        struct ImaginaryFriend
        {
            public Texture2D t2dFriend;
            public Vector2 v2FriendPosition;

            public bool IsSummoned
            {
                get { return isSummoned; }
                set { isSummoned = value; }
            }
            bool isSummoned;

            public bool IsAbilityOneActive
            {
                get { return isAbilityOneActive; }
                set { isAbilityOneActive = value; }
            }
            bool isAbilityOneActive;
        }

        static Vector2 itemPosition = new Vector2(150, GlobalValues.GroundHeight + 25);
        static Vector2 enemyItemPosition = new Vector2(GlobalValues.BackBufferWidth - 100, GlobalValues.GroundHeight + 25);

        ImaginaryFriend imaginaryFriend = new ImaginaryFriend();
        Items item = new Items(itemPosition, Items.ItemType.AllowFriend);
        Items enemyItem = new Items(enemyItemPosition, Items.ItemType.EnemySpawn);
        Enemy enemy = new Enemy();

        #region Player Variables
        Texture2D t2dPlayerSprite;
        Texture2D t2dYouDied;
        SoundEffect seJumpUp;
        SoundEffect seFallDown;
        SoundEffect seBeAttacked;
        SoundEffect seLightOn;
        SoundEffect seDeath;

        public Vector2 v2PlayerPosition = new Vector2(GlobalValues.ScreenCenterX, GlobalValues.GroundHeight);
        Vector2 v2NoticePosition = new Vector2(GlobalValues.ScreenCenterX - 130, GlobalValues.ScreenCenterY - 130);

        public float GetPlayerPositionX
        {
            get { return v2PlayerPosition.X; }
        }

        bool isRight = false;
        public bool IsPlayerRight
        {
            get { return isRight; }
        }

        Point frameSize = new Point(50, 50);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(6, 2); //x frames on line, y how many lines of frames

        const int iSprintingSpeed = 1;
        const int iWalkingSpeed = 2;
        const int iSummonDisplacement = 60;
        const int iMaxFriendDistance = 80;
        int lightTimer = 200;
        int iJumpVelocity = 0;
        int iHealth = 1000;
        int iHalfHealth = 500;
        int iQuarterHealth = 250;
        float fWithinDistance = 350;
        bool isJumping = false;
        bool canSummon = false;
        bool playerAlive = true;

        SpriteFont healthFont;

        PlayerState playerState = PlayerState.Idle;
        //-----------------------------------------
        #endregion

        #region Health Bar Variables
        Texture2D t2dHealthBar;
        static int HealthHeight = 20;
        static int HealthWidth = 20;
        Vector2 v2HealthPosition = new Vector2(HealthWidth, HealthHeight);

        #endregion

        #region Player Methods
        //Constructing!
        public Player()
        {
        }

        //Loading!
        public void LoadContent(ContentManager contentManager)
        {
            t2dPlayerSprite = contentManager.Load<Texture2D>("Sprites/TempTest");
            imaginaryFriend.t2dFriend = contentManager.Load<Texture2D>("Sprites/TempLamp");
            t2dHealthBar = contentManager.Load<Texture2D>("Sprites/TempHealth");
            t2dYouDied = contentManager.Load<Texture2D>("Sprites/youDied");
            healthFont = contentManager.Load<SpriteFont>("Fonts/healthFont");
            item.LoadContent(contentManager);
            enemyItem.LoadContent(contentManager);
            enemy.LoadContent(contentManager);

            imaginaryFriend.IsSummoned = false;

            

            seJumpUp = contentManager.Load<SoundEffect>("Sounds/Jump_off");
            seFallDown = contentManager.Load<SoundEffect>("Sounds/Jump_Land");
            seBeAttacked = contentManager.Load<SoundEffect>("Sounds/TempAttacked");
            seLightOn = contentManager.Load<SoundEffect>("Sounds/TempLightOn");
            seDeath = contentManager.Load<SoundEffect>("Sounds/TempDeath");

        }

        //Updating!
        public void Update()
        {

            if (playerAlive)
            {
                DoInput();

                if (item.ShowIcon)
                {
                    item.IconFloat();
                }
                else if (enemyItem.ShowIcon)
                {
                    enemyItem.IconFloat();
                }

                if (enemy.IsEnemySummoned)
                {
                    EnemySeekPlayer();
                }

                playerState = PlayerState.Idle;
            }
        }

        //Drawing!
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            item.Draw(spriteBatch);
            enemyItem.Draw(spriteBatch);
            enemy.Draw(spriteBatch);

            spriteBatch.Draw(t2dHealthBar, v2HealthPosition, Color.White);

            #region Health Bar
            Vector2 hudLocation = new Vector2(v2HealthPosition.X, v2HealthPosition.Y);
            Vector2 center = new Vector2(v2HealthPosition.X + t2dHealthBar.Width / 2.0f,
                                         v2HealthPosition.Y + t2dHealthBar.Height / 2.0f);

            string healthString = "HP: " + iHealth;
            Color healthColor;
            if (iHealth > iHalfHealth)
            {
                healthColor = Color.Black;
            }
            else if (iHealth <= iHalfHealth && iHealth >= iQuarterHealth)
            {
                healthColor = Color.Yellow;
            }
            else
            {
                healthColor = Color.Red;
            }
            spriteBatch.DrawString(healthFont, healthString, hudLocation, healthColor);

            #endregion


            if (playerAlive)
            {
                spriteBatch.Draw(t2dPlayerSprite, v2PlayerPosition, new Rectangle(
                                 frameSize.X * currentFrame.X,
                                 frameSize.Y * currentFrame.Y,
                                 frameSize.X, frameSize.Y),
                                 Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                if (imaginaryFriend.IsSummoned && imaginaryFriend.IsAbilityOneActive)
                {
                    spriteBatch.Draw(imaginaryFriend.t2dFriend, imaginaryFriend.v2FriendPosition, Color.White);
                }
                else if (imaginaryFriend.IsSummoned)
                {
                    spriteBatch.Draw(imaginaryFriend.t2dFriend, imaginaryFriend.v2FriendPosition, Color.LightBlue);
                }
            }
            else
            {
                spriteBatch.Draw(t2dYouDied, v2NoticePosition, Color.Red);
            }
        }

        //Handle Input!
        public void DoInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool isKeyPressed = false;

            int MaxX = (GlobalValues.BackBufferWidth - frameSize.X) - 25;
            int MaxY = GlobalValues.BackBufferHeight - frameSize.Y;
            int MinX = 0 + 25;

            int MinY = 0;

            if (playerState == PlayerState.Idle)
            {
                currentFrame.Y = 0;
                currentFrame.X++;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
            }

            #region Walking
            //Left Walk
            if (keyboardState.IsKeyDown(Keys.A)
                || gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                //frameSize = new Point(50, 50);
                currentFrame.Y = 1;
                currentFrame.X++;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
                v2PlayerPosition.X += -iWalkingSpeed;
                playerState = PlayerState.Walking;
                isRight = false;
            }

            //Left Sprint
            if ((keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.J))
                || ((gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)
                && gamepadState.IsButtonDown(Buttons.X))))
            {
                //frameSize = new Point(50, 50);
                currentFrame.Y = 1;
                currentFrame.X++;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
                v2PlayerPosition.X += -iWalkingSpeed * iSprintingSpeed;
                playerState = PlayerState.Walking;
                isRight = false;
            }

            //Right Walk
            if (keyboardState.IsKeyDown(Keys.D)
                || gamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                //frameSize = new Point(50, 50);
                currentFrame.Y = 1;
                currentFrame.X++;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
                v2PlayerPosition.X += iWalkingSpeed;
                playerState = PlayerState.Walking;
                isRight = true;

            }

            //Right Sprint
            if ((keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.J))
                || (gamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
                && gamepadState.IsButtonDown(Buttons.X)))
            {
                //frameSize = new Point(50, 50);
                currentFrame.Y = 1;
                currentFrame.X++;
                if (currentFrame.X >= 5)
                    currentFrame.X = 0;
                v2PlayerPosition.X += iWalkingSpeed * iSprintingSpeed;
                playerState = PlayerState.Walking;
                isRight = true;

            }

            #endregion

            #region Jumping
            //-- Vertical --------------------
            if (isJumping)
            {
                //Parabolic maaaaaagic. <3
                iJumpVelocity += 1;
                v2PlayerPosition.Y += iJumpVelocity;

                //Pseudo collision.
                if (v2PlayerPosition.Y >= GlobalValues.GroundHeight)
                {
                    seFallDown.Play();
                    v2PlayerPosition.Y = GlobalValues.GroundHeight;
                    isJumping = false;
                    playerState = PlayerState.Idle;
                }
            }
            else
            {
                //Get the jump prepared.
                if (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.A))
                {
                    isJumping = true;
                    playerState = PlayerState.Jumping;
                    seJumpUp.Play();
                    iJumpVelocity = -14;
                }
            }
            #endregion

            #region Interacting
            if (!item.IsPickedUp)
            {

                //If player is within certain distance, show icon for item.
                if ((v2PlayerPosition.X >= item.Position.X - 100) && (v2PlayerPosition.X <= item.Position.X + 100 + item.ItemTexture.Width))
                {
                    item.ShowIcon = true;
                }
                else
                {
                    item.ShowIcon = false;
                }

                //If player is colliding with item, she can pick it up.
                if ((item.Position.X >= v2PlayerPosition.X) && (item.Position.X <= v2PlayerPosition.X + frameSize.X))
                {

                    if (keyboardState.IsKeyDown(Keys.I) || gamepadState.IsButtonDown(Buttons.B) && playerState == PlayerState.Idle)
                    {
                        item.IsPickedUp = true;
                        item.ShowIcon = false;
                        canSummon = true;
                    }
                }

            }

            //Item that spawns enemy.
            if (!enemyItem.IsPickedUp)
            {
                if ((v2PlayerPosition.X >= enemyItem.Position.X - 100) && (v2PlayerPosition.X <= enemyItem.Position.X + 100 + enemyItem.ItemTexture.Width))
                {
                    enemyItem.ShowIcon = true;
                }
                else
                {
                    enemyItem.ShowIcon = false;
                }

                if ((enemyItem.Position.X >= v2PlayerPosition.X) && (enemyItem.Position.X <= v2PlayerPosition.X + frameSize.X))
                {
                    if (keyboardState.IsKeyDown(Keys.I) || gamepadState.IsButtonDown(Buttons.B) && playerState == PlayerState.Idle)
                    {
                        enemyItem.IsPickedUp = true;
                        enemyItem.ShowIcon = false;
                        enemy.IsEnemySummoned = true;
                        enemy.SpawnEnemy();
                    }
                }


            }

            #endregion

            #region Summoning / Friends
            //-- Summoning! ------------------
            if (imaginaryFriend.IsSummoned)
            {
                FriendFollow(keyboardState, gamepadState);
            }

            //if item is picked up, allow this-----------
            if (!imaginaryFriend.IsSummoned && canSummon)
            {
                if (keyboardState.IsKeyDown(Keys.K) || gamepadState.IsButtonDown(Buttons.LeftShoulder))
                {
                    //Summon
                    playerState = PlayerState.Summoning;
                    imaginaryFriend.IsSummoned = true;

                    if (isRight)
                        imaginaryFriend.v2FriendPosition = new Vector2((v2PlayerPosition.X - iSummonDisplacement), (GlobalValues.GroundHeight - 50.0f));
                    else
                        imaginaryFriend.v2FriendPosition = new Vector2((v2PlayerPosition.X + iSummonDisplacement), (GlobalValues.GroundHeight - 50.0f));

                    isKeyPressed = true;
                }
            }

            //Friend is now summoned, what can they do?
            if (imaginaryFriend.IsSummoned && !isKeyPressed)
            {
                if (keyboardState.IsKeyDown(Keys.M) || gamepadState.IsButtonDown(Buttons.RightShoulder))
                {
                    //Desummon
                    playerState = PlayerState.Summoning;
                    imaginaryFriend.IsSummoned = false;
                }

                if (keyboardState.IsKeyDown(Keys.I) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    //Execute ability.
                    FriendAbilityOn();
                }

                if (keyboardState.IsKeyDown(Keys.O) || gamepadState.IsButtonDown(Buttons.DPadDown))
                {
                    //Stop ability.
                    FriendAbilityOff();
                }

                //If ability is active, time it!
                if (imaginaryFriend.IsAbilityOneActive)
                {
                    if (lightTimer <= 0)
                    {
                        FriendAbilityOff();
                    }
                    else
                    {
                        lightTimer--;
                    }
                }
            }
            //--------------------------------------

            #endregion

            #region Screen Bound
            if (v2PlayerPosition.X > MaxX)
            {
                v2PlayerPosition.X = MaxX;
            }

            else if (v2PlayerPosition.X < MinX)
            {
                v2PlayerPosition.X = MinX;
            }

            if (enemy.EnemyPositionX > MaxX)
            {
                enemy.EnemyPositionX = MaxX;
            }
            else if (enemy.EnemyPositionX < MinX)
            {
                enemy.EnemyPositionX = MinX;
            }

            if (v2PlayerPosition.Y > GlobalValues.GroundHeight)
            {
                v2PlayerPosition.Y = MaxY;
            }

            else if (v2PlayerPosition.Y < MinY)
            {
                v2PlayerPosition.Y = MinY;
            }
            #endregion
        }

        //Follow Mallory!
        public void FriendFollow(KeyboardState keyboardState, GamePadState gamepadState)
        {

            if (imaginaryFriend.v2FriendPosition.X >= (v2PlayerPosition.X + iMaxFriendDistance))
            {
                do
                {
                    imaginaryFriend.v2FriendPosition.X -= 1;
                }
                while (imaginaryFriend.v2FriendPosition.X >= (v2PlayerPosition.X + iMaxFriendDistance));
            }
            else if (imaginaryFriend.v2FriendPosition.X <= (v2PlayerPosition.X - iMaxFriendDistance))
            {
                do
                {
                    imaginaryFriend.v2FriendPosition.X += 1;
                }
                while (imaginaryFriend.v2FriendPosition.X <= (v2PlayerPosition.X - iMaxFriendDistance));
            }


        }

        //Execute Ability!
        public void FriendAbilityOn()
        {
            //Dependant on friend. Switch statement, IDs. Add later.

            //Generic Friend: Lamp
            //----------------------
            //Basic Ability One:
            //*Turn Light on and off
            //Basic Ability Two:
            //*Act as platform
            //----------------------
            if (!imaginaryFriend.IsAbilityOneActive)
            {
                seLightOn.Play();
                lightTimer = 100;
            }
            imaginaryFriend.IsAbilityOneActive = true;
        }

        public void FriendAbilityOff()
        {
            if (imaginaryFriend.IsAbilityOneActive)
            {
                seLightOn.Play();
            }
            imaginaryFriend.IsAbilityOneActive = false;
        }

        public void EnemySeekPlayer()
        {
            //If the lamp isn't on, seek!
            if (!imaginaryFriend.IsAbilityOneActive)
            {
                if ((enemy.EnemyPositionX <= (v2PlayerPosition.X + fWithinDistance + 50))
                    && (enemy.EnemyPositionX >= (v2PlayerPosition.X - fWithinDistance)))
                {
                    enemy.IsEnemyNear = true;

                    if (enemy.EnemyPositionX >= v2PlayerPosition.X + 50)
                    {
                        enemy.EnemyPositionX -= enemy.SeekSpeed;
                        EnemyAttackPlayer();
                    }

                    if (enemy.EnemyPositionX <= v2PlayerPosition.X)
                    {
                        enemy.EnemyPositionX += enemy.SeekSpeed;
                        EnemyAttackPlayer();
                    }
                }
                else
                {
                    enemy.IsEnemyNear = false;
                }

            }
            else //flee!
            {
                if ((enemy.EnemyPositionX <= (imaginaryFriend.v2FriendPosition.X + fWithinDistance + 50))
                    && (enemy.EnemyPositionX >= (imaginaryFriend.v2FriendPosition.X - fWithinDistance)))
                {
                    if (enemy.EnemyPositionX >= imaginaryFriend.v2FriendPosition.X + 50)
                    {
                        enemy.EnemyPositionX++;
                    }

                    if (enemy.EnemyPositionX <= imaginaryFriend.v2FriendPosition.X)
                    {
                        enemy.EnemyPositionX--;
                    }
                }
                else
                {
                    enemy.IsEnemyNear = false;
                }
            }
        }

        public void EnemyAttackPlayer()
        {
            if (!imaginaryFriend.IsAbilityOneActive)
            {
                if ((enemy.EnemyPositionX >= v2PlayerPosition.X)
                 && (enemy.EnemyPositionX <= v2PlayerPosition.X + 50))
                {
                    seBeAttacked.Play();

                    iHealth -= enemy.GetAttackPower;

                    if (iHealth <= 0)
                    {
                        PlayerDeath();
                    }
                }
            }
        }

        public void PlayerDeath()
        {
            playerAlive = false;
            seDeath.Play();

            MediaPlayer.Stop();

        }



        #endregion


    }
}
