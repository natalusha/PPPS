using System;
namespace Core.Momento
{
    public class Momento
    {
        public readonly int Id;
        public readonly string Name;
       
        

        public Momento(int id, string name)
        {
            Id = id;
            Name = name.Clone().ToString();
           
           

        }
    }
}
