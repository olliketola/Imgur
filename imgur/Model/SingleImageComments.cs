using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imgur.Model
{
    class SingleImageComments
    {

        public string comment { get; set; }
        public string author { get; set; }
        public string datetime { get; set; }


        public SingleImageComments(string comment,string author, string datetime)
        {
            this.comment = comment;
            this.author = author;
            this.datetime = datetime;

        }
    }
}
