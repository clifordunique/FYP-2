using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

namespace Tests
{
    public class ChestTest
    {
        //QQ should collision be done artificially
        [UnityTest]
        public IEnumerator ChestPrefabSpawn()
        {
            var chestTestObject = Spawner.SpawnChest();
            yield return null;
            Assert.IsTrue(chestTestObject != null);

            Object.Destroy(chestTestObject);
        }

        [UnityTest]
        public IEnumerator ChestClosedByDefault()
        {
            var chestTestObject = Spawner.SpawnChest();
            var chest = chestTestObject.GetComponent<ChestScript>();
            yield return null;
            Assert.IsFalse(chest.isOpen);

            Object.Destroy(chestTestObject);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ChestOpeningOnCollisionWithPlayer()
        {
            Spawner.SetUpGround();
            var chestTestObject = Spawner.SpawnChest(0,0.5f);
            var chest = chestTestObject.GetComponent<ChestScript>();

            var player = Spawner.SpawnPlayer(0, 2.0f);
            
            //player should fall onto and collide with the chest
            yield return new WaitForSeconds(2.0f);
            Assert.IsTrue(chest.isOpen);

            Object.Destroy(chestTestObject);

        }
        [UnityTest]
        public IEnumerator ChestOpeningOnceOnlyOnCollisionWithPlayer()
        {
            //If the player keep colliding witht he open chest, no additional item will come out
            Spawner.SetUpGround();
            var chestTestObject = Spawner.SpawnChest(0, 0.5f);
            var chest = chestTestObject.GetComponent<ChestScript>();

            

            var player = Spawner.SpawnPlayer(-5.0f, 2.0f);
            
            chest.OnTriggerEnter2D(player.GetComponent<BoxCollider2D>());
            yield return new WaitForSeconds(0.5f);
            chest.OnTriggerEnter2D(player.GetComponent<BoxCollider2D>());

            
            yield return new WaitForSeconds(2.0f);

            var itemsSpawned = GameObject.FindGameObjectsWithTag("Boots");
            Assert.AreEqual(itemsSpawned.Length, 1);

            Object.Destroy(chestTestObject);

        }
        [UnityTest]
        public IEnumerator ChestNotOpeningOnCollisionWithNonPlayer()
        {
            Spawner.SetUpGround();
            var chestTestObject = Spawner.SpawnChest(0, 0.5f);
            var chest = chestTestObject.GetComponent<ChestScript>();

            var golem = Spawner.SpawnGolem(0, 0.6f);

            //QQ what should I use is golem not good, a  placeholder?
            yield return new WaitForSeconds(1.0f);
            Assert.IsTrue(chest.isOpen);
            yield return null;
        }
        [UnityTest]
        public IEnumerator ChestDisappearing()
        {
            Spawner.SetUpGround();
            var chestTestObject = Spawner.SpawnChest(0, 0.5f);
            var chest = chestTestObject.GetComponent<ChestScript>();

            chest.WhenOpened();
            yield return new WaitForSeconds(4.0f);
            Assert.IsTrue(chest==null);

            Object.Destroy(chestTestObject);
        }
        [UnityTest]
        public IEnumerator ChestItemAppearing()
        {
            Spawner.SetUpGround();
            var chestTestObject = Spawner.SpawnChest(0, 0.5f);
            var chest = chestTestObject.GetComponent<ChestScript>();

            
            chest.WhenOpened();
            yield return new WaitForSeconds(4.0f);

            var itemsSpawned = TestUtils.FindListOf("boots");
            Assert.AreEqual(itemsSpawned.Length, 1);
            
            //QQ how to check for RNG things(shooting items around)
            yield return null;
            Object.Destroy(chestTestObject);
        }


    }
    //item doesnt keep popping out
    //item doesnt get pickup immediately
}
//QQ test mterics how to know beforehand if something inutitive can be used or need to add others
//things relying on futuer like how many items list, shud work on the tiems first?