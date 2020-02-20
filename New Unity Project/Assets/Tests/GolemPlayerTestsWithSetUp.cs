using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GolemTestsSetUp
    {
        private GameObject player;
        private PlayerController playerController;
        private EdgeCollider2D playerCollider2d;

        private GameObject golem;
        private GolemController golemController;
        private BoxCollider2D golemCollider2d;


        [SetUp]
        public void Setup()
        {
            player = Spawner.SpawnPlayer();
            playerController = player.GetComponent<PlayerController>();
            playerCollider2d = player.GetComponent<EdgeCollider2D>();


            golem = Spawner.SpawnGolem();
            golemController = golem.GetComponent<GolemController>();
            golemCollider2d = golem.GetComponent<BoxCollider2D>();

        }
        // A Test behaves as an ordinary method
        [TearDown]
        public void Teardown()
        {
            if (player != null) { Object.Destroy(player); }
            if (golem != null) { Object.Destroy(golem); }           
        }
        [Test]
        public void PrefabNotNull()
        {
            Assert.IsNotNull(golem);
        }
        //taking damage monster
        [UnityTest]
        public IEnumerator GolemLookingAtPlayer()
        {
            //Spawner.SpawnPlayer(2,1);
            yield return null;
        }


        [UnityTest]
        public IEnumerator GolemTakingDamage()
        {
            var originalGolemHealth = golemController.health;
            golemController.TakeDamage(1);
            yield return null;
            var updatedHealth = golemController.health;

            Assert.AreEqual(originalGolemHealth - 1, updatedHealth);
            
        }

        //Combat Melee
        [UnityTest]
        public IEnumerator MeleeHit()
        {
            var originalHealth = golemController.health;

            Collider2D[] testList  = { golemCollider2d };

            //the golem is attacked and should take 1 damage
            playerController.MeleeHit(testList);

            yield return null;

            Assert.AreEqual(originalHealth - playerController.damage, golemController.health);
        }
        [UnityTest]
        public IEnumerator GolemDeath()
        {
            int originalHealth = golemController.health;

            Collider2D[] testList = { golemCollider2d };

            //the golem is attacked 5 times, and should die as health will be reduced to 0
            for (int i = 0; i< 5; i++)
            {
                playerController.MeleeHit(testList);
            }
            
            yield return new WaitForSeconds(5.5f);

            //Assert.IsNull(golem);
            Assert.IsTrue(golem == null);
        }
        [UnityTest]
        public IEnumerator GolemAttack()
        {

            int originalhealth = playerController.health;
            golemController.DamagePlayer(playerCollider2d);

            yield return new WaitForSeconds(10.0f);

            Assert.AreEqual(originalhealth - golemController.damage, playerController.health);
        }


        //Tests below requires the gameObjects to be instantiated at certain location. Therefore setup is somewhat overidden here


    }
}
