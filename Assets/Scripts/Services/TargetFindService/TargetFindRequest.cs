using System.Threading.Tasks;
using Entities;
using UnityEngine;

namespace Services
{
    internal class TargetFindRequest
    {
        public Tank Tank;
        public Vector3 Position;
        public TaskCompletionSource<Box> Completion;
    }
}