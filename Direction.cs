#region File Description
//-----------------------------------------------------------------------------
// Direction.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Linq;
//using Microsoft.Xna.Framework.Input;
using UnityEngine;
using TeamUtility.IO;
#endregion

namespace InputSequence
{
    /// <summary>
    /// Helper class for working with the 8-way directions stored in a Buttons enum.
    /// </summary>
    static class Direction
    {
        // Helper bit masks for directions defined with the Buttons flags enum.
        public const KeyCode None = 0;
        public const KeyCode Up = KeyCode.UpArrow/* | KeyCode.W*/;
        public const KeyCode Down = KeyCode.DownArrow/* | KeyCode.S*/;
        public const KeyCode Left = KeyCode.LeftArrow/* | KeyCode.A*/;
        public const KeyCode Right = KeyCode.RightArrow/* | KeyCode.D*/;
        public const KeyCode UpLeft = Up | Left;
        public const KeyCode UpRight = Up | Right;
        public const KeyCode DownLeft = Down | Left;
        public const KeyCode DownRight = Down | Right;
        public const KeyCode Any = Up | Down | Left | Right;

        /// <summary>
        /// Gets the current direction from a game pad and keyboard.
        /// </summary>
        public static KeyCode FromInput(PlayerID PlayerIndex = PlayerID.One)
        {
            KeyCode direction = None;

            // Get vertical direction.
            if (TeamUtility.IO.InputManager.GetKeyDown(KeyCode.UpArrow) ||
                //TeamUtility.IO.InputManager.GetButtonDown("Up") ||
                TeamUtility.IO.InputManager.GetAxisRaw("Vertical", PlayerIndex) == 1)
            {
                direction |= Up;
            }
            else if (TeamUtility.IO.InputManager.GetKeyDown(KeyCode.DownArrow) ||
                //TeamUtility.IO.InputManager.GetButtonDown("Down") ||
                TeamUtility.IO.InputManager.GetAxisRaw("Vertical", PlayerIndex) == -1)
            {
                direction |= Down;
            }

            // Comebine with horizontal direction.
            if (TeamUtility.IO.InputManager.GetKeyDown(KeyCode.LeftArrow) ||
                //TeamUtility.IO.InputManager.GetButtonDown("Left") ||
                TeamUtility.IO.InputManager.GetAxisRaw("Horizontal", PlayerIndex) == -1)
            {
                direction |= Left;
            }
            else if (TeamUtility.IO.InputManager.GetKeyDown(KeyCode.RightArrow) ||
                //TeamUtility.IO.InputManager.GetButtonDown("Right") ||
                TeamUtility.IO.InputManager.GetAxisRaw("Horizontal", PlayerIndex) == 1)
            {
                direction |= Right;
            }

            return direction;
        }

        /// <summary>
        /// Gets the direction without non-direction buttons from a set of Buttons flags.
        /// </summary>
        public static KeyCode FromButtons(KeyCode buttons)
        {
            // Extract the direction from a full set of buttons using a bit mask.
            return buttons & Any;
        }
    }
}
