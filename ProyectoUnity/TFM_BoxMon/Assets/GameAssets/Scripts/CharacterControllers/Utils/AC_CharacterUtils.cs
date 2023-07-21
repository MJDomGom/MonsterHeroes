using UnityEngine;

namespace com.blackantgames.character.controller
{
    /// <summary>
    /// Static auxiliary class that represents the Animator Controller components that allows
    /// to work and interact with the animator
    /// </summary>
    public static class AC_CharacterUtils
    {
        public static int AC_FLOAT_MOVEMENT_SPEED = Animator.StringToHash("movementSpeed");
        public static int AC_TRIGGER_JUMP = Animator.StringToHash("jump");
        public static int AC_BOOL_ON_AIR = Animator.StringToHash("onAir");
        public static int AC_TRIGGER_DEATH = Animator.StringToHash("deadTrigger");
        public static int AC_TRIGGER_ALERT = Animator.StringToHash("alert");
        public static int AC_TRIGGER_SHOOT = Animator.StringToHash("shoot");
        public static int AC_TRIGGER_BOUGHT = Animator.StringToHash("bought");
        public static int AC_TRIGGER_SELECTED = Animator.StringToHash("selected");
		public static int AC_TRIGGER_CANNOT_BUY = Animator.StringToHash("cannotBuy");
	}
}