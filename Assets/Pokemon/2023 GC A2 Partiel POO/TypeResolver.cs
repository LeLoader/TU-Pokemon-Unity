
using System;
using System.Collections.Generic;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition des types dans le jeu
    /// </summary>
    public enum TYPE { NORMAL, WATER, FIRE, GRASS }

    public class TypeResolver
    {

        static Dictionary<TYPE, TYPE> weakness = new Dictionary<TYPE, TYPE>()
        {
            { TYPE.WATER, TYPE.GRASS },
            { TYPE.FIRE, TYPE.WATER },
            { TYPE.GRASS, TYPE.FIRE },
        };

        static Dictionary<TYPE, TYPE> resistance = new Dictionary<TYPE, TYPE>()
        {
            { TYPE.WATER, TYPE.FIRE },
            { TYPE.FIRE, TYPE.GRASS },
            { TYPE.GRASS, TYPE.WATER },
        };

        /// <summary>
        /// Récupère le facteur multiplicateur pour la résolution des résistances/faiblesses
        /// WATER faible contre GRASS, resiste contre FIRE
        /// FIRE faible contre WATER, resiste contre GRASS
        /// GRASS faible contre FIRE, resiste contre WATER
        /// </summary>
        /// <param name="attacker">Type de l'attaque (le skill)</param>
        /// <param name="receiver">Type de la cible</param>
        /// <returns>
        /// Normal returns 1 if attacker or receiver
        /// 0.8 if resist
        /// 1.0 if same type
        /// 1.2 if vulnerable
        /// </returns>
        public static float GetFactor(TYPE attacker, TYPE receiver)
        {
            if (weakness.TryGetValue(receiver, out TYPE type1))
            {
                if (type1 == attacker)
                {
                    return 1.2f;
                }
            }

            if (resistance.TryGetValue(receiver, out TYPE type2))
            {
                if (type2 == attacker)
                {
                    return 0.8f;
                }
            }

            return 1f;
        }
    }
}
