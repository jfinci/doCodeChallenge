using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerOnJordanFinci
{
    public class City
    {
        #region properties

        public char name;

        public Dictionary<City, int> routes;

        #endregion

        #region constructor

        public City(char name)
        {
            this.routes = new Dictionary<City, int>();
            this.name = name;
        }

        #endregion

        #region methods

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("City ");
            output.Append(this.name);
            output.Append(":\n");
            output.Append(this.routes.ToString());
            output.Append("\n");
            return output.ToString();
        }

        #endregion
    }
}
