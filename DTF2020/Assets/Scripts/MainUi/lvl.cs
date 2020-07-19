using System;
using System.Threading;

namespace DefaultNamespace.UI
{
    [Serializable]
    public struct lvl
    {
        public lvl(int countStars, bool lockState)
        {
            this.countStars = countStars;
            this.lockState = lockState;
        }
        
        public int countStars;

        public bool lockState;
    }
}