using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spawner
{
    //Characters
    public static GameObject SpawnPlayer()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
    }
    public static GameObject SpawnPlayer(float x, float y)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), new Vector3(x, y), Quaternion.identity);
    }
    public static GameObject SpawnGolem()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Golem"));
    }
    public static GameObject SpawnGolem(float x, float y)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Golem"), new Vector3(x, y), Quaternion.identity);
    }

    //Environment
    public static GameObject SpawnChest()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Chest"));
    }
    public static GameObject SpawnChest(float x, float y)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Chest"), new Vector3(x, y), Quaternion.identity);
    }

    //Simple ground on y=0
    public static GameObject SetUpGround()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Grid"));
    }
    public static GameObject SetUpStage1(float x, float y)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Stage/Stage1/Grassland"), new Vector3(x, y), Quaternion.identity);
    }

    //items
    public static GameObject SpawnBoots()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Items/Boots"));
    }
    public static GameObject SpawnSword()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Items/Sword"));
    }
    public static GameObject SpawnBow()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Items/Bow"));
    }
    public static GameObject SpawnScroll()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Items/Scroll"));
    }
    //UI
    public static GameObject SpawnUI()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI"));
    }



    //Testing
    public static GameObject SpawnMockEnemy()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UsedForTesting/MockEnemy"));
    }
    public static GameObject SpawnMockEnemy(float x, float y)
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UsedForTesting/MockEnemy"), new Vector3(x, y), Quaternion.identity);
    }

    public static GameObject SetUpTestStage(float x, float y)
    {
        //The testStage is 50 Units long from (-25,0) to (25,0)
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Stage/Stage1/Grassland"), new Vector3(x, y), Quaternion.identity);
    }

}