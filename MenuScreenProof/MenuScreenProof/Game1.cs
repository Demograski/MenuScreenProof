
#region Notes on current progress -> Read Me First!
///Well, I've got it working basically. Some parts of the menu give me a bit of trouble.
///Mostly the pause menu structure... I'll figure it out.
///
///For debugging purposes, I have a legend og what lines cause trouble when included, but need to 
///be added.
///Legend: 
//<!--> = Find me for demonstration, this B**ch is causing a logical error; Should be an easy solution, just can't seem to find it.
//


#endregion

#region Using Statements
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
using System.Threading;
using System.IO;
#endregion

namespace MenuScreenProof
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Definitions and stuff


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont tempFont;

        #region Screen stuff

        GlobalValues.ActiveScreen currentScreen;
       // GlobalValues.ButtonState newGbuttonState;
       // GlobalValues.ButtonState loadgbuttonState;
       // GlobalValues.ButtonState optionsbuttonState;
       // GlobalValues.ButtonState exitGbuttonState;
       // GlobalValues.GameState gameState;
       // GlobalValues.MenuState menuState;

       // KeyboardState kBoardState = new KeyboardState();
        
        //MainMenu stuff
        Texture2D StartScreen, btnExit, btnLoad, btnNewGame, btnOptions;
        Vector2 StartScreenBGPos, btnNewGPos, btnLoadPos, btnOptionsPos, btnExitPos;
        Vector2 btnOrigin;// btnSize;
        Color newBColor, loadBColor, optionsBColor, exitBColor;

        //temp index for the thing
        int MMindex;
        int OPindex;
        int Pindex;
        //

        //gameplay screen stuff, all temporary.
        Texture2D GameScreen;
        //

        //Options Screen
        Texture2D Options;
        Color option1Color, option2color, option3color;
        Vector2 option1Pos, option2Pos, option3Pos;
        //GlobalValues.ButtonState option1State, option2State, option3State;
        //

        //Pause Screen
        Texture2D Pause;
        Color PauseSelect1, PauseSelect2;
        Vector2 PauseSelectPos1, PauseSelectPos2;
        //
        #endregion

        #region Player Stuff

        //Player Stuff
        Player player = new Player();
        //------------
        //Tiles
        Tile[,] tiles;
        string path;
        //

        #endregion

        #endregion

        #region Construct!

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GlobalValues.BackBufferWidth;
            graphics.PreferredBackBufferHeight = GlobalValues.BackBufferHeight;
            Content.RootDirectory = "Content";

            //Debugging
            currentScreen = GlobalValues.ActiveScreen.MainMenu;
            //gameState = GlobalValues.GameState.Paused;
            //menuState = GlobalValues.MenuState.Running;

            //btnSize = new Vector2(76, 26);
            

            newBColor = Color.White;
            loadBColor = Color.White;
            optionsBColor = Color.White;
            exitBColor = Color.White;
            //
            option1Color = Color.White;
            option2color = Color.White;
            option3color = Color.White;

            MMindex = 0;
            OPindex = 0;
            Pindex = 0;

            PauseSelect1 = Color.White;
            PauseSelect2 = Color.White;

            path = "C:/Users/Sean Kennedy/Desktop/MenuScreenProof/MenuScreenProof/MenuScreenProofContent/Test.txt";
            //BGM

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Content.Load<Song>("Sounds/BGM"));

            
            //------------
        }
        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
        #region Initialize!

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            if (currentScreen == GlobalValues.ActiveScreen.MainMenu || currentScreen == GlobalValues.ActiveScreen.Options
                || currentScreen == GlobalValues.ActiveScreen.Pause)
            {
                //mouse will be visible in all screens but the gameplay screen.
                IsMouseVisible = true;
                //if the current screen is the main menu screen
               
                   
                //initialize that shit
                StartScreenBGPos = Vector2.Zero;
                btnNewGPos = new Vector2(GlobalValues.ScreenCenterX - 40, GlobalValues.ScreenCenterY - 60);
                btnLoadPos = new Vector2(GlobalValues.ScreenCenterX - 40, GlobalValues.ScreenCenterY);
                btnOptionsPos = new Vector2(GlobalValues.ScreenCenterX - 40, GlobalValues.ScreenCenterY + 60);
                btnExitPos = new Vector2(GlobalValues.ScreenCenterX - 40, GlobalValues.ScreenCenterY + 120);

                //newGbuttonState = GlobalValues.ButtonState.Highlighted;
                //loadgbuttonState = GlobalValues.ButtonState.Idle;
                //optionsbuttonState = GlobalValues.ButtonState.Idle;
                //exitGbuttonState = GlobalValues.ButtonState.Idle;

                option1Pos = btnNewGPos;
                option2Pos = btnLoadPos;
                option3Pos = btnOptionsPos;

                PauseSelectPos1 = btnNewGPos;
                PauseSelectPos2 = btnLoadPos;
                

                
            }
            else
            {
                IsMouseVisible = false;
            }

            base.Initialize();
        }

        #endregion

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        #region Load!

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tempFont = Content.Load<SpriteFont>("Fonts/TempFont");
          
            //Start Screen stuff.
            StartScreen = Content.Load<Texture2D>("Screens/StartScreenBG");
            
        
            //gameplay screen
            GameScreen = Content.Load<Texture2D>("Screens/GameBG");
            player.LoadContent(Content);
            LoadTiles(path);

            //Options Screen
            Options = Content.Load<Texture2D>("Screens/OptionsBG");

            //Pause Screen
            Pause = Content.Load<Texture2D>("Screens/StartScreenBG");

            
        }

        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
        #region Unload!

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        #region Update!

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            

            #region Main Menu Update Stuff

            if (currentScreen == GlobalValues.ActiveScreen.MainMenu)
            {
                
                //check mouse position stuff
                //check for changes in the state stuff
                if (MMindex > 3)
                    MMindex = 0;
                if (MMindex < 0)
                    MMindex = 3;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    MMindex++;
                    Thread.Sleep(100);
                }

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    MMindex--;
                    Thread.Sleep(100);
                }

                //new GameButton
                if (MMindex == 0) //maybe try an index?... no...that would do the same thing.
                {
                    newBColor = Color.Red;
                    //newGbuttonState = GlobalValues.ButtonState.Highlighted;

                    loadBColor = Color.White;
                    //loadgbuttonState = GlobalValues.ButtonState.Idle;

                    exitBColor = Color.White;
                    //exitGbuttonState = GlobalValues.ButtonState.Idle;

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                        currentScreen = GlobalValues.ActiveScreen.GamePlay;

                   

                }
                else if (MMindex == 1)
                {
                    newBColor = Color.White;
                    //newGbuttonState = GlobalValues.ButtonState.Idle;

                    loadBColor = Color.Red;
                    //loadgbuttonState = GlobalValues.ButtonState.Highlighted;

                    optionsBColor = Color.White;
                    //optionsbuttonState = GlobalValues.ButtonState.Idle;
    
                }
                else if (MMindex == 2)
                {
                    loadBColor = Color.White;
                    //loadgbuttonState = GlobalValues.ButtonState.Idle;

                    optionsBColor = Color.Red;
                    //optionsbuttonState = GlobalValues.ButtonState.Highlighted;

                    exitBColor = Color.White;
                    //exitGbuttonState = GlobalValues.ButtonState.Idle;

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                        currentScreen = GlobalValues.ActiveScreen.Options;
                    
                }
                else if (MMindex == 3)
                {
                    optionsBColor = Color.White;
                    //optionsbuttonState = GlobalValues.ButtonState.Idle;

                    exitBColor = Color.Red;
                    //exitGbuttonState = GlobalValues.ButtonState.Highlighted;

                    newBColor = Color.White;
                    //newGbuttonState = GlobalValues.ButtonState.Idle;

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                        this.Exit();
                }
            }//end if MainMenu

            #endregion

            #region GamePlay Screen

            else if (currentScreen == GlobalValues.ActiveScreen.GamePlay)
            {

                //This is supposed to pause the game. Stop all action and bring up the pause menu.
                //returns to the main menu for now.
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                    currentScreen = GlobalValues.ActiveScreen.Pause;

                //Then it should update the player and the enemies and such.
                player.Update();
                    
            }

            #endregion

            #region Options Screen

            else if (currentScreen == GlobalValues.ActiveScreen.Options)
            {
                if (OPindex > 3)
                    OPindex = 0;
                if (OPindex < 0)
                    OPindex = 3;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                    currentScreen = GlobalValues.ActiveScreen.MainMenu;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    OPindex++;
                    Thread.Sleep(100);
                }

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    OPindex--;
                    Thread.Sleep(100);
                }

                if (OPindex == 0)
                {
                    option1Color = Color.Red;
                    option2color = Color.White;
                    option3color = Color.White;
                    //these will have a point later, but for now, they're just here.
                }
                else if (OPindex == 1)
                {
                    option1Color = Color.White;
                    option2color = Color.Red;
                    option3color = Color.White;
                }
                else if (OPindex == 2)
                {
                    option1Color = Color.White;
                    option2color = Color.White;
                    option3color = Color.Red;
                }
            }

            #endregion

            #region Pause

            else if (currentScreen == GlobalValues.ActiveScreen.Pause)
            {
               
                if (Pindex > 1)
                    Pindex = 1;
                if (Pindex < 0)
                    Pindex = 0;

               // if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                 //currentScreen = GlobalValues.ActiveScreen.MainMenu;//<!-->

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    Pindex++;
                    Thread.Sleep(100);
                }

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    Pindex--;
                    Thread.Sleep(100);
                }

                if (Pindex == 0)
                {
                    PauseSelect1 = Color.Red;
                    PauseSelect2 = Color.White;

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                        currentScreen = GlobalValues.ActiveScreen.MainMenu; //<!-->
                }
                else if (Pindex == 1)
                {
                    PauseSelect1 = Color.White;
                    PauseSelect2 = Color.Red;

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                        currentScreen = GlobalValues.ActiveScreen.MainMenu;//<!-->
                }
            }

            #endregion



            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        #region Draw!

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            btnOrigin = new Vector2(0, tempFont.LineSpacing / 2);


            spriteBatch.Begin();

            #region Start Screen
            if (currentScreen == GlobalValues.ActiveScreen.MainMenu)
            {
                //consider using the long one... the one with layer depth and origin.
                //this is a proof for now.
                spriteBatch.Draw(StartScreen, StartScreenBGPos, Color.White);
                spriteBatch.DrawString(tempFont, "New Game", btnNewGPos, newBColor);
                spriteBatch.DrawString(tempFont, "Load Game", btnLoadPos, loadBColor);
                spriteBatch.DrawString(tempFont, "Options", btnOptionsPos, optionsBColor);
                spriteBatch.DrawString(tempFont, "Exit Game", btnExitPos, exitBColor);

            }
            #endregion

            #region Gameplay Screen

            else if (currentScreen == GlobalValues.ActiveScreen.GamePlay)
            {
                
                spriteBatch.Draw(GameScreen, Vector2.Zero, Color.White);
                DrawTiles(spriteBatch);
                player.Draw(spriteBatch, gameTime);
            }

            #endregion

            #region Options

            if (currentScreen == GlobalValues.ActiveScreen.Options)
            {
                spriteBatch.Draw(Options, StartScreenBGPos, Color.White);
                spriteBatch.DrawString(tempFont, "Option1", option1Pos, option1Color);
                spriteBatch.DrawString(tempFont, "Option2", option2Pos, option2color);
                spriteBatch.DrawString(tempFont, "Option3", option3Pos, option3color);
            }

            #endregion

            #region Pause menu

            if (currentScreen == GlobalValues.ActiveScreen.Pause)
            {
                spriteBatch.Draw(Pause, StartScreenBGPos, Color.Black);
                spriteBatch.DrawString(tempFont, "Resume", PauseSelectPos1, PauseSelect1);
                spriteBatch.DrawString(tempFont, "Return to Main Menu", PauseSelectPos2, PauseSelect2);
            }

            #endregion


            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        #region Tile Definitions and Draw Code

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        private void LoadTiles(string path)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is a different length from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            //allocate the grid
            tiles = new Tile[width, lines.Count];

            //loop over every tile position,
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                // Blank space
                case '.':
                    return new Tile(null, TileCollision.Passable);


                // Floating platform
                case '~':
                    return LoadTile("Platform", TileCollision.Platform);


                // wall block
                case '|':
                    return LoadTile("Block2", TileCollision.Impassable);

                // Player 1 start point
                case 'A':
                    return LoadStartTile(x, y);

                // Impassable block - floor
                case ':':
                    return LoadTile("Block1", TileCollision.Impassable);

                //Loads an item above a certain tile
                // case"/":
                //   return

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + name), collision);
        }

        private Tile LoadStartTile(int x, int y)
        {
            player.v2PlayerPosition = new Vector2(x, y);

            return new Tile(null, TileCollision.Passable);
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    //if there is a visible tile...
                    Texture2D tile = tiles[x, y].Texture;
                    if (tile != null)
                    {
                        //draw it in screen space
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spriteBatch.Draw(tile, position, Color.White);
                    }
                }
        }

        #endregion




        #region Tile Collision

        public void DoCollision()
        {

        }

        #endregion
    }
}
