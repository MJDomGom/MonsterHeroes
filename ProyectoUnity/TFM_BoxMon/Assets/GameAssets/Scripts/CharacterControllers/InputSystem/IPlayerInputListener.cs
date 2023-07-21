using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.blackantgames.character.controller
{

    /// <summary>
    /// Interface for the player input listener implementation
    /// 
    /// PlayerInputListener will listen for the player input independently of
    /// how is this input being provided
    /// </summary>
    public interface IPlayerInputListener
    {

        /// <summary>
        /// Notifies the movement value parameter for the character movement
        /// </summary>
        /// <param name="lateralDirection">Lateral direction of the movement</param>
        public void NotifyMovementValues(float lateralDirection);

        /// <summary>
        /// Notifies that it was requested to perform a jump
        /// </summary>
        public void NotifyJumpRequest();
    }
}
