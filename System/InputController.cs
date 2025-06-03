using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrexRunner.Entities;

namespace TrexRunner.System;

public class InputController
{
    private Trex _trex;

    private KeyboardState _previousKeyboardState;

    public InputController(Trex trex)
    {
        _trex = trex;
    }

    public void ProcessControls(GameTime gameTime)
    {
        KeyboardState newKeyboardState = Keyboard.GetState();

        if (!_previousKeyboardState.IsKeyDown(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
        {
            if (_trex.State != TrexState.Jumping)
                _trex.BeginJump();
            else
                _trex.ContinueJump();
        }

        _previousKeyboardState = newKeyboardState;
    }
}