using Racing.Map;
using Racing.Map.Tracking;
using Racing.UI;
using Racing.UI.InGame;
using Racing.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Racing.User
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// List of all active players under AI control
        /// </summary>
        public static List<Player> AIPlayers { get { return FindObjectsOfType<Player>().Where(p => p.InputType == InputType.AI).ToList(); } }

        /// <summary>
        /// List of all active players under human control
        /// </summary>
        public static List<Player> HumanPlayers { get { return FindObjectsOfType<Player>().Where(p => p.InputType == InputType.Player).ToList(); } }

        /// <summary>
        /// List of all checkpoints the player has passed through in one lap
        /// </summary>
        public List<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
        public int TargetCheckpointIndex { get; set; } = 0;
        public int PreviousCheckpointIndex { get; set; }
        public int LapNumber
        {
            get { return lapNumber; }
            set
            {
                lapNumber = value;
                if (InputType == InputType.Player)
                    UserInterface.GetControllerAsType<InGameUIController>().SetLapIndicator(value);
            }
        }
        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                if (InputType == InputType.Player)
                    UserInterface.GetControllerAsType<InGameUIController>().SetPositionIndicator(value);
            }
        }

        private int lapNumber = 1;
        private int position = 0;
        public InputType InputType;
        public bool Finished { get; set; } = false;

        public override string ToString()
        {
            return $"{transform.parent.name} || {LapNumber} || {TargetCheckpointIndex} || {Vector3.Distance(transform.position, RaceScene.CurrentTrack.Checkpoints[TargetCheckpointIndex].transform.position)}";
        }

        /// <summary>
        /// Returns a modified TargetCheckpointIndex for determination of track position
        /// </summary>
        public int TargetIndexForPosition()
        {
            if (PreviousCheckpointIndex == RaceScene.CurrentTrack.Checkpoints.Count - 1 && TargetCheckpointIndex == 0)
            {
                return RaceScene.CurrentTrack.Checkpoints.Count;
            }

            return TargetCheckpointIndex;
        }
    }
}
