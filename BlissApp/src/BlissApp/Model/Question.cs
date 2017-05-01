using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlissApp.Model
{
    public class Question
    {
        public int id;
        public string question;
        public string image_url;
        public string thumb_url;
        public string datetime;
        public List<Choice> choices;

        public Question(int _id, string _question, string _image_url, string _thumb_url, string _datetime, List<Choice> _choices)
        {
            this.id = _id;
            this.question = _question;
            this.image_url = _image_url;
            this.thumb_url = _thumb_url;
            this.datetime = _datetime;
            this.choices = _choices;
        }

        public Question() { }
       
    }
}
