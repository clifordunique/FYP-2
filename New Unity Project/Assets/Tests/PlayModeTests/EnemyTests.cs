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

/// Values for health,speed,damage will be set to the values set in prefabs unless initialized otherwise in the tests. The default values are as follows:
/// ogreHealth = 10
/// ogreSpeed = 1
/// ogreDamage = 1
/// ogreType = 0
/// ogreRange = 1.5

namespace Tests
{

    public class EnemyTests
    {
        private GameObject testStage;
        private GameObject playerObject;
        private GameObject ogreObject;
        private GameObject necromancerObject;

        private PlayerController player;

        //for testing enemy functions (not exclusive to melee/ranged)
        private Enemy enemy;
        //ogre is a MELEE enemy and uses the script meleeEnemyController, which all melee enemies use. It is used for testing meleeEnemyController.
        private MeleeEnemyController ogre;
        //necromancer is a RANGED enemy and uses the script RangedEnemyController...ditto
        private RangedEnemyController necromancer;

        [SetUp]
        public void Setup()
        {
            testStage = Spawner.SetUpTestStage(0, 0);
        }
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerObject);         
            Object.Destroy(ogreObject);
            Object.Destroy(testStage);
            Object.Destroy(necromancerObject);
        }

        // A UnityTest behaves like a coroutine in Play Mode.
        // `yield return null;` to skip a frame.

        [UnityTest]
        public IEnumerator _01TakingDamage()
        {
            ogreObject = Spawner.SpawnOgre();
            enemy = ogreObject.GetComponent<Enemy>();

            //1 frame needs to run so start() would run, which would initialize things such as variables or components(e.g. animator)
            yield return null;

            var originalHealth = ogre.health;
            enemy.TakeDamage(1);
            var newHealth = ogre.health;
            Assert.AreEqual(originalHealth - 1, newHealth);
        }

        //All enemy should have some mechanisms to damage the player, presumably by detecting the player's collider
        [UnityTest]
        public IEnumerator _02DamagePlayer()
        {
            //added after UI was made as a UI instance was needed for the test to function
            playerObject = Spawner.SpawnPlayer();
            ogreObject = Spawner.SpawnOgre();

            player = playerObject.GetComponent<PlayerController>();
            enemy = ogreObject.GetComponent<Enemy>();
            
            //1 frame for start()
            yield return null;
            var originalPlayerHealth = player.health;

            enemy.DamagePlayer(playerObject.GetComponent<BoxCollider2D>());
            var newPlayerHealth = player.health;
            Assert.AreEqual(originalPlayerHealth - 1, newPlayerHealth);
        }

        //Everything dies... An enemy should 1.plays a death animation when it dies 
        //                                   2. disappear from the scene a few seconds after
        [UnityTest]
        public IEnumerator _03EnemyDeath()
        {
            //Time.timeScale = 20.0f;

            ogreObject = Spawner.SpawnOgre();
            ogre = ogreObject.GetComponent<MeleeEnemyController>();

            //1 frame for start()
            yield return null;

            ogre.OnDeath();

            //A little time is needed for the death animation to play, the object should be destoryed 5 seconds after it dies,
            yield return new WaitForSeconds(5.1f);
            Assert.IsTrue(ogreObject == null);
        }
        //add test when health = 0 see if can do something betetr than just if statement ondeath()

        //Testing the LookAtPlayer() function in Update() of Enemy class
        [UnityTest]
        public IEnumerator _04LookingAtPlayer()
        {
            playerObject = Spawner.SpawnPlayer(-4, 1);
            ogreObject = Spawner.SpawnOgre(0, 1);
            player = playerObject.GetComponent<PlayerController>();
            ogre = ogreObject.GetComponent<MeleeEnemyController>();

            //OnSpawn, the enemy faces the default side, which is right as intended by the sprites
            Assert.AreEqual(1, ogre.GetLookDirection());
            Debug.Log("first assert passed: Local Scale is 1 on spawn");

            //the ogreObject should look to the left at the playerObject, as update() ran once
            yield return null;

            var ogreXpos = ogre.GetPosition().x;
            var playerXpos = player.GetPosition().x;
          
            //Confirming that ogreObject is to the left of the playerObject
            Assert.Greater(ogreXpos, playerXpos);
            Assert.AreEqual(-1, ogre.GetLookDirection());

        }
        //Enemies should move to players up til a certain range depending on the enemy type, ogre has a range same as Golem in the game (melee range)
        //in Update()
        //FUNCTION TESTED: MoveTowardsPlayers() in GroundEnemy.c
        //The enemy should move towards the player steadily
        [UnityTest]
        public IEnumerator _05MeleeMoveTowardsPlayerAtCorrectSpeed()
        {
            playerObject = Spawner.SpawnPlayer(24, 0.5f);
            ogreObject = Spawner.SpawnOgre(0, 0.5f);
            ogre = ogreObject.GetComponent<MeleeEnemyController>();

            var originalOgreXPos = ogre.GetPosition().x;
            Assert.AreEqual(originalOgreXPos, 0);
            Debug.Log("First assert passed");

            yield return new WaitForSeconds(1.0f);  //It should move towards (24,?) where the player is

            var newOgreXPos = ogreObject.transform.position.x;

            //speed is in unit per second
            var expectedXPos = originalOgreXPos + ogre.speed;
            //within 0.1 variance is accurate enough
            Debug.Log(newOgreXPos);
            Assert.IsTrue(newOgreXPos > expectedXPos - 0.1 && newOgreXPos < expectedXPos + 0.1);    //within expected speed
        }

        //The enemy should stop moving when within range of the player
        //This further tests and expands the functionality of MoveTowardsPlayersGrounded()
        [UnityTest]
        public IEnumerator _06StopMovingWhenInRange()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            ogreObject = Spawner.SpawnOgre(2, 0.5f);
            ogre = ogreObject.GetComponent<MeleeEnemyController>();

            yield return new WaitForSeconds(2.0f);
            var ogreXPos = ogre.GetPosition().x;
            Debug.Log(ogreXPos);
            Assert.IsTrue(ogreXPos > ogre.baseRange - 0.01 && ogreXPos < ogre.baseRange + 0.01);     //more confirmation, it should be extremely accurate to the default value of 1.5f

            yield return new WaitForSeconds(0.5f);
            var newogreXPos = ogreObject.transform.position.x;

            Assert.IsTrue(Mathf.Abs(newogreXPos - ogreXPos) < Mathf.Epsilon);      //Mathf.Epsilon is a very small number (close to 0)
        }
        //CheckForPlayerInFront checks if the player is in the monster's range
        [UnityTest]
        public IEnumerator _07MeleeCheckForPlayerInFront()
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            ogreObject = Spawner.SpawnOgre(-0.5f, 0.5f);
            ogre = ogreObject.GetComponent<MeleeEnemyController>();
           
            yield return null;
            Assert.AreEqual(ogre.GetLookDirection(), 1);
            Assert.AreEqual(ogre.CheckForPlayerInFront(), playerObject.GetComponent<EdgeCollider2D>());

        }
        [UnityTest]
        public IEnumerator _08RangedMoveTowardsFromPlayerAtCorrectSpeed()
        {
            playerObject = Spawner.SpawnPlayer(20, 0.5f);
            necromancerObject = Spawner.SpawnNecromancer(0, 0.5f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();

            var originalNecromancerXPos = necromancer.GetPosition().x;
            Assert.AreEqual(originalNecromancerXPos, 0);
            Debug.Log("First assert passed");

            yield return new WaitForSeconds(1.0f);  //It should move towards (24,?) where the player is

            var newNecromancerXPos = necromancer.GetPosition().x;

            Debug.Log(newNecromancerXPos);
            
            var expectedXPos = originalNecromancerXPos + necromancer.baseSpeed;  //unit per second is used for speed, so no multiplier needed for baseSpeed
            Assert.IsTrue(newNecromancerXPos > expectedXPos - 0.1 && newNecromancerXPos < expectedXPos + 0.1);    //speed should be slower than the original speed so the plaeyr can ctach up more easily
        }

        //Testing MoveAwayFromPlayer() in groundenemy.c
        [UnityTest]
        public IEnumerator _09RangedMoveAwayFromPlayerAtCorrectSpeed()
        {
            playerObject = Spawner.SpawnPlayer(-1, 0.5f);
            necromancerObject = Spawner.SpawnNecromancer(0, 0.5f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();

            var originalNecromancerXPos = necromancer.GetPosition().x;
            Assert.AreEqual(originalNecromancerXPos, 0);
            Debug.Log("First assert passed");

            yield return new WaitForSeconds(1.0f);  //It should move towards (24,?) where the player is

            var newNecromancerXPos = necromancer.GetPosition().x;

            Debug.Log(newNecromancerXPos);
            var expectedXPos = originalNecromancerXPos + necromancer.baseSpeed * necromancer.backwardsSpeedMod;
            Assert.IsTrue(newNecromancerXPos > expectedXPos - 0.1 && newNecromancerXPos < expectedXPos + 0.1);    //speed should be slower than the original speed so the plaeyr can ctach up more easily
        }
        [UnityTest]
        public IEnumerator _10RangedEnemyShootProjectile()
        {
            necromancerObject = Spawner.SpawnNecromancer(0, 0.5f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();
            yield return null;
            necromancer.FireProjectile();
            yield return null;
            var necromancerProjectile = GameObject.Find("NecromancerProjectile(Clone)");
            Assert.IsNotNull(necromancerProjectile);
        }
        //1 more test for localscale
        [UnityTest]
        public IEnumerator _11RangedEnemyProjectileHasCorrectLocalScaleRight()
        {
            necromancerObject = Spawner.SpawnNecromancer(0, 0.3f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();
            yield return null;
            necromancer.FireProjectile();
            yield return null;
            var necromancerProjectile = GameObject.Find("NecromancerProjectile(Clone)");
            Assert.AreEqual(necromancerProjectile.transform.localScale.x, necromancerObject.transform.localScale.x);
        }
        [UnityTest]
        public IEnumerator _12RangedEnemyProjectileHasCorrectLocalScaleLeft()
        {
            necromancerObject = Spawner.SpawnNecromancer(0, 0.3f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();
            yield return null;
            necromancerObject.transform.localScale = new Vector3(-1, necromancer.transform.localScale.y, necromancer.transform.localScale.z);
            necromancer.FireProjectile();
            yield return null;
            var necromancerProjectile = GameObject.Find("NecromancerProjectile(Clone)");
            Assert.AreEqual(necromancerProjectile.transform.localScale.x, necromancerObject.transform.localScale.x);
        }

        //The projectiles should disappear when they are far from the scene, so they dont slow down the game when there are alot
        [UnityTest]
        public IEnumerator _13RangedEnemyProjectileDisappearsWhenFar()
        {
            necromancerObject = Spawner.SpawnNecromancer(0, 0.5f);
            necromancer = necromancerObject.GetComponent<RangedEnemyController>();
            yield return null;
            necromancer.FireProjectile();
            yield return null;
            var necromancerProjectile = GameObject.Find("NecromancerProjectile(Clone)");
            Assert.IsNotNull(necromancerProjectile);

            necromancerProjectile.transform.position = new Vector3(101, 0, 0);
            yield return null;                                              //for update() to run
            Assert.IsTrue(necromancerProjectile == null);
        }

        [UnityTest]
        public IEnumerator _13EnemiesCanDropChest()
        {
            ogreObject = Spawner.SpawnOgre(0, 0.5f);
            var enemy = ogreObject.GetComponent<Enemy>();
            yield return null;
            enemy.dropRate = 1;
            enemy.Drop();
            yield return null;
            var loot = GameObject.FindGameObjectWithTag("Chest");

            Assert.IsNotNull(loot);
        }



        ///When within range, the enemy should attack the player. After some consideration, I decided that it would be more sensible
        ///to expand the TracePlayerGround to include attack() when in range, this tests the Attack() in it.
            //DamagerPlayer() is tested on EnemyTest.cs

        [UnityTest]
        public IEnumerator AttackWhenInRange()  //attack animation shud make it stop moving
        {
            playerObject = Spawner.SpawnPlayer(0, 0.5f);
            ogreObject = Spawner.SpawnOgre(0.49f, 0.5f);

            ogre = ogreObject.GetComponent<MeleeEnemyController>();
            player = playerObject.GetComponent<PlayerController>();

            DisableGravityForObject(playerObject);
            DisableGravityForObject(ogreObject);

            yield return new WaitForSeconds(1.0f);

            Assert.AreEqual(true, ogre.attacking);
        }


        //[UnityTest]
        //public IEnumerator nograv()
        //{
        //    playerObject = Spawner.SpawnPlayer(0, 10);
        //    DisableGravityForObject(playerObject);

        //    yield return new WaitForSeconds(2.0f);

        //}

        //shared among both types
        [UnityTest]
        public IEnumerator _108StaggeredWhenHit()
        {
            ogreObject = Spawner.SpawnOgre();
            ogre = ogreObject.GetComponent<MeleeEnemyController>();
            ogre.canBeStaggered = true;         //ogre should be staggerable by default when instantiated, but it is set to true again for clarity

            yield return null;

            ogre.TakeDamage(1);
            yield return null;
            Assert.AreEqual(true, ogre.staggered);

        }
        [UnityTest]
        public IEnumerator _109CannotBeStaggeredWhenHit()
        {
            ogreObject = Spawner.SpawnOgre();
            ogre = ogreObject.GetComponent<MeleeEnemyController>();
            ogre.canBeStaggered = false;         //ogre should be staggerable by default when instantiated, it is set to false for testing

            yield return null;

            ogre.TakeDamage(1);
            yield return null;
            //should not be staggered despite taking damage
            Assert.AreEqual(false, ogre.staggered);

        }

        public void DisableGravityForObject(GameObject gameObject)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}


//playerObject = Spawner.SpawnPlayer();
//player = playerObject.GetComponent<PlayerController>();
//ogreObject = Spawner.SpawnOgre();
//ogre = ogreObject.GetComponent<MeleeEnemyController>();



//Object.Destroy(playerObject);
//Object.Destroy(ogre);
//Object.Destroy(testStage);  
