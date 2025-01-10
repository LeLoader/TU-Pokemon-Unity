
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir /////////// FAIT
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs) ///////// FAIT
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
        // - Gérer les PP (limite du nombre d'utilisation) d'une attaque, ////////// FAIT
        // si on selectionne une attack qui a 0 PP on inflige 0

        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
        // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
        // de ces anciens TU pour ta nouvelle situation.

        [Test]
        public void FightWithBurnStatusAttack()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, mewtwo);
            FireBall fb = new FireBall();
            Punch p = new Punch();

            f.ExecuteTurn(fb, p); // Pikachu attack with Fireball

            Assert.IsInstanceOf<BurnStatus>(mewtwo.CurrentStatus);
            Assert.IsNull(pikachu.CurrentStatus);
            Assert.That(mewtwo.CurrentHealth, Is.EqualTo(mewtwo.MaxHealth - mewtwo.CurrentStatus.DamageEachTurn - TypeResolver.GetFactor(pikachu.BaseType, mewtwo.BaseType) * fb.Power));
        }

        [Test]
        public void FightWithSleepStatusAttack()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, mewtwo);
            Punch p = new Punch();
            MagicalGrass mg = new MagicalGrass();

            f.ExecuteTurn(p, mg); // Mewtwo attacks with MagicalGrass

            Assert.IsInstanceOf<SleepStatus>(pikachu.CurrentStatus);
            Assert.IsNull(mewtwo.CurrentStatus);
            Assert.That(pikachu.CurrentStatus.CanAttack, Is.EqualTo(false)); // Pikachu is asleep

            Assert.That(mewtwo.CurrentHealth, Is.EqualTo(mewtwo.MaxHealth)); // Mewtwo attack first, pikachu is asleep therefor attack misses
        }

        [Test]
        public void FightWithCrazyAttack()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, mewtwo);
            Punch p = new Punch();
            CrazyBomb cb = new CrazyBomb();

            f.ExecuteTurn(p, cb); // Mewtwo attacks with CrazyBomb

            Assert.IsInstanceOf<CrazyStatus>(pikachu.CurrentStatus);
            Assert.IsNull(mewtwo.CurrentStatus);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth - TypeResolver.GetFactor(mewtwo.BaseType, pikachu.BaseType) * Math.Max(cb.Power - pikachu.Defense, 0) - (pikachu.CurrentStatus.DamageOnAttack * pikachu.Attack))); // Pikachu is crazy so he attacks himself while attack mewtwo
        }

        [Test]
        public void DecreasePP()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var punch = new Punch();
            var waterbloublou = new WaterBlouBlou();
            var oldPP = waterbloublou.PP;

            pikachu.ReceiveAttack(punch); 
            Assert.That(punch.PP, Is.EqualTo(-1)); //Infinite pp
            pikachu.ReceiveAttack(waterbloublou);
            Assert.That(waterbloublou.PP, Is.EqualTo(oldPP - 1)); //10 - 1 = 9
        }

        [Test]
        public void DecreasePPTo0()
        {
            var pikachu = new Character(100, 50, 0, 20, TYPE.NORMAL);
            var cb = new CrazyBomb();

            pikachu.ReceiveAttack(cb);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(90)); // 100 - 10 (0 def)
            pikachu.ReceiveAttack(cb);
            Assert.That(cb.PP, Is.EqualTo(0));
            Assert.That(cb.Power, Is.EqualTo(0));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(90)); // Power is 0, nothing has changed
        }

        // 0 DEF FOR EASIER CALCULATION

        [Test]
        public void FireGrassInteraction()
        {
            var salameche = new Character(100, 10, 0, 10, TYPE.FIRE);
            var bulbizarre = new Character(100, 10, 0, 10, TYPE.GRASS);
            Fight f = new Fight(salameche, bulbizarre);
            var punch = new MiniPunch();

            f.ExecuteTurn(punch, punch);
            Assert.That(salameche.CurrentHealth, Is.EqualTo(salameche.MaxHealth - punch.Power * TypeResolver.GetFactor(bulbizarre.BaseType, salameche.BaseType))); // FIRE RES GRASS => 100 - 8 = 92
            Assert.That(salameche.CurrentHealth, Is.EqualTo(92));
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(bulbizarre.MaxHealth - punch.Power * TypeResolver.GetFactor(salameche.BaseType, bulbizarre.BaseType))); // GRASS WEAK FIRE => 100 - 12 = 88
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(88));
        }

        [Test]
        public void WaterFireInteraction()
        {
            var salameche = new Character(100, 10, 0, 10, TYPE.FIRE);
            var carapuce = new Character(100, 10, 0, 10, TYPE.WATER);
            Fight f = new Fight(salameche, carapuce);
            var punch = new MiniPunch();

            f.ExecuteTurn(punch, punch);
            Assert.That(salameche.CurrentHealth, Is.EqualTo(salameche.MaxHealth - punch.Power * TypeResolver.GetFactor(carapuce.BaseType, salameche.BaseType))); // FIRE WEAK WATER => 100 - 12 = 88
            Assert.That(salameche.CurrentHealth, Is.EqualTo(88));
            Assert.That(carapuce.CurrentHealth, Is.EqualTo(carapuce.MaxHealth - punch.Power * TypeResolver.GetFactor(salameche.BaseType, carapuce.BaseType))); // WATER RES FIRE => 100 - 8 = 92
            Assert.That(carapuce.CurrentHealth, Is.EqualTo(92));
        }

        [Test]
        public void GrassWaterInteraction()
        {
            var carapuce = new Character(100, 10, 0, 10, TYPE.WATER);
            var bulbizarre = new Character(100, 10, 0, 10, TYPE.GRASS);
            Fight f = new Fight(carapuce, bulbizarre);
            var punch = new MiniPunch();

            f.ExecuteTurn(punch, punch);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(bulbizarre.MaxHealth - punch.Power * TypeResolver.GetFactor(carapuce.BaseType, bulbizarre.BaseType))); // GRASS RES WATER => 100 - 8 = 92
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(92));
            Assert.That(carapuce.CurrentHealth, Is.EqualTo(carapuce.MaxHealth - punch.Power * TypeResolver.GetFactor(bulbizarre.BaseType, carapuce.BaseType))); // WATER WEAK GRASS => 100 - 12 = 88
            Assert.That(carapuce.CurrentHealth, Is.EqualTo(88));
        }

        [Test]
        public void NormalVsAllInteraction()
        {
            var carapuce = new Character(100, 10, 0, 10, TYPE.WATER);
            var bulbizarre = new Character(100, 10, 0, 10, TYPE.GRASS);
            var salameche = new Character(100, 10, 0, 10, TYPE.FIRE);
            var pikachu = new Character(100, 10, 0, 10, TYPE.NORMAL);
            var punch = new MiniPunch();

            pikachu.ReceiveAttack(punch, carapuce.BaseType);
            pikachu.ReceiveAttack(punch, bulbizarre.BaseType);
            pikachu.ReceiveAttack(punch, salameche.BaseType);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(70));
            bulbizarre.ReceiveAttack(punch, pikachu.BaseType);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(90));
            salameche.ReceiveAttack(punch, pikachu.BaseType);
            Assert.That(salameche.CurrentHealth, Is.EqualTo(90));
            carapuce.ReceiveAttack(punch, pikachu.BaseType);
            Assert.That(carapuce.CurrentHealth, Is.EqualTo(90));
        }
    }
}
