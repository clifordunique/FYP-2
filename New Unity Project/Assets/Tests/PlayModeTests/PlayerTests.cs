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
        public IEnumerator _01PlayerDoesNotMoveWithoutInput()
        {

            playerObject = Spawner.SpawnPlayer(0, 0.6f);
            player = playerObject.GetComponent<PlayerController>();

            yield return null;

            var originalXPosition = player.GetPosition().x;

            yield return null;
            Assert.AreEqual(player.GetPosition().x, originalXPosition);
        }

        [UnityTest]
        public IEnumerator _02PlayerMoveRight()
        {
            var originalXPosition = 0;
            playerObject = Spawner.SpawnPlayer(originalXPosition, 1f);
            player = playerObject.GetComponent<PlayerController>();

            yield return null;
            
            player.Move(1);
            Debug.Log(player.GetPosition().x);
            Debug.Log(player.XSpeedCheck());
            yield return new WaitForSeconds(0.1f);


            Assert.Greater(player.GetPosition().x, originalXPosition);
        }

        [UnityTest]
        public IEnumerator _03PlayerMoveLeft()
        {
            var originalXPosition = 0;
            playerObject = Spawner.SpawnPlayer(originalXPosition, 0.6f);
            player = playerObject.GetComponent<PlayerController>();

            yield return null;
            var x = player.GetPosition().x;

            player.Move(-1);
            Debug.Log(player.XSpeedCheck());
            yield return new WaitForSeconds(0.1f);

            Debug.Log(x);
            Debug.Log(originalXPosition);
            Assert.Less(player.GetPosition().x, originalXPosition);
        }


        [UnityTest]
        public IEnumerator _04PlayerLookDirection()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            //check that it is 1 (looks right by default)
            Assert.AreEqual(player.GetLookDirection(), 1);

            player.ChangeLookDirection(-1);
            yield return null;

            //Since the player model looks to the right on default, moving to the left should flip it
            Assert.AreEqual(player.GetLookDirection(), -1);
        }

        [UnityTest]
        public IEnumerator _05Jumping()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            Assert.IsTrue(player.grounded);

            player.Jump();
            yield return null;
            //When the player jump, a force should be applied that would make the player move(upwards) in the y direction
            var upwardsVelocity = player.GetComponent<Rigidbody2D>().velocity.y;

            //Mathf.Epsilon is a number very close to 0
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
        public IEnumerator _06PlayerAttAnimationGround()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            player.StartAttack();

            //1 frame to start the animation
            yield return null;

            Assert.IsTrue(player.IsCurrentAnimaion("BasicHit1") || player.IsCurrentAnimaion("BasicHit2"));
        }

        [UnityTest]
        public IEnumerator _07PlayerAttackMoveAlternates()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();

            //player should have 2 different attacks
            yield return null;
            var originalMove = player.attackMove;
            Assert.AreEqual(originalMove, 1);

            player.StartAttack();
            var newMove = player.attackMove;
          
            Assert.AreEqual(newMove, 2);
        }
        [UnityTest]
        public IEnumerator _08ShootArrowAnimationPlays()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            player.ShootArrow();

            //1 frame to start the animation
            yield return null;

            Assert.IsTrue(player.IsCurrentAnimaion("RangedAttack"));

            //Names have (Clone) at the end for instantiated GameObjects)

        }
        //shooting
        [UnityTest]
        public IEnumerator _08ArrowLaunchedWhenShooting()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            Assert.IsTrue(player.grounded);

            player.Launch();

            yield return null;

            //Names have (Clone) at the end for instantiated GameObjects)
            var arrow = GameObject.Find("Arrow(Clone)");

            //PlayerPrefab faces the right side, therefore X should increase
            Assert.IsNotNull(arrow);
            Object.Destroy(arrow);
        }
        [UnityTest]
        public IEnumerator _09ArrowFiredAtTheCorrectDirectionRight()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            Assert.IsTrue(player.grounded);

            player.Launch();
            var arrow = GameObject.Find("Arrow(Clone)");

            Debug.Log(arrow.transform.localScale.x);
            Debug.Log(player.transform.localScale.x);

            Assert.IsTrue(arrow.transform.localScale.x == player.GetLookDirection());

            Object.Destroy(arrow);
        }
        [UnityTest]
        public IEnumerator _10ArrowFiredAtTheCorrectDirectionLeft()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            player.ChangeLookDirection(-1);

            player.Launch();
            var arrow = GameObject.Find("Arrow(Clone)");
            Assert.IsTrue(arrow.transform.localScale.x == player.transform.localScale.x);

            Object.Destroy(arrow);
        }

        [UnityTest]
        public IEnumerator _09PlayerSlowedWhenAttacking()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();

            yield return null;
            player.Move(1);
            var originalSpeed = player.XSpeedCheck();

            player.StartAttack();
            //1 frame needed for the animation to start
            yield return null;

            //yield return null;
            player.Move(1);
            var newSpeed = player.XSpeedCheck();
            Debug.Log(originalSpeed + "->" + newSpeed);
            Assert.Less(newSpeed, originalSpeed - Mathf.Epsilon);
        }

        [UnityTest]
        public IEnumerator _10PlayerCannotNormalAttackWhenInAir()
        {
            playerObject = Spawner.SpawnPlayer(0, 10f);
            player = playerObject.GetComponent<PlayerController>();
            playerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            yield return null;

            player.StartAttack();
            //1 frame to start the animation
            yield return null;
            //1 frame to run update() to get the correct information
            yield return null;

            Assert.IsFalse(player.IsCurrentAnimaion("BasicHit1") || player.IsCurrentAnimaion("BasicHit2"));
        }
        [UnityTest]
        public IEnumerator _10PlayerCannotShootArrowsWhenInAir()
        {
            playerObject = Spawner.SpawnPlayer(0, 10f);
            player = playerObject.GetComponent<PlayerController>();
            playerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            yield return null;

            player.StartAttack();
            //1 frame to start the animation
            yield return null;
            //1 frame to run update() to get the correct information
            yield return null;

            Assert.IsFalse(player.IsCurrentAnimaion("RangedAttack"));
        }



        //Seems to be hard to do with Nsub, using humble object instead
        [UnityTest]
        public IEnumerator PlayerAttGolemInGame()
        {
            yield return null;

        }



        [UnityTest]
        public IEnumerator _11PlayerSpeedLevelIncreasesOnPickingUpBoots()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            var originalSpeedLevel = player.speedLevel;
            var boots = Spawner.SpawnBoots();
            player.Pickup(boots);

            yield return null;
            Assert.AreEqual(originalSpeedLevel + 1, player.speedLevel);

            Object.Destroy(boots);
        }

        [UnityTest]
        public IEnumerator _12PlayerMoveFasterWithSpeedLevel()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();

            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(1);
            player.UnityService = unityService;

            yield return null;

            var originalSpeed = player.XSpeedCheck();
            player.speedLevel += 1;

            yield return null;
            var newSpeed = player.XSpeedCheck();
            Assert.AreEqual(originalSpeed + player.speedPerLevel, newSpeed);

        }

        [UnityTest]
        public IEnumerator _13BootsDisappearingAfterPickup()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            var boots = Spawner.SpawnBoots();
            yield return null;

            player.Pickup(boots);

            yield return null;
            Assert.IsTrue(boots == null);

            Object.Destroy(boots);
        }
        [UnityTest]
        public IEnumerator _14HurtAnimationPlaysWhenTakingDamage()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            player.TakingDamage(1);
            yield return null;
            Assert.IsTrue(player.staggered);

        }

        [UnityTest]
        public IEnumerator _15PlayerLookDiectionLockedWhenStaggered()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;           
            Assert.IsTrue(player.GetLookDirection() == 1);           
            player.staggered = true;

            //this should not affect the lookdirection as player is straggered
            player.ChangeLookDirection(-1);
            Assert.IsTrue(player.GetLookDirection() == 1);

        }
        //testing behaviorial scripts in animations
        [UnityTest]
        public IEnumerator _14PlayerAttackingTrueWhenAttacking()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            player.StartAttack();
            //1 frame for animationt to start
            yield return null;

            Assert.IsTrue(player.attacking);
        }
        [UnityTest]
        public IEnumerator _14PlayerShootingTrueWhenShootingArrows()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            player.ShootArrow();
            //1 frame for animationt to start
            yield return null;

            Assert.IsTrue(player.shooting);
        }

        //inAction is simply attacking || shooting
        [UnityTest]
        public IEnumerator _15PlayerLookDiectionLockedWhenInAction()
        {
            playerObject = Spawner.SpawnPlayer();
            player = playerObject.GetComponent<PlayerController>();
            yield return null;
            

            Assert.IsTrue(player.GetLookDirection() == 1);
            player.inAction = true;
            //this should not affect the lookdirection as player is straggered
            player.ChangeLookDirection(-1);
            Assert.IsTrue(player.GetLookDirection() == 1);

        }
        [UnityTest]
        public IEnumerator _14PlayerFallingAnimationWhenFallingDownInAir()
        {
            playerObject = Spawner.SpawnPlayer(0, 10f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            //1 frame for animationt to start
            yield return null;

            Assert.IsTrue(player.IsCurrentAnimaion("Fall"));
        }
        




        [UnityTest]
        public IEnumerator _20PlayerCastThunderBolt()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            //player.ThunderStrike();

        }
        [UnityTest]
        public IEnumerator _20PlayerCastFireBall()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.3f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            //player.FireBall();

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
        public void DisableGravityForObject(GameObject gameObject)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        [UnityTest]
        public IEnumerator _99PlayerMoveRight()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(1);
            player.UnityService = unityService;
            yield return new WaitForSeconds(1.0f);

            Assert.Greater(player.GetPosition().x, originalX);
        }
        [UnityTest]
        public IEnumerator _99PlayerMoveLeft()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            player = playerObject.GetComponent<PlayerController>();
            yield return null;

            var unityService = Substitute.For<IUnityService>();
            var originalX = player.transform.position.x;
            unityService.GetAxis("Horizontal").Returns(-1);
            player.UnityService = unityService;
            yield return new WaitForSeconds(1.0f);

            Assert.Less(player.GetPosition().x, originalX);
        }
    }

}
