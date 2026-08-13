using Zenject;
using Entities;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services
{
    public class TargetFindService : ITargetFindService
    {
        [Inject] private IBoxManager _boxManager;

        private readonly Queue<TargetFindRequest> _queue = new();

        private bool _isProcessing;

        private Task _processingTask;

        public Task<Box> FindTarget(Tank tank, Vector3 position)
        {
            var request = new TargetFindRequest
            {
                Tank = tank,
                Position = position,
                Completion = new TaskCompletionSource<Box>()
            };

            _queue.Enqueue(request);

            _processingTask ??= ProcessQueue();

            return request.Completion.Task;
        }

        private async Task ProcessQueue()
        {
            try
            {
                while (_queue.Count > 0)
                {
                    var request = _queue.Dequeue();

                    var target = FindTargetInternal(request.Tank, request.Position);

                    request.Completion.SetResult(target);

                    await Task.Yield();
                }
            }
            finally
            {
                _processingTask = null;

                if (_queue.Count > 0)
                    _processingTask = ProcessQueue();
            }
        }

        private Box FindTargetInternal(Tank tank, Vector3 position)
        {
            Box newTarget = null;
            var bestDistance = float.MaxValue;

            foreach (var column in _boxManager.GetColumns())
            {
                foreach (Transform child in column.transform)
                {
                    if (!child.TryGetComponent(out Box box))
                        continue;

                    var hitReceiver = box.GetComponentInChildren<BoxHitReceiver>();

                    if (!hitReceiver.CanReceiveHit)
                        continue;

                    if (hitReceiver.IsReserved)
                        continue;

                    if (box.BoxData.Color != tank.TankData.Color)
                        continue;

                    var distance =
                        (box.transform.position - position).sqrMagnitude;

                    if (distance >= bestDistance)
                        continue;

                    bestDistance = distance;
                    newTarget = box;
                }
            }

            return newTarget;
        }
    }
}