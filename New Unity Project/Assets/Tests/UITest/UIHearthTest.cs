using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UIHearthTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void UIHearthTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator UIHearthTestWithEnumeratorPasses()
        {
            var player = Spawner.SpawnPlayer().GetComponent<PlayerController>();
            player.health = 9;
            yield return null;          
        }
    }
}
