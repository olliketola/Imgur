using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imgur.Model
{
    class SingleImageComments
    {
        public string id{ get; set; }
        public string comment { get; set; }
        public string author { get; set; }
        public string datetime { get; set; }
        public Boolean test { get; set; }

        public SingleImageComments(string id, string comment,string author, string datetime, Boolean test)
        {
            this.id = id;
            this.comment = comment;
            this.author = author;
            this.datetime = datetime;
            this.test = test;

        }
    }
}
