using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ProjectileTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ProjectileTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ArrowProduced()
        {
            var player = Spawner.SpawnPlayer();
            var playerController = player.GetComponent<PlayerController>();

            //a frame needed to run start()
            yield return null;

            playerController.ShootArrow();

            yield return new WaitForSeconds(2.0f);
            var arrow = GameObject.FindGameObjectWithTag("Arrow");

            Assert.IsNotNull(arrow);

        }
        [UnityTest]
        public IEnumerator ArrowFlyWithSpeed()
        {
            var player = Spawner.SpawnPlayer();
            var playerController = player.GetComponent<PlayerController>();

            //a frame needed to run start()
            yield return null;

            playerController.ShootArrow();

            yield return new WaitForSeconds(2.0f);
            var originalX = GameObject.FindGameObjectWithTag("Arrow").transform.position.x;

            yield return new WaitForSeconds(2.0f);
            var NewX = GameObject.FindGameObjectWithTag("Arrow").transform.position.x; ;

            //PlayerPrefab faces the right side, therefore X should increase
            Assert.Greater(NewX,originalX);

        }
    }
}
