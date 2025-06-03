using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TrexRunner.Graphics;

namespace TrexRunner.Entities;

public class Trex : IGameEntity
{
    private const float GRAVITY = 1600f;
    private const float JUMP_START_VELOCITY = -480f;

    private const int TREX_IDLE_BACKGROUND_SPRITE_POS_X = 40;
    private const int TREX_IDLE_BACKGROUND_SPRITE_POS_Y = 0;

    public const int TREX_DEFAULT_SPRITE_POS_X = 848;
    public const int TREX_DEFAULT_SPRITE_POS_Y = 0;
    public const int TREX_DEFAULT_SPRITE_WIDTH = 44;
    public const int TREX_DEFAULT_SPRITE_HEIGHT = 52;

    private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
    private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
    private const float BLINK_ANIMATION_EYE_CLOSE_TIME = 0.5f;

    private Sprite _idleTrexBackgroundSprite;

    private Sprite _idleSprite;
    private Sprite _idleBlinkSprite;

    private SoundEffect _jumpSound;

    private SpriteAnimation _blinkAnimation;

    private Random _random;

    private float _verticalVelocity;

    private float _startPosY;

    public TrexState State { get; private set; }
    public Vector2 Position { get; set; }
    public bool IsAlive { get; private set; }
    public float Speed { get; private set; }
    public int DrawOrder { get; set; }

    public Trex(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound)
    {
        Position = position;
        _idleTrexBackgroundSprite = new Sprite(spriteSheet, TREX_IDLE_BACKGROUND_SPRITE_POS_X, TREX_IDLE_BACKGROUND_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
        State = TrexState.Idle;

        _jumpSound = jumpSound;

        _random = new Random();

        _idleSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
        _idleBlinkSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);

        _blinkAnimation = new SpriteAnimation();
        CreateBlinkAnimation();

        _blinkAnimation.Play();

        _startPosY = position.Y;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (State == TrexState.Idle)
        {
            _idleTrexBackgroundSprite.Draw(spriteBatch, Position);
            _blinkAnimation.Draw(spriteBatch, Position);
        }
        else if (State == TrexState.Jumping || State == TrexState.Falling)
        {
            _idleSprite.Draw(spriteBatch, Position);
        }

    }

    public void Update(GameTime gameTime)
    {
        if (State == TrexState.Idle)
        {
            if (!_blinkAnimation.IsPlaying)
            {
                CreateBlinkAnimation();
                _blinkAnimation.Play();
            }

            _blinkAnimation.Update(gameTime);
        }
        else if (State == TrexState.Jumping || State == TrexState.Falling)
        {
            Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.Y >= _startPosY)
            {
                Position = new Vector2(Position.X, _startPosY);
                _verticalVelocity = 0;
                State = TrexState.Idle;
            }
        }
    }

    private void CreateBlinkAnimation()
    {
        _blinkAnimation.Clear();
        _blinkAnimation.ShouldLoop = false;

        double blinkTimeStamp = BLINK_ANIMATION_RANDOM_MIN + _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN);

        _blinkAnimation.AddFrame(_idleSprite, 0);
        _blinkAnimation.AddFrame(_idleBlinkSprite, (float)blinkTimeStamp);
        _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BLINK_ANIMATION_EYE_CLOSE_TIME);
    }

    public bool BeginJump()
    {
        if (State == TrexState.Jumping || State == TrexState.Falling)
            return false;

        _jumpSound.Play();

        State = TrexState.Jumping;

        _verticalVelocity = JUMP_START_VELOCITY;

        return true;
    }

    public bool ContinueJump()
    {
        return true;
    }
}