using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICurrencyStoreListener 
{ 
    /// <summary>
    /// Notifies the normal egg count
    /// </summary>
    /// <param name="normalEggCount">Amount of normal eggs</param>
    void NotifyNormalEggCount(int normalEggCount);

    /// <summary>
    /// Notifies the golden egg count
    /// </summary>
    /// <param name="goldenEggCount">Amount of golde eggs</param>
    void NotifyGoldenEggCount(int goldenEggCount);
}
