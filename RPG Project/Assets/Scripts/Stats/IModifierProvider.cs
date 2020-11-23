using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        /// <summary>
        /// Returns a collection of additive modifiers of the particular stat type.
        /// </summary>
        /// <param name="statType"></param>
        IEnumerable<float> GetAdditiveModifiers(StatType statType);

        /// <summary>
        /// Returns a percent collection of additive modifiers of the particular stat type.
        /// </summary>
        /// <param name="statType"></param>
        IEnumerable<float> GetAdditivePercentages(StatType statType);
    }
}