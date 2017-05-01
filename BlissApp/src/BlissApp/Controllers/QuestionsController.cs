using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using BlissApp.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlissApp.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        private static string conString = "Data Source=SIMONSANTOS;Initial Catalog=Blissapp;Integrated Security=True";

        //Retrieve questions base on parameters, then retrieves choices associated with the questions retrieves previously, applying the parameters as well
        private List<Question> Read(int? limit, int? offset, string filter)
        {
            List<Question> res = new List<Question>();
            SqlConnection con = new SqlConnection(conString);
            using (con)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    int offset_count = 0;
                    string oString = "select * from Question";
                    string oString2 = "select * from Choice where question_id=@question_id";

                    SqlCommand oCmd = new SqlCommand(oString, con);
                    con.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if (offset_count >= offset)
                            {
                                if (limit > 0)
                                {
                                    res.Add(new Question(int.Parse(oReader["question_id"].ToString()), oReader["question"].ToString().Trim(), oReader["image_url"].ToString().Trim(), oReader["thumb_url"].ToString().Trim(), oReader["timePosted"].ToString().Trim(), new List<Choice>()));
                                }
                                limit--;
                            }
                            offset_count++;
                        }

                        con.Close();
                    }
                    for (int i = 0; i < res.Count; i++)
                    {
                        oCmd = new SqlCommand(oString2, con);
                        oCmd.Parameters.Add("@question_id", SqlDbType.Int);
                        oCmd.Parameters["@question_id"].Value = res[i].id;
                        con.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                res[i].choices.Add(new Choice(oReader["choice"].ToString().Trim(), int.Parse(oReader["votes"].ToString())));
                            }
                            con.Close();
                        }
                    }
                }
                else
                {
                    int offset_count = 0;
                    string oString = "select * from Question where LOWER(question) LIKE @question";
                    string oString2 = "select * from Choice where question_id=@question_id and LOWER(choice) LIKE @choice";

                    SqlCommand oCmd = new SqlCommand(oString, con);
                    oCmd.Parameters.Add("@question", SqlDbType.VarChar);
                    oCmd.Parameters["@question"].Value = "%" + filter + "%";
                    con.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if (offset_count >= offset)
                            {
                                if (limit > 0)
                                {
                                    res.Add(new Question(int.Parse(oReader["question_id"].ToString()), oReader["question"].ToString().Trim(), oReader["image_url"].ToString().Trim(), oReader["thumb_url"].ToString().Trim(), oReader["timePosted"].ToString().Trim(), new List<Choice>()));
                                }
                                limit--;
                            }
                            offset_count++;
                        }

                        con.Close();
                    }
                    for (int i = 0; i < res.Count; i++)
                    {
                        oCmd = new SqlCommand(oString2, con);
                        oCmd.Parameters.Add("@question_id", SqlDbType.Int);
                        oCmd.Parameters.Add("@choice", SqlDbType.VarChar);
                        oCmd.Parameters["@question_id"].Value = res[i].id;
                        oCmd.Parameters["@choice"].Value = "%" + filter + "%";
                        con.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                res[i].choices.Add(new Choice(oReader["choice"].ToString().Trim(), int.Parse(oReader["votes"].ToString())));
                            }
                            con.Close();
                        }
                    }
                }
            }
            return res;
        }

        [HttpGet]
        public List<Question> Get(int? limit, int? offset, string filter, string question_filter, int? question_id)
        {
            //Are the variables null?
            if (limit.HasValue)
            {
                if (offset.HasValue)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return Read(limit, offset, filter);
                    }
                    else
                    {
                        return Read(limit, offset, null);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return Read(limit, 0, filter);
                    }
                    else
                    {
                        return Read(limit, 0, null);
                    }
                }
            }
            else
            {
                if (offset.HasValue)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return Read(10, offset, filter);
                    }
                    else
                    {
                        return Read(10, offset, null);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        
                        return Read(10, 0, filter);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(question_filter))
                        {
                            if (question_filter != "english")
                            {
                                Response.StatusCode = 400;
                                return null;
                            }
                            else
                            {
                                return Read(10, 0, null);
                            }
                        }
                        else
                        {
                            if (question_id.HasValue)
                            {
                                List<Question> res = new List<Question>();
                                SqlConnection con = new SqlConnection(conString);
                                using (con)
                                {
                                    string oString = "select * from Question where question_id=@id";
                                    string oString2 = "select * from Choice where question_id=@question_id";
                                    SqlCommand oCmd = new SqlCommand(oString, con);
                                    oCmd.Parameters.Add("@id", SqlDbType.Int);
                                    oCmd.Parameters["@id"].Value = question_id;
                                    con.Open();
                                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                                    {
                                        while (oReader.Read())
                                        {
                                            res.Add(new Question(int.Parse(oReader["question_id"].ToString()), oReader["question"].ToString().Trim(), oReader["image_url"].ToString().Trim(), oReader["thumb_url"].ToString().Trim(), oReader["timePosted"].ToString().Trim(), new List<Choice>()));
                                        }
                                    }
                                    con.Close();

                                    oCmd = new SqlCommand(oString2, con);
                                    oCmd.Parameters.Add("@question_id", SqlDbType.Int);
                                    oCmd.Parameters["@question_id"].Value = question_id;
                                    con.Open();
                                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                                    {
                                        while (oReader.Read())
                                        {
                                            res[0].choices.Add(new Choice(oReader["choice"].ToString().Trim(), int.Parse(oReader["votes"].ToString())));
                                        }
                                        con.Close();
                                    }
                                }
                                return res;
                            }
                            else
                            {
                                return Read(10, 0, null);
                            }
                        }
                    }
                }
            }
        }

        [HttpGet("{id}")]
        public Question Get(int id)
        {
            Question res = new Question();
            SqlConnection con = new SqlConnection(conString);
            using (con)
            {
                string oString = "select * from Question where question_id=@id";
                string oString2 = "select * from Choice where question_id=@question_id";
                SqlCommand oCmd = new SqlCommand(oString, con);
                oCmd.Parameters.Add("@id", SqlDbType.Int);
                oCmd.Parameters["@id"].Value = id;
                con.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        res = new Question(int.Parse(oReader["question_id"].ToString()), oReader["question"].ToString().Trim(), oReader["image_url"].ToString().Trim(), oReader["thumb_url"].ToString().Trim(), oReader["timePosted"].ToString().Trim(), new List<Choice>());
                    }
                }
                con.Close();

                oCmd = new SqlCommand(oString2, con);
                oCmd.Parameters.Add("@question_id", SqlDbType.Int);
                oCmd.Parameters["@question_id"].Value = res.id;
                con.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        res.choices.Add(new Choice(oReader["choice"].ToString().Trim(), int.Parse(oReader["votes"].ToString())));
                    }
                    con.Close();
                }
            }
            return res;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Question q)
        {
            string sql = "UPDATE Question SET question=@question, image_url=@image_url, thumb_url=@thumb_url, timePosted=@datetime where question_id=@id";
            string sql2 = "DELETE FROM Choice where question_id=@id";
            string sql3 = "INSERT INTO Choice (question_id,choice,votes) values (@id,@choice,@votes)";
            SqlConnection con = new SqlConnection(conString);
            q.id = id;
            q.datetime = DateTime.Now.ToString();
            SqlCommand oCmd = new SqlCommand(sql, con);
            oCmd.Parameters.Add("@id", SqlDbType.Int);
            oCmd.Parameters["@id"].Value = q.id;
            oCmd.Parameters.Add("@question", SqlDbType.VarChar);
            oCmd.Parameters["@question"].Value = q.question;
            oCmd.Parameters.Add("@image_url", SqlDbType.VarChar);
            oCmd.Parameters["@image_url"].Value = q.image_url;
            oCmd.Parameters.Add("@thumb_url", SqlDbType.VarChar);
            oCmd.Parameters["@thumb_url"].Value = q.thumb_url;
            oCmd.Parameters.Add("@datetime", SqlDbType.DateTime);
            oCmd.Parameters["@datetime"].Value = q.datetime;
            con.Open();
            oCmd.ExecuteNonQuery();
            con.Close();

            
            foreach (Choice c in q.choices)
            {
                oCmd = new SqlCommand(sql2, con);
                oCmd.Parameters.Add("@id", SqlDbType.Int);
                oCmd.Parameters["@id"].Value = q.id;
                con.Open();
                oCmd.ExecuteNonQuery();
                con.Close();
            }

            foreach (Choice c in q.choices)
            {
                oCmd = new SqlCommand(sql3, con);
                oCmd.Parameters.Add("@id", SqlDbType.Int);
                oCmd.Parameters["@id"].Value = q.id;
                oCmd.Parameters.Add("@choice", SqlDbType.VarChar);
                oCmd.Parameters["@choice"].Value = c.choice;
                oCmd.Parameters.Add("@votes", SqlDbType.Int);
                oCmd.Parameters["@votes"].Value = c.votes;
                con.Open();
                oCmd.ExecuteNonQuery();
                con.Close();
            }

            return Json(Get(id));  
        }

        [HttpPost]
        public Question Post([FromBody]Question q)
        {
            string sql = "INSERT INTO Question (question,image_url,thumb_url,timePosted) values (@question,@image_url,@thumb_url,@datetime)";
            string sql2 = "SELECT * from Question";
            string sql3= "INSERT INTO Choice (question_id,choice,votes) values (@id,@choice,@votes)";
            SqlConnection con = new SqlConnection(conString);
            SqlCommand oCmd = new SqlCommand(sql, con);
            oCmd.Parameters.Add("@question", SqlDbType.VarChar);
            oCmd.Parameters["@question"].Value = q.question;
            oCmd.Parameters.Add("@image_url", SqlDbType.VarChar);
            oCmd.Parameters["@image_url"].Value = q.image_url;
            oCmd.Parameters.Add("@thumb_url", SqlDbType.VarChar);
            oCmd.Parameters["@thumb_url"].Value = q.thumb_url;
            oCmd.Parameters.Add("@datetime", SqlDbType.DateTime);
            oCmd.Parameters["@datetime"].Value = DateTime.Now.ToString();
            con.Open();
            oCmd.ExecuteNonQuery();
            con.Close();

            
            List<Question> res = new List<Question>();
            oCmd = new SqlCommand(sql2, con);
            con.Open();
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    res.Add(new Question(int.Parse(oReader["question_id"].ToString()), oReader["question"].ToString().Trim(), oReader["image_url"].ToString().Trim(), oReader["thumb_url"].ToString().Trim(), oReader["timePosted"].ToString().Trim(), new List<Choice>()));
                }
            }
            con.Close();
            int id = res[res.Count - 1].id;

            foreach (Choice c in q.choices)
            {
                oCmd = new SqlCommand(sql3, con);
                oCmd.Parameters.Add("@id", SqlDbType.Int);
                oCmd.Parameters["@id"].Value = id;
                oCmd.Parameters.Add("@choice", SqlDbType.VarChar);
                oCmd.Parameters["@choice"].Value = c.choice;
                oCmd.Parameters.Add("@votes", SqlDbType.Int);
                oCmd.Parameters["@votes"].Value = c.votes;
                con.Open();
                oCmd.ExecuteNonQuery();
                con.Close();
            }

            Response.StatusCode = 201;
            return Get(id);
        }
    }
}
