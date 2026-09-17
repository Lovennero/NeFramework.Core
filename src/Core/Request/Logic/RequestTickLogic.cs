using System;

namespace Framework.Core.Request
{
    internal sealed class RequestTickLogic : IRequestTickLogic
    {
        private readonly RequestModel _model;
        private readonly RequestTickModel _tickModel;
        
        public RequestTickLogic(RequestModel model, RequestTickModel tickModel)
        {
            _model = model;
            _tickModel = tickModel;
        }

        public void Tick(float logicTime, float realTime)
        {
            Capture();
            
            TickRetrierRegular(logicTime, realTime);
            TickWaiterRegular();
            TickRunnerRegular();
        }

        private void Capture()
        {
            var groupSort = _model.GroupsSort;
            var groupCount = groupSort.Count;

            if (_tickModel.Groups.Length < groupCount)
            {
                _tickModel.Groups = new RequestGroupRecord[groupCount];
                
                _tickModel.GroupRunners = new RequestRecord[groupCount][];
                _tickModel.GroupRunnersCount = new int[groupCount];
                
                _tickModel.GroupRetriers = new RequestRecord[groupCount][];
                _tickModel.GroupRetriersCount = new int[groupCount];
            }
            
            groupSort.CopyTo(_tickModel.Groups,0);
            _tickModel.GroupsCount = groupCount;
            

            for (var i = 0; i < _tickModel.GroupsCount; i++)
            {
                var group = _tickModel.Groups[i];
                
                var groupRunners = _model.GroupsRunners[group.GroupKey];
                var groupRetriers = _model.GroupsRetriers[group.GroupKey];
                
                var groupRunnersCount = groupRunners.Count;
                var groupRetriersCount = groupRetriers.Count;
                
                if (_tickModel.GroupRunners[i] == null || _tickModel.GroupRunners[i].Length < groupRunnersCount)
                    _tickModel.GroupRunners[i] = new RequestRecord[Math.Max(4, groupRunnersCount)];
                if(_tickModel.GroupRetriers[i] == null || _tickModel.GroupRetriers[i].Length < groupRetriersCount)
                    _tickModel.GroupRetriers[i] = new RequestRecord[Math.Max(4, groupRetriersCount)];
                
                _tickModel.GroupRunnersCount[i] = groupRunnersCount;
                _tickModel.GroupRetriersCount[i] = groupRetriersCount;
                
                groupRunners.CopyTo(_tickModel.GroupRunners[i], 0);
                groupRetriers.CopyTo(_tickModel.GroupRetriers[i], 0);
            }
            
        }

        private void TickRetrierRegular(float logicTime, float realTime)
        {
            
            for (var i = 0; i < _tickModel.GroupsCount; i++)
            {
                var group = _tickModel.Groups[i];
                
                var realGroupRetriers = _model.GroupsRetriers[group.GroupKey];
                var realGroupWaiters = _model.GroupsWaiters[group.GroupKey];
                
                var groupRetriers = _tickModel.GroupRetriers[i];
                var groupRetriersCount = _tickModel.GroupRetriersCount[i];
                for (var j = 0; j < groupRetriersCount; j++)
                {
                    var record =  groupRetriers[j];
                    record.Request.Tick(logicTime, realTime);
                    if (record.Request.State == RequestState.Waiting)
                    {
                        realGroupRetriers.Remove(record);
                        realGroupWaiters.Add(record);
                    }
                    
                }
            }
        }
        
        private void TickWaiterRegular()
        {
            for (var i = 0; i < _tickModel.GroupsCount; i++)
            {
                var group = _tickModel.Groups[i];
                while (group.Concurrent < group.GroupConfig.MaxConcurrent)
                {
                    var groupsWaiter = _model.GroupsWaiters[group.GroupKey];
                    var groupsRuner =  _model.GroupsRunners[group.GroupKey];
                    if(groupsWaiter.Count <= 0) break;

                    var waiter = groupsWaiter.Min;
                    waiter.Request.Start();
                    group.Concurrent++;
                    groupsWaiter.Remove(waiter);
                    groupsRuner.Add(waiter);
                }
            }
        }
        
        private void TickRunnerRegular()
        {
            for (var i = 0; i < _tickModel.GroupsCount; i++)
            {
                var group = _tickModel.Groups[i];
                
                var realGroupRunners = _model.GroupsRunners[group.GroupKey];
                var realGroupRetriers = _model.GroupsRetriers[group.GroupKey];
                
                var groupRunners = _tickModel.GroupRunners[i];
                var runnerCount = _tickModel.GroupRunnersCount[i];
                
                for (var j = 0; j < runnerCount; j++)
                {
                    var record = groupRunners[j];
                    
                    if (record.Request.State == RequestState.Completed)
                    {
                        realGroupRunners.Remove(record);
                        group.Concurrent--;
                    }
                    
                    if (record.Request.State == RequestState.Retry)
                    {
                        realGroupRetriers.Add(record);
                        realGroupRunners.Remove(record);
                        group.Concurrent--;
                    }

                }
            }
        }
    }
}