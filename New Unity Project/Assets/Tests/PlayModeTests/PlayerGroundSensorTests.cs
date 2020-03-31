using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

//Player feature
//1. Movement - Player can: left,right,jump | Player should look at the direction he is moving | Player need to be on the ground to jump
//2. Normal Attacks - Melee, Ranged | Special Attacks - Punch, Magic | Alternating player attack
//4. Melee attack does damage based on (undecided) level
//5. Items pickup boost players levels



namespace Tests
{
    public class PlayerGroundSensorTests
    {
        private GameObject testStage;
        private GameObject playerObject;
        private PlayerController player;
        [SetUp]
        public void Setup()
        {
            testStage = Spawner.SetUpTestStage(0, 0);
        }
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(testStage);
            Object.Destroy(playerObject);
        }

        [UnityTest]
        public IEnumerator _01GroundedIsFalseWhenPlayerInAir()
        {
            //To sense if the player is in the air(false)
            playerObject = Spawner.SpawnPlayer(0, 10.0f);
            player = playerObject.GetComponent<PlayerController>();
            playerObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            yield return new WaitForSeconds(10.0f);

            bool isPlayerGrounded = player.grounded;
            Assert.IsFalse(isPlayerGrounded);
        }

        [UnityTest]
        public IEnumerator _02GroundedIsTrueWhenPlayerOnground()
        {
            playerObject = Spawner.SpawnPlayer(0, 1.5f);
            player = playerObject.GetComponent<PlayerController>();
            //To sense if the player is on the ground
            //2 seconds ensures that the player is now on the ground
            yield return new WaitForSeconds(2.0f);

            bool isPlayerGrounded = player.grounded;
            Assert.IsTrue(isPlayerGrounded);
        }

        [UnityTest]
        public IEnumerator _03NonTDDLowestYValueForPlayerToBeSpawnedOnTheGround()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            Assert.IsTrue(player.grounded);
        }
    }

    }
