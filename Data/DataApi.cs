using System;


namespace Data
{
    public abstract class DataAbstractAPI
    {
        //The static method "CreateAPI" of the "DataAbstractAPI" class returns a new instance of the "DataAPI" class.
        public static DataAbstractAPI CreateAPI()
        {
            return new DataAPI();
        }
    }
    // an internal Data API class that inherits from the Data Abstract Api
    internal class DataAPI : DataAbstractAPI
    {

    }
}
