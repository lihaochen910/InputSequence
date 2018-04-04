using UnityEngine;

namespace InputSequence
{
    /// <summary>
    /// 秘籍输入引擎
    /// </summary>
    public class InputSequenceEngine : MonoBehaviour
    {
        //public bool Active = false;

        // This is the master list of moves in logical order. This array is kept
        // around in order to draw the move list on the screen in this order.
        Move[] moves;
        // The move list used for move detection at runtime.
        MoveList moveList;

        InputManager inputManager;

        // Time until the currently "active" move dissapears from the screen.
        readonly float MoveTimeOut = 1.0f;

        Move playerMove;
        float playerMoveTime;

        // Direction textures.
        //Sprite upTexture;
        //Sprite downTexture;
        //Sprite leftTexture;
        //Sprite rightTexture;
        //Sprite upLeftTexture;
        //Sprite upRightTexture;
        //Sprite downLeftTexture;
        //Sprite downRightTexture;

        void Awake()
        {
            moves = InputSequence.Moves.Cmd.Moves;

            moveList = new MoveList(moves);

            inputManager = new InputManager(TeamUtility.IO.PlayerID.One, moveList.LongestMoveLength);

            Debug.Log("秘籍引擎初始化...".Colored(Colors.cyan));
        }

        void Start()
        {
            //var sprites = Resources.LoadAll<Sprite>("Sprites/Direction");

            //upTexture = (from sp in sprites where sp.name == "Up" select sp).First();
            //downTexture = (from sp in sprites where sp.name == "Down" select sp).First();
            //leftTexture = (from sp in sprites where sp.name == "Left" select sp).First();
            //rightTexture = (from sp in sprites where sp.name == "Right" select sp).First();
            //upLeftTexture = (from sp in sprites where sp.name == "UpLeft" select sp).First();
            //upRightTexture = (from sp in sprites where sp.name == "UpRight" select sp).First();
            //downLeftTexture = (from sp in sprites where sp.name == "DownLeft" select sp).First();
            //downRightTexture = (from sp in sprites where sp.name == "DownRight" select sp).First();
        }

        void Update()
        {
            if (Time.time - playerMoveTime > MoveTimeOut)
                playerMove = null;

            inputManager.Update();

            Move newMove = moveList.DetectMove(inputManager);

            if (newMove != null)
            {
                playerMove = newMove;
                playerMoveTime = Time.time;

                Debug.Log("<color=#00FF00>" + playerMove.Name + "</color> <color=#99FFFF>" + playerMove.PerformMove() + "</color>");
            }
        }
        /// <summary>
        /// 激活失活控制
        /// </summary>
        private void HandleActiveInput()
        {
            //if (TeamUtility.IO.InputManager.GetKeyDown(KeyCode.Return))
            //{
            //    Active = !Active;
            //    if (Active)
            //        Debug.Log("<color=#FFFF66>秘籍系统</color> <color=#00FF00>开启</color>");
            //    else Debug.Log("<color=#FFFF66>秘籍系统</color> <color=#FF3300>关闭</color>");
            //}
        }

        //Sprite Direction2Sprite(KeyCode dir)
        //{
        //    switch (dir)
        //    {
        //        case Direction.Up:
        //            return upTexture;
        //        case Direction.Down:
        //            return downTexture;
        //        case Direction.Left:
        //            return leftTexture;
        //        case Direction.Right:
        //            return rightTexture;
        //        case Direction.UpLeft:
        //            return upLeftTexture;
        //        case Direction.UpRight:
        //            return upRightTexture;
        //        case Direction.DownLeft:
        //            return downLeftTexture;
        //        case Direction.DownRight:
        //            return downRightTexture;
        //        default:
        //            return null;
        //    }
        //}
    }
}
