using UnityEngine;

namespace InputSequence.Moves
{
    public static class Cmd
    {
        public static Move[] Moves = new Move[]
        {
            new Move("ComeBackToLife",  Direction.Up, Direction.Left, Direction.Down, Direction.Right, Direction.Up, Direction.Down){
                PerformMoveAction = Comeback2Life },

            new Move("RecoveryHeath",  Direction.Left, Direction.Left, Direction.Right, Direction.Right){
                PerformMoveAction = RecoveryHeath },

            new Move("ChangeWeapon",  Direction.Up, Direction.Down, Direction.Left, Direction.Right){
                PerformMoveAction = ChangeWeapon },

            new Move("God", "Nukee"){
                PerformMoveAction = GodeMode },

            new Move("KillCurrentRoomAllEnemy", Direction.Left, Direction.Right, Direction.Left, Direction.Right, Direction.Left, Direction.Right){
                PerformMoveAction = KillCurrentRoomAllEnemy },
        };

        private static string Comeback2Life()
        {
            GetPlayerGameObject().GetComponent<Player.PlayerHealth>().ComeBackToLife();
            return "满血复活";
        }

        private static string RecoveryHeath()
        {
            GetPlayerGameObject().GetComponent<Player.PlayerHealth>().RecoveryHeath();
            return "回满血";
        }

        private static string ChangeWeapon()
        {
            GetPlayerGameObject().GetComponent<WeaponChanger>().ChangeRandomWeapon();
            return "随机武器";
        }

        private static string GodeMode()
        {
            GetPlayerGameObject().GetComponent<Player.PlayerHealth>().ActiveGodMode(!GetPlayerGameObject().GetComponent<Player.PlayerHealth>().GodMode);
            return GetPlayerGameObject().GetComponent<Player.PlayerHealth>().GodMode ? "上帝模式激活" : "上帝模式关闭";
        }

        private static string KillCurrentRoomAllEnemy()
        {
            foreach(Transform monster in LevelMapper.Instance.CurrentRoomGameObject.transform.Find("Monsters"))
            {
                monster.GetComponent<PlayMakerFSM>().Fsm.SetState("Dead");
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取玩家游戏物体
        /// </summary>
        /// <returns></returns>
        public static GameObject GetPlayerGameObject()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
    }
}
