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

    public class NoSetUpTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GolemTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GolemTracingPlayer()
        {
            var player = Spawner.SpawnPlayer(4, 1);
            var golem = Spawner.SpawnGolem(-4, 1);
            Spawner.SetUpGround();

            yield return new WaitForSeconds(5.0f);

            Assert.Less(Vector2.Distance(golem.transform.position, player.transform.position), 3);
            Object.Destroy(player);
            Object.Destroy(golem);
        }
        [UnityTest]
        public IEnumerator GolemLookingLeftAtPlayer()
        {
            
            var golem = Spawner.SpawnGolem(-4, 1);
            var golemController = golem.GetComponent<GolemController>();
            var player = Spawner.SpawnPlayer(4, 1);
            //golem looks right, so the localScale should be 1
            Assert.AreEqual(1, golemController.GetComponent<Transform>().localScale);

            yield return new WaitForSeconds(2.0f);

            //golem looks right, so the localScale should be 1
            Assert.AreEqual(-1, golemController.GetComponent<Transform>().localScale);

            Object.Destroy(player);
            Object.Destroy(golem);

        }

        [UnityTest]
        public IEnumerator xr()
        {
            var player = Spawner.SpawnPlayer(4, 1);
            var playerController = player.GetComponent<PlayerController>();
            yield return null;
        }
    }
}
