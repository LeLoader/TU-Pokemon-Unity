﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// Ici est rangé un set de compétences afin d'être prête à être utilisé en combat
/// Vous pouvez changer/adapter ces skills au besoin du moment que c'est pertinent dans le reste du code.
namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Coup de poing basique
    /// </summary>
    public class Punch : Skill
    {
        public Punch() : base(TYPE.NORMAL, 70, StatusPotential.NONE, -1) { }
    }

    public class MiniPunch : Skill
    {
        public MiniPunch() : base(TYPE.NORMAL, 10, StatusPotential.NONE, -1) { }
    }

    /// <summary>
    /// Coup de poing basique ++
    /// </summary>
    public class MegaPunch : Skill
    {
        public MegaPunch() : base(TYPE.NORMAL, 7000, StatusPotential.NONE, 1) { }
    }

    /// <summary>
    /// Boule de feu qui ajoute le statut BURN
    /// </summary>
    public class FireBall : Skill
    {
        public FireBall() : base(TYPE.FIRE, 50, StatusPotential.BURN, 5) { }
    }
    /// <summary>
    /// Attaque eau basique
    /// </summary>
    public class WaterBlouBlou : Skill
    {
        public WaterBlouBlou() : base(TYPE.WATER, 20, StatusPotential.NONE, 10) { }
    }
    /// <summary>
    /// Attaque plante qui inflige le statut SLEEP
    /// </summary>
    public class MagicalGrass : Skill
    {
        public MagicalGrass() : base(TYPE.GRASS, 70, StatusPotential.SLEEP, 2) { }
    }

    public class CrazyBomb : Skill // J'ai jamais joué à un pokemon de ma vie, donc ça sort de mon imagination :////
    {
        public CrazyBomb() : base(TYPE.NORMAL, 10, StatusPotential.CRAZY, 1) { }
    }
}
