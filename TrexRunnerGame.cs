﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexRunner.Entities;
using TrexRunner.Graphics;
using TrexRunner.System;

namespace TrexRunner;

public class TrexRunnerGame : Game
{
    private const string ASSET_NAME_SPRITESHEET = "TrexSpritesheet";
    private const string ASSET_NAME_SFX_HIT = "hit";
    private const string ASSET_NAME_SFX_SCORE_REACHED = "score-reached";
    private const string ASSET_NAME_SFX_BUTTON_PRESS = "button-press";

    public const int WINDOW_WIDTH = 600;
    public const int WINDOW_HEIGHT = 150;

    public const int TREX_START_POS_X = 1;
    public const int TREX_START_POS_Y = WINDOW_HEIGHT - 16;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SoundEffect _sfxHit;
    private SoundEffect _sfxButtonPress;
    private SoundEffect _sfxScoreReached;

    private Texture2D _spriteSheetTexture;

    private Trex _trex;

    private InputController _inputController;

    public TrexRunnerGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_SFX_HIT);
        _sfxButtonPress = Content.Load<SoundEffect>(ASSET_NAME_SFX_BUTTON_PRESS);
        _sfxScoreReached = Content.Load<SoundEffect>(ASSET_NAME_SFX_SCORE_REACHED);

        _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

        _trex = new Trex(_spriteSheetTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y - Trex.TREX_DEFAULT_SPRITE_HEIGHT), _sfxButtonPress);
        _inputController = new InputController(_trex);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);

        _inputController.ProcessControls(gameTime);

        _trex.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();

        _trex.Draw(_spriteBatch, gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
