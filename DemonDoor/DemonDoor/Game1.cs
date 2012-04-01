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

using XNAVERGE;

namespace DemonDoor {

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : VERGEGame {

        public McGrenderStack mcg;

        public Texture2D im_title, im_door, im_stage, im_skybox, im_demon, im_gameover;
        public Texture2D[] im_civvies;
        public Texture2D[] im_clouds;
        public SpriteFont ft_hud24;

        public static Game1 game;

        private Screen _level;

        private AudioEngine _engine;
        private SoundBank _sb;
        private WaveBank _wb;

        private Cue _bgm;

        private IList<Cue> _activeCues;

        internal void LoadLevel(string level)
        {
            Screen s = null;

            switch (level) {
                case "title": s = new TitleScreen(); break;
                case "level1": s = new Level1Screen(); break;
                case "gameover": s = new GameOverScreen(); break;
            }

            foreach (Cue q in _activeCues) {
                q.Stop(AudioStopOptions.AsAuthored);
            }

            _activeCues.Clear();

            _level = s;
            _level.Load();

            if (_musicEnabled)
            {
                KillMusic();
                StartMusic();
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            game = this;
            _activeCues = new List<Cue>();

            // TODO: Add your initialization logic here         
            im_title = Content.Load<Texture2D>( "art/title" );
            im_door = Content.Load<Texture2D>( "art/door" );
            im_stage = Content.Load<Texture2D>( "art/stage" );
            im_skybox = Content.Load<Texture2D>( "art/skybox" );
            im_demon = Content.Load<Texture2D>( "art/demon" );
            im_gameover = Content.Load<Texture2D>("art/gameover");

            _engine = new AudioEngine("Content/music.xgs");
            _sb = new SoundBank(_engine, "Content/Sound Bank.xsb");
            _wb = new WaveBank(_engine, "Content/Wave Bank.xwb");

            List<Texture2D> tmpLst = new List<Texture2D>();
            for( int i = 1; i < 10; i++ ) {
                tmpLst.Add( Content.Load<Texture2D>("art/cloud_0"+i) );
            }
            im_clouds = tmpLst.ToArray();

            tmpLst = new List<Texture2D>();
            for( int i = 1; i < 8; i++ ) {
                tmpLst.Add( Content.Load<Texture2D>( "art/civilian_0" + i ) );
            }
            im_civvies = tmpLst.ToArray();

            ft_hud24 = Content.Load<SpriteFont>( "HUD24" );

            LoadLevel("title");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spritebatch = new SpriteBatch( GraphicsDevice );

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        int systime;

        ButtonState _yLast = ButtonState.Released;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ButtonState yNow = GamePad.GetState(PlayerIndex.One).Buttons.Y;

            if (yNow == ButtonState.Pressed && _yLast == ButtonState.Released)
            {
                ToggleMusic();
            }

            _yLast = yNow;

            systime = gameTime.TotalGameTime.Milliseconds;

            // TODO: Add your update logic here

            _activeCues = _activeCues.Where(q => !(q.IsStopped)).ToList();

            _level.Update(gameTime);
            base.Update(gameTime);
        }

        bool _musicEnabled = true;

        private void ToggleMusic()
        {
            if (_musicEnabled)
            {
                KillMusic();
                _musicEnabled = false;
            }
            else
            {
                StartMusic();
                _musicEnabled = true;
            }
        }

        private void KillMusic()
        {
            if (_bgm != null)
            {
                _bgm.Stop(AudioStopOptions.AsAuthored);
            }
        }

        private void StartMusic()
        {
            if (_level.BgBgBg != null)
            {
                Cue cue = _sb.GetCue(_level.BgBgBg);
                cue.Play();
                _bgm = cue;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime ) {

            GraphicsDevice.Clear(Color.Black);

            base.Draw( gameTime );

            // do level-specific drawing afterward so we can draw hud
            spritebatch.Begin();
            _level.Draw(spritebatch, gameTime);
            spritebatch.End();
        }

        internal void PlayCue(string name)
        {
            Cue q = _sb.GetCue(name);
            q.Play();
            _activeCues.Add(q);
        }
    }
}
