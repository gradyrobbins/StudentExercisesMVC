using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;



namespace StudentExercisesMVC.Controllers
{
    public class ExercisesController : Controller
    {

        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }




        
        // GET: Exercises
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                          SELECT Id, [Name], [Language] 
                                          FROM Exercise;
                                                    ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("Name")),
                            ExerciseLang = reader.GetString(reader.GetOrdinal("Language"))
                            
                        };

                        exercises.Add(exercise);
                    }

                    reader.Close();

                    return View(exercises);
                }
            }
        }



        // GET: Exercises/Details/5
        
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                          SELECT Id, [Name], [Language] 
                                          FROM Exercise
                                           
                                         
                                            WHERE Id = @id
                                         ;
                ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));



                    SqlDataReader reader = cmd.ExecuteReader();



                    Exercise exercise = null;
                    while (reader.Read())
                    {


                        exercise = new Exercise
                       
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("Name")),
                            ExerciseLang = reader.GetString(reader.GetOrdinal("Language"))

                        };
                       

                    }
                    reader.Close();

                    return View(exercise);
                }
            }
        }

        // GET: Exercises/Create
        // do not need to return a viewmodel- just return the standard view of "exercise" ?  because it doesn't need more information than its own self contained properties/methods
        public ActionResult Create()
        {
            return View();
        }



        

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exercise exercise)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO exercise ([Name], [Language])
                                           VALUES (@ExerciseName, @ExerciseLang)";
                        cmd.Parameters.Add(new SqlParameter("@ExerciseName", exercise.ExerciseName));
                        cmd.Parameters.Add(new SqlParameter("@ExerciseLang", exercise.ExerciseLang));
                        
                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Exercises/Edit/5
        public ActionResult Edit(int id)
        {
            Exercise exercise = GetExerciseById(id);
            if (exercise == null)
            {
                return NotFound();
            }

            

            return View(exercise);
        }


        //sample SQL
        // UPDATE Exercise
        //SET Name = 'BLoop', Language = 'Bleep'
        //WHERE Exercise.Id = 3;

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Exercise exercise)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Exercise 
                                           SET Name = @name, 
                                               Language = @language
                                              
                                         WHERE Exercise.id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@name", exercise.ExerciseName));
                        cmd.Parameters.Add(new SqlParameter("@language", exercise.ExerciseLang));
                        
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: Exercises/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exercises/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




        private Exercise GetExerciseById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id AS ExerciseId,
                                               e.Name AS ExerciseName,
                                                e.Language AS ExerciseLanguage
                                          FROM Exercise e 
                                         WHERE  e.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            ExerciseLang = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                        };
                    }

                    reader.Close();

                    return exercise;
                }
            }

        }





    }
}



