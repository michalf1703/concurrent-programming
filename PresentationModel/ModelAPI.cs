using System;
using System.Collections.Generic;
using System.Text;
using Logic;

namespace Model
{
    public abstract class ModelAbstractApi
    {
        public abstract List<BallModel> balls { get; }
        //GenerateBalls takes the number of balls
        public abstract void GenerateBalls(int count);
        //The "CreateApi" method in the ModelAbstractApi class returns a new ModelApi object,
        public static ModelAbstractApi CreateApi()
        {
            return new ModelApi();
        }
    }
    //The ModelApi class inherits from the ModelAbstractApi class
    internal class ModelApi : ModelAbstractApi
    {
        //balls returns the result of calling the "ChangeBall" method
        public override List<BallModel> balls => ChangeBall();
        // performing actions related to the logic of balls
        private LogicAbstractApi _logicApi;

        public ModelApi()
        {
            _logicApi = _logicApi ?? LogicAbstractApi.CreateApi();
        }
        //The "ChangeBall" method in the ModelApi class creates a list of BallModel objects, where each Ball object returned
        //by the "GetBalls" method of the LogicAbstractApi class is converted to a BallModel object.
        public List<BallModel> ChangeBall()
        {
            List<BallModel> ballModels = new List<BallModel>();

            foreach (Ball b in _logicApi.GetBalls())
            {
                ballModels.Add(new BallModel(b));
            }
            return ballModels;
        }
        //The "CreateBalls" method in the ModelApi class creates the balls and starts them by calling the "createBalls" and "start"
        //methods of the LogicAbstractApi class.
        public override void GenerateBalls(int count)
        {
            _logicApi.createBalls(count);
            _logicApi.start();
        }
    }
}