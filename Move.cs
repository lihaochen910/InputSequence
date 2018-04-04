#region File Description
//-----------------------------------------------------------------------------
// Move.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
//using Microsoft.Xna.Framework.Input;
using UnityEngine;
#endregion

namespace InputSequence
{
    /// <summary>
    /// Describes a sequences of buttons which must be pressed to active the move.
    /// A real game might add a virtual PerformMove() method to this class.
    /// </summary>
    public class Move
    {
        public string Name;

        // The sequence of button presses required to activate this move.
        public KeyCode[] Sequence;

        // Set this to true if the input used to activate this move may
        // be reused as a component of longer moves.
        public bool IsSubMove;

        // 该秘籍成立时要执行的Action
        public System.Func<String> PerformMoveAction;

        public Move(string name, params KeyCode[] sequence)
        {
            Name = name;
            Sequence = sequence;
        }

        public Move(string name, string commandWord)
        {
            Name = name;
            Sequence = CommandWord2Sequence(commandWord);
        }

        public virtual string PerformMove()
        {
            if (PerformMoveAction != null)
                return PerformMoveAction();

            return string.Empty;
        }
        /// <summary>
        /// 指令单词转KeyCode序列
        /// </summary>
        /// <param name="commandWord"></param>
        /// <returns>KeyCodeSequence</returns>
        private static KeyCode[] CommandWord2Sequence(string commandWord)
        {
            var seq = new System.Collections.Generic.List<KeyCode>();

            foreach(var C in commandWord.ToUpper())
            {
                seq.Add(Char2KeyCode(C));
            }

            return seq.ToArray();
        }
        /// <summary>
        /// 字符转KeyCode
        /// </summary>
        /// <param name="C"></param>
        private static KeyCode Char2KeyCode(char C)
        {
            switch(C)
            {
                case 'A': return KeyCode.A;
                case 'B': return KeyCode.B;
                case 'C': return KeyCode.C;
                case 'D': return KeyCode.D;
                case 'E': return KeyCode.E;
                case 'F': return KeyCode.F;
                case 'G': return KeyCode.G;
                case 'H': return KeyCode.H;
                case 'I': return KeyCode.I;
                case 'J': return KeyCode.J;
                case 'K': return KeyCode.K;
                case 'L': return KeyCode.L;
                case 'M': return KeyCode.M;
                case 'N': return KeyCode.N;
                case 'O': return KeyCode.O;
                case 'P': return KeyCode.P;
                case 'Q': return KeyCode.Q;
                case 'R': return KeyCode.R;
                case 'S': return KeyCode.S;
                case 'T': return KeyCode.T;
                case 'U': return KeyCode.U;
                case 'V': return KeyCode.V;
                case 'W': return KeyCode.W;
                case 'X': return KeyCode.X;
                case 'Y': return KeyCode.Y;
                case 'Z': return KeyCode.Z;
                default: return KeyCode.None;
            }
        }
    }
}
