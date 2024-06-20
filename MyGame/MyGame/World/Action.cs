using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.World
{
    public class Action
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Action(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
