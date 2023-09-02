namespace ChainsawMan
{
    /// <summary>
    /// An interface to make a game object able to apply or receive damage
    /// </summary>
    public interface IDamage
    {
        /// <summary>
        /// Apply Damage to a unit
        /// </summary>
        /// <param name="dmg"></param>
        public void ApplyDamage(float dmg);
    }
}
