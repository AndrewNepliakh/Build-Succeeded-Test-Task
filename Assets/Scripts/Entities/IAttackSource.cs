using System;
using UnityEngine;

namespace Entities
{
    public interface IAttackSource
    {
        GameObject GameObject { get; }
        
        event Action OnShootEvent;
        event Action OnDisableEvent;
    }
}