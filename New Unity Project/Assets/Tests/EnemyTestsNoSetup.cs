using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

//Golem features
//1.Cannot be knocked back by normal attack
//2.Move at constant speed towards players
//3.Attack player when next to player
//4.Die when out of health



namespace Tests
{

    public class EnemyTestsNoSetup
    {
        private GameObject testStage;
        private GameObject playerObject;
        private GameObject mockEnemyObject;

        [SetUp]
        public void Setup()
        {
            testStage = Spawner.SetUpTestStage(0, 0);
        }
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerObject);
            Object.Destroy(mockEnemyObject);
            Object.Destroy(testStage);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

        //Testing the LookAtPlayer() function in Update() of Enemy class
        [UnityTest]
        public IEnumerator EnemyLookingAtPlayer()
        {
            playerObject = Spawner.SpawnPlayer(-4, 1);
            mockEnemyObject = Spawner.SpawnMockEnemy(0, 1);

            //OnSpawn, the enemy faces the default side, which is right as intended by the sprites
            Assert.AreEqual(1, mockEnemyObject.GetComponent<Transform>().localScale.x);
            Debug.Log("first assert passed");

            //the mockEnemyObject should look to the left at the playerObject, as update() ran once
            yield return new WaitForSeconds(1.0f);            

            var mockEnemyXpos = mockEnemyObject.transform.position.x;
            var playerXpos = playerObject.transform.position.x;
          
            //Confirming that mockEnemyObject is to the left of the playerObject
            Assert.Greater(mockEnemyXpos, playerXpos);
            Assert.AreEqual(-1, mockEnemyObject.GetComponent<Transform>().localScale.x);

        }
        //Enemies should move to players up til a certain range depending on the enemy type, MockEnemy has a range same as Golem in the game (melee range)
        //in Update()
        //FUNCTION TESTED: MoveTowardsPlayersGrounded() in GroundEnemy.c
        //The enemy should move towards the player steadily
        [UnityTest]
        public IEnumerator MeleeMoveToPlayerAtCorrectSpeed()
        {
            playerObject = Spawner.SpawnPlayer(24, 1);
            mockEnemyObject = Spawner.SpawnMockEnemy(0, 1);

            var mockEnemyXPos = mockEnemyObject.transform.position.x;
            Assert.AreEqual(mockEnemyXPos, 0);
            Debug.Log("First assert passed");

            yield return new WaitForSeconds(2.0f);  //It should move towards (24,?) where the player is

            mockEnemyXPos = mockEnemyObject.transform.position.x;
            Assert.Greater(mockEnemyXPos, 0);
            Assert.IsTrue(mockEnemyXPos > 1.9 && mockEnemyXPos < 2.1f);    //within expected speed
        }

        //The enemy should stop moving when within range of the player
        //This further tests and expands the functionality of MoveTowardsPlayersGrounded()
        [UnityTest]
        public IEnumerator StopMovingWhenInRange()
        {
            playerObject = Spawner.SpawnPlayer(0, 1);
            mockEnemyObject = Spawner.SpawnMockEnemy(3, 1);

            yield return new WaitForSeconds(2.0f);
            var mockEnemyXPos = mockEnemyObject.transform.position.x;
            Assert.IsTrue(mockEnemyXPos > 1.49 && mockEnemyXPos < 1.51);     //more confirmation, it should be extremely accurate

            yield return new WaitForSeconds(2.0f);
            var newMockEnemyXPos = mockEnemyObject.transform.position.x;
               
            Assert.IsTrue(Mathf.Abs(newMockEnemyXPos-mockEnemyXPos) < Mathf.Epsilon);      //Mathf.Epsilon is a very small number (close to 0)

        }

        ///When within range, the enemy should attack the player. After some consideration, I decided that it would be more sensible
        ///to expand the TracePlayerGround to include attack() when in range, this tests the Attack() in it.
        //DamagerPlayer() is tested on EnemyTest.cs
        
        [UnityTest]
        public IEnumerator AttackWhenInRange()
        {
            playerObject = Spawner.SpawnPlayer(0, 1);
            mockEnemyObject = Spawner.SpawnMockEnemy(1.5f, 1);
            var player = playerObject.GetComponent<PlayerController>();

            yield return null;
            var originalPlayerHealth = player.health;

            //the animation takes 1.875seconds(calculated using the frames needed), so 2 seconds should give it time to attack the player ONCE
            yield return new WaitForSeconds(2.0f);

            var newPlayerHealth = player.health;

            Assert.AreEqual(originalPlayerHealth - 1, newPlayerHealth);
        }
        [UnityTest]
        public IEnumerator Stagger()
        {
            mockEnemyObject = Spawner.SpawnMockEnemy();


            yield return null;
        }

    }
}






//Object.Destroy(playerObject);
//Object.Destroy(mockEnemy);
//Object.Destroy(testStage);  
