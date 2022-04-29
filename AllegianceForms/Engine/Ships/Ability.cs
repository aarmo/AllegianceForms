using System;

namespace AllegianceForms.Engine.Ships
{
    public class Ability
    {
        public delegate void AbilityEventHandler(Ability sender);
        
        public event AbilityEventHandler AbilityStarted;
        public event AbilityEventHandler AbilityFinished;

        public EAbilityType AbilityEffect { get; private set; }
        public bool Active { get; private set; } = false;
        public DateTime AvailableAfter { get; private set; } = DateTime.MinValue;
        public DateTime InActiveAfter { get; private set; } = DateTime.MinValue;

        // TODO: Load these values from a data file so we can tweak them individually
        public float CooldownDuration { get; private set; } = 30;
        public float AbilityEffectMultiplier { get; private set; } = 1.5f;
        public float AbilityDuration { get; private set; } = 5;

        public Ability(EAbilityType type, float cooldownBonus = 1f, float durationBonus = 1f, float effectBonus = 1f)
        {
            AbilityEffect = type;
            CooldownDuration *= cooldownBonus;
            AbilityDuration *= durationBonus;
            AbilityEffectMultiplier *= effectBonus;
        }

        public bool IsActive()
        {
            if (!Active) return false;

            if (Active && DateTime.Now > InActiveAfter)
            {
                Active = false;

                if (AbilityFinished != null) AbilityFinished(this);
                return false;
            }

            return true;
        }

        public bool IsReady()
        {
            if (Active || DateTime.Now < AvailableAfter) return false;

            return true;
        }

        public bool Activate()
        {
            if (!IsReady()) return false;

            Active = true;
            AvailableAfter = DateTime.Now.AddSeconds(CooldownDuration);
            InActiveAfter = DateTime.Now.AddSeconds(AbilityDuration);

            if (AbilityStarted != null) AbilityStarted(this);
            return true;
        }
    }
}