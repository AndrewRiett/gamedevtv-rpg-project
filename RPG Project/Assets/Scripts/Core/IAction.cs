using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public interface IAction
    {
        /// <summary>
        /// Interface method that cancels a currentAction.
        /// </summary>
        void Cancel();
    }
}