﻿using System;
using System.Collections.Generic;
using Data;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract void createBalls(int count);
        public abstract List<Ball> GetBalls();

        public abstract void start();
        public static LogicAbstractApi CreateApi(DataAbstractAPI data = default(DataAbstractAPI))
        {
            return new LogicApi(data == null ? DataAbstractAPI.CreateAPI() : data);
        }

    }

    internal class LogicApi : LogicAbstractApi
    {

        private DataAbstractAPI _dataAPI;
        private Task _changePosition;
        private Area _region;

        public LogicApi(DataAbstractAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _region = new Area(500);
        }

        //interactive operation 
        public override void createBalls(int count)
        {
            _region.addBalls(count);
        }

        public override List<Ball> GetBalls()
        {
            return _region.balls;
        }

        public override void start()
        {
            if (_region.balls.Count > 0)
            {
                _changePosition = Task.Run(_region.MoveBalls);
            }
        }
    }
}