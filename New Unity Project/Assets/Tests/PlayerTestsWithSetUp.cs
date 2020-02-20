using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

//Player feature
//1. Movement - Player can: left,right,jump | Player should look at the direction he is moving | Player need to be on the ground to jump
//2. Normal Attacks - Melee, Ranged, Jump Attack | Special Attacks - Punch, Magic | Alternating player attack | Jump Attack when in air
//4. Melee attack does damage based on (undecided) level
//5. Items pickup boost players levels

namespace Tests
{
    //Movement
    public class PlayerTestsWithSetUp
    {
        private GameObject playerObject;
        private PlayerController player;
        private GameObject testStage;


        [SetUp]
        public void Setup()
        {
            playerObject = Spawner.SpawnPlayer(0,1.5f);
            player = playerObject.GetComponent<PlayerController>();
            testStage = Spawner.SetUpTestStage(0,0);
            
        }
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerObject);
            Object.Destroy(testStage);
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator PlayerMovingNew()
        {
            var originalX = player.transform.position.x;
            player.Move(1);
            yield return new WaitForSeconds(2.0f);

            Assert.Greater(player.transform.position.x, originalX);
        }
        [UnityTest]
        public IEnumerator PlayerMovingRight()
        {
            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(1);
            player.UnityService = unityService;
            yield return new WaitForSeconds(2.0f);

            Assert.Greater(player.transform.position.x, originalX);
        }

        [UnityTest]
        public IEnumerator PlayerMovingLeft()
        {
            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(-1);
            player.UnityService = unityService;
            yield return new WaitForSeconds(2.0f);

            Assert.Less(player.transform.position.x, originalX);
        }
        [UnityTest]
        public IEnumerator PlayerLookDirection()
        {
            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(-1);
            player.UnityService = unityService;
            yield return new WaitForSeconds(2.0f);

            //Since the player model looks to the right on default, moving to the left should flip it
            Assert.AreEqual(player.transform.localScale.x, -1);
        }

        [UnityTest]
        public IEnumerator Jumping()
        {
            //2 seconds ensures that the player is now on the ground
            yield return new WaitForSeconds(2.0f);
            Assert.IsTrue(player.grounded);

            player.Jump();
            yield return null;
            //When the player jump, a force should be applied that would make the player move(upwards) in the y direction
            var upwardsVelocity = player.GetComponent<Rigidbody2D>().velocity.y;

            Assert.Greater(upwardsVelocity, Mathf.Epsilon);
        }

        //case for in the air done in PlayerTests

        //Attack
        //[UnityTest]
        //public IEnumerator PlayerMeleeAttack()
        //{
        //    Enemy enemy;
        //    player.MeleeHit(enemy)
        //}



        [UnityTest]
        public IEnumerator PlayerAttAnimationGround()
        {
            //tbc to 1 and 2?
            yield return new WaitForSeconds(2.0f);
            player.Attack();
            yield return null;
            Assert.AreEqual("BasicHit1",player.currentAnim);
        }
        [UnityTest]
        public IEnumerator PlayerAttChain()
        {
            //player should have 2 different attacks
            yield return null;
            var originalChain = player.attackChain;

            player.Attack();
            var newChain = player.attackChain;

            Assert.AreEqual(originalChain, 1);
            Assert.AreEqual(newChain, 2);
        }
        //shooting
        [UnityTest]
        public IEnumerator ArrowFlyWithSpeed()
        {

            player.ShootArrow();

            yield return null;
            var arrow = GameObject.FindGameObjectWithTag("Arrow");

            yield return new WaitForSeconds(2.0f);
            var NewX = GameObject.FindGameObjectWithTag("Arrow").transform.position.x; ;

            //PlayerPrefab faces the right side, therefore X should increase
            Assert.AreEqual(arrow, null);
        }




        [UnityTest]
        public IEnumerator PlayerSlowedWhenAttacking()
        {

            var unityService = Substitute.For<IUnityService>();
            unityService.GetAxis("Horizontal").Returns(1);
            player.UnityService = unityService;

            //time needed for character to land on the floor
            yield return new WaitForSeconds(2.0f);

            var originalSpeed = player.XSpeedCheck();

            player.Attack();

            yield return new WaitForSeconds(0.2f);

            var newSpeed = player.XSpeedCheck();

            Assert.Less(newSpeed, originalSpeed);
        }
        [UnityTest]
        public IEnumerator PlayerCannotNormalAttackWhenJumping()
        {
            playerObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            yield return new WaitForSeconds(10.0f);
        }

        [UnityTest]
        public IEnumerator JumpAttackAnimation()
        {
            //tbc setup in air
            playerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.Attack();
            yield return null;

            Assert.AreEqual("JumpAttack1",player.currentAnim);

        }


        //Seems to be hard to do with Nsub, using humble object instead
        [UnityTest]
        public IEnumerator PlayerAttGolemInGame()
        {
            //it should damage the golem at close range

            var golem = Spawner.SpawnGolem(1, 1.5f);
            var golemController = golem.GetComponent<Enemy>();

            var ground = Spawner.SetUpTestStage(0, 0);

            var originalHealth = golemController.health;

            yield return new WaitForSeconds(1.0f);
            player.Attack();

            //about half a second is needed for the swing to connect with the enemy
            yield return new WaitForSeconds(1.0f);
            Assert.AreEqual(originalHealth - 1, golemController.health);

            Object.Destroy(golem);

        }



        [UnityTest]
        public IEnumerator PlayerLevelUpSpeedOnPickingUpBoots()
        {
            var originalSpeedLevel = player.speedLevel;
            var boots = Spawner.SpawnBoots();

            //0.5s for the chest item to become available
            yield return new WaitForSeconds(1.0f);
            //29 11

            player.Pickup(boots);

            yield return null;
            Assert.AreEqual(originalSpeedLevel + 1, player.speedLevel);

            Object.Destroy(boots);
        }
        [UnityTest]
        public IEnumerator BootsDisappearingAfterPickup()
        {
            var boots = Spawner.SpawnBoots();

            //0.5s for the chest item to become available
            yield return new WaitForSeconds(1.0f);

            player.Pickup(boots);

            yield return null;
            Assert.IsTrue(boots == null);

            Object.Destroy(boots);
        }

        [UnityTest]
        public IEnumerator FlyingPunchMovesThePlayer()
        {

            yield return null;
            var originalX = player.transform.position.x;
            player.Punch();

            yield return new WaitForSeconds(2.0f);
            var newX = player.transform.position.x;

            //player looks right on spawn
            Assert.Greater(newX, originalX);
        }
        //not affected by normal movement input while punching
    }

}
