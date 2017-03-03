using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnJordanFinci
{
    public class TrainStop
    {
        #region properties

        public char Name;

        public Dictionary<TrainStop, int> AdjacentStops;

        #endregion

        #region constructor

        public TrainStop(char name)
        {
            this.AdjacentStops = new Dictionary<TrainStop, int>();
            this.Name = name;
        }

        #endregion

        #region methods

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("City ");
            output.Append(this.Name);
            output.Append(":\n");
            output.Append(this.AdjacentStops.ToString());
            output.Append("\n");
            return output.ToString();
        }

        #endregion
    }
}
