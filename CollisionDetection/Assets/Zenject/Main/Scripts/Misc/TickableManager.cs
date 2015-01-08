using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class TickableManager
    {
        [Inject]
        [InjectOptional]
        readonly List<ITickable> _tickables = null;

        [Inject]
        [InjectOptional]
        readonly List<IFixedTickable> _fixedTickables = null;

        [Inject]
        [InjectOptional]
        readonly List<ILateTickable> _lateTickables = null;

        [Inject]
        [InjectOptional]
        readonly List<Tuple<Type, int>> _priorities = null;

        [Inject("Fixed")]
        [InjectOptional]
        readonly List<Tuple<Type, int>> _fixedPriorities = null;

        [Inject("Late")]
        [InjectOptional]
        readonly List<Tuple<Type, int>> _latePriorities = null;

        TaskUpdater<ITickable> _updater;
        TaskUpdater<IFixedTickable> _fixedUpdater;
        TaskUpdater<ILateTickable> _lateUpdater;

        [PostInject]
        public void Initialize()
        {
            InitTickables();
            InitFixedTickables();
            InitLateTickables();
        }

        void InitFixedTickables()
        {
            _fixedUpdater = new TaskUpdater<IFixedTickable>(UpdateFixedTickable);

            foreach (var type in _fixedPriorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<IFixedTickable>(),
                    "Expected type '{0}' to drive from IFixedTickable while checking priorities in TickableHandler", type.Name());
            }

            var priorityMap = _fixedPriorities.ToDictionary(x => x.First, x => x.Second);

            foreach (var tickable in _fixedTickables)
            {
                int priority;

                if (priorityMap.TryGetValue(tickable.GetType(), out priority))
                {
                    _fixedUpdater.AddTask(tickable, priority);
                }
                else
                {
                    _fixedUpdater.AddTask(tickable);
                }
            }
        }

        void InitTickables()
        {
            _updater = new TaskUpdater<ITickable>(UpdateTickable);

            foreach (var type in _priorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<ITickable>(),
                    "Expected type '{0}' to drive from ITickable while checking priorities in TickableHandler", type.Name());
            }

            var priorityMap = _priorities.ToDictionary(x => x.First, x => x.Second);

            foreach (var tickable in _tickables)
            {
                int priority;

                if (priorityMap.TryGetValue(tickable.GetType(), out priority))
                {
                    _updater.AddTask(tickable, priority);
                }
                else
                {
                    _updater.AddTask(tickable);
                }
            }
        }

        void InitLateTickables()
        {
            _lateUpdater = new TaskUpdater<ILateTickable>(UpdateLateTickable);

            foreach (var type in _latePriorities.Select(x => x.First))
            {
                Assert.That(type.DerivesFrom<ILateTickable>(),
                    "Expected type '{0}' to drive from ILateTickable while checking priorities in TickableHandler", type.Name());
            }

            var priorityMap = _latePriorities.ToDictionary(x => x.First, x => x.Second);

            foreach (var tickable in _lateTickables)
            {
                int priority;

                if (priorityMap.TryGetValue(tickable.GetType(), out priority))
                {
                    _lateUpdater.AddTask(tickable, priority);
                }
                else
                {
                    _lateUpdater.AddTask(tickable);
                }
            }
        }

        void UpdateLateTickable(ILateTickable tickable)
        {
            using (ProfileBlock.Start("{0}.LateTick()".With(tickable.GetType().Name())))
            {
                tickable.LateTick();
            }
        }

        void UpdateFixedTickable(IFixedTickable tickable)
        {
            using (ProfileBlock.Start("{0}.FixedTick()".With(tickable.GetType().Name())))
            {
                tickable.FixedTick();
            }
        }

        void UpdateTickable(ITickable tickable)
        {
            using (ProfileBlock.Start("{0}.Tick()".With(tickable.GetType().Name())))
            {
                tickable.Tick();
            }
        }

        public void Add(ITickable tickable)
        {
            _updater.AddTask(tickable);
        }

        public void Add(ITickable tickable, int priority)
        {
            _updater.AddTask(tickable, priority);
        }

        public void AddLate(ILateTickable tickable)
        {
            _lateUpdater.AddTask(tickable);
        }

        public void AddFixed(IFixedTickable tickable)
        {
            _fixedUpdater.AddTask(tickable);
        }

        public void AddFixed(IFixedTickable tickable, int priority)
        {
            _fixedUpdater.AddTask(tickable, priority);
        }

        public void Remove(ITickable tickable)
        {
            _updater.RemoveTask(tickable);
        }

        public void RemoveLate(ILateTickable tickable)
        {
            _lateUpdater.RemoveTask(tickable);
        }

        public void RemoveFixed(IFixedTickable tickable)
        {
            _fixedUpdater.RemoveTask(tickable);
        }

        public void Update()
        {
            _updater.OnFrameStart();
            _updater.UpdateAll();
        }

        public void FixedUpdate()
        {
            _fixedUpdater.OnFrameStart();
            _fixedUpdater.UpdateAll();
        }

        public void LateUpdate()
        {
            _lateUpdater.OnFrameStart();
            _lateUpdater.UpdateAll();
        }
    }
}
