using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.World
{
    public class Object
    {
        #region Variables
        int id;
        bool isUnpassable;

        public bool IsUnpassable
        {
            get { return isUnpassable; }
            set { isUnpassable = value; }
        }
        #endregion

        #region Properties
        public int ID
        {
            get { return id; }
            set {id = value; }
        }
        #endregion
    }
}
