#region File Description
//-----------------------------------------------------------------------------
// InputManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
using UnityEngine;
using TeamUtility.IO;
#endregion

namespace InputSequence
{
    class InputManager
    {
        public PlayerID PlayerIndex { get; private set; }

        public KeyCode direction { get; private set; }

        /// <summary>
        /// The last "real time" that new input was received. Slightly late button
        /// presses will not update this time; they are merged with previous input.
        /// </summary>
        public float LastInputTime { get; private set; }

        /// <summary>
        /// The current sequence of pressed buttons.
        /// </summary>
        public List<KeyCode> Buffer;

        
        /// <summary>
        /// This is how long to wait for input before all input data is expired.
        /// This prevents the player from performing half of a move, waiting, then
        /// performing the rest of the move after they forgot about the first half.
        /// </summary>
        public readonly float BufferTimeOut = 0.8f;

        
        /// <summary>
        /// This is the size of the "merge window" for combining button presses that
        /// occur at almsot the same time.
        /// If it is too small, players will find it difficult to perform moves which
        /// require pressing several buttons simultaneously.
        /// If it is too large, players will find it difficult to perform moves which
        /// require pressing several buttons in sequence.
        /// </summary>
        public readonly float MergeInputTime = 0.15f;


        /// <summary>
        /// Provides the map of non-direction game pad buttons to keyboard keys.
        /// </summary>
        internal static readonly List<KeyCode> NonDirectionButtons =
            new List<KeyCode>
            {
                KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
                KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
                KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
                KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
            };
        //internal static readonly Dictionary<Buttons, Keys> NonDirectionButtons =
        //    new Dictionary<Buttons, Keys>
        //    {
        //        { Buttons.A, Keys.A },
        //        { Buttons.B, Keys.B },
        //        { Buttons.X, Keys.X },
        //        { Buttons.Y, Keys.Y },
        //        // Other available non-direction buttons:
        //        // Start, Back, LeftShoulder, LeftTrigger, LeftStick,
        //        // RightShoulder, RightTrigger, and RightStick.
        //    };


        public InputManager(PlayerID playerIndex, int bufferSize)
        {
            PlayerIndex = playerIndex;
            Buffer = new List<KeyCode>(bufferSize);
        }

        /// <summary>
        /// Gets the latest input and uses it to update the input history buffer.
        /// </summary>
        public void Update()
        {
            // Get latest input state.
            KeyCode lastDirection = direction;
#if UNITY_STANDALONE_WIN
            if (PlayerIndex == PlayerID.One)
            {
                //KeyboardState = Keyboard.GetState();
                //KeyboardState = Keyboard.GetState(PlayerIndex);
            }
#endif            
            // Expire old input.
            float time = Time.time;
            float timeSinceLast = time - LastInputTime;
            if (timeSinceLast > BufferTimeOut)
            {
                Buffer.Clear();
            }

            // Get all of the non-direction buttons pressed.
            KeyCode buttons = 0;
            foreach (var key in NonDirectionButtons)
            {
                // Check the game pad and keyboard for presses.
                if (TeamUtility.IO.InputManager.GetKeyDown(key))
                {
                    // Use a bitwise-or to accumulate button presses.
                    buttons |= key;
                }
            }

            // It is very hard to press two buttons on exactly the same frame.
            // If they are close enough, consider them pressed at the same time.
            bool mergeInput = (Buffer.Count > 0 && timeSinceLast < MergeInputTime);

            // If there is a new direction,
            direction = Direction.FromInput(PlayerIndex);
            if (lastDirection != direction)
            {
                // combine the direction with the buttons.
                buttons |= direction;

                // Don't merge two different directions. This avoids having impossible
                // directions such as Left+Up+Right. This also has the side effect that
                // the direction needs to be pressed at the same time or slightly before
                // the buttons for merging to work.
                mergeInput = false;
            }

            // If there was any new input on this update, add it to the buffer.
            if (buttons != 0)
            {
                if (mergeInput)
                {
                    // Use a bitwise-or to merge with the previous input.
                    // LastInputTime isn't updated to prevent extending the merge window.
                    Buffer[Buffer.Count - 1] = Buffer[Buffer.Count - 1] | buttons;                    
                }
                else
                {
                    // Append this input to the buffer, expiring old input if necessary.
                    if (Buffer.Count == Buffer.Capacity)
                    {
                        Buffer.RemoveAt(0);
                    }
                    Buffer.Add(buttons);

                    // Record this the time of this input to begin the merge window.
                    LastInputTime = time;
                }
            }
        }

        /// <summary>
        /// Determines if a move matches the current input history. Unless the move is
        /// a sub-move, the history is "consumed" to prevent it from matching twice.
        /// </summary>
        /// <returns>True if the move matches the input history.</returns>
        public bool Matches(Move move)
        {
            // If the move is longer than the buffer, it can't possibly match.
            if (Buffer.Count < move.Sequence.Length)
                return false;

            // Loop backwards to match against the most recent input.
            for (int i = 1; i <= move.Sequence.Length; ++i)
            {
                if (Buffer[Buffer.Count - i] != move.Sequence[move.Sequence.Length - i])
                {
                    return false;
                }
            }

            // Rnless this move is a component of a larger sequence,
            if (!move.IsSubMove)
            {
                // consume the used inputs.
                Buffer.Clear();
            }

            return true;
        }
    }
}
