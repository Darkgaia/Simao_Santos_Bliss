using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlissApp.Model
{
    public class Choice
    {
        public string choice;
        public int votes;

        public Choice(string _choice, int _votes)
        {
            this.choice = _choice;
            this.votes = _votes;
        }
    }

}
