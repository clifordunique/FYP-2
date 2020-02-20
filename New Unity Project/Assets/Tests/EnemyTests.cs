using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests


///Many functions are meant to be ran at all times in Update() and does not need to be called, the tests 
///involving these fucntions will be commented with "in Update()"

///Enemy is an abstract class, therefore, tests are carried out by using a concrete class that is made for 
///testing - MockEnemy. This class implements all of the functions in enemy and has mock attributes(mostly arbituary)
///for testing.

///EnemyTests are used to test function which does not requires a specific scene (the location of the characters does
///not affect the test, they do not need to be on a ground, etc.)
///Many other tests involving the things mentioned above are done on EnemyTestNoSetUp, as it allows mockEnemy and player
///to be spawned in different locations as needed.
{
    public class EnemyTests
    {
        private GameObject testStage;

        private GameObject mockEnemyObject;
        private MockEnemyController mockEnemy;

        private GameObject playerObject;
        private PlayerController player;
        private BoxCollider2D playerBoxCollider2D;
        [SetUp]
        public void Setup()
        {

            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            playerBoxCollider2D = playerObject.GetComponent<BoxCollider2D>();

            mockEnemyObject = Spawner.SpawnMockEnemy();
            mockEnemy = mockEnemyObject.GetComponent<MockEnemyController>();

            testStage = Spawner.SetUpTestStage(0, 0);

        }
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerObject);
            Object.Destroy(mockEnemyObject);
            Object.Destroy(testStage);           
        }

        //All enemy should be able to take damage from the player
        [UnityTest]
        public IEnumerator TakingDamage()
        {

            yield return null;
            var originalHealth = mockEnemy.health;
            mockEnemy.TakeDamage(1);
            var newHealth = mockEnemy.health;
            Assert.AreEqual(originalHealth - 1, newHealth);
        }
        
        //All enemy should have some mechanisms to damage the player, by detecting the player's collider(very high chance)
        [UnityTest]
        public IEnumerator DamagePlayer()
        {
            //added after UI was made as a UI instance was needed for the test to function

            yield return null;
            var originalPlayerHealth = player.health;
            mockEnemy.DamagePlayer(playerBoxCollider2D);
            var newPlayerHealth = player.health;
            Assert.AreEqual(originalPlayerHealth - 1, newPlayerHealth);


        }
        //Look at player done on nosetup


        //Everything dies... An enemy should 1.plays a death animation when it dies 
        //                                   2. disappear from the scene a few seconds after
        [UnityTest]
        public IEnumerator EnemyDeath()
        {            
            mockEnemy.health = 0;
            yield return new WaitForSeconds(6.0f);

            Assert.IsTrue(mockEnemyObject == null);
            //Assert(mockEnemy.currentAnim = "Death");
        }

    }
}
