﻿using Newtonsoft.Json.Linq;
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
        public JArray arr { get; set; }
        


        public SingleImageComments(string id, string comment,string author, string datetime,JArray arr)
        {
            this.id = id;
            this.comment = comment;
            this.author = author;
            this.datetime = datetime;
            this.arr = arr;

        }
    }
}
