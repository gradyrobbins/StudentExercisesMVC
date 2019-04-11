using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
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


        // GET: Students
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
           SELECT  s.[Id]
      ,[FirstName]
      ,[LastName]
      ,[SlackHandle]
      ,[CohortId]
	  ,[Name]
  FROM Student s
  INNER JOIN Cohort c
  ON s.CohortId = c.Id;
        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                CohortName = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return View(students);
                }
            }
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                         SELECT  s.Id
                                                 ,s.FirstName
                                                 ,s.LastName
                                                 ,s.SlackHandle
                                                 ,s.CohortId
	                                             ,c.Name
                                         FROM Student s
                                           
                                         LEFT JOIN Cohort c
                                         ON s.CohortId = c.Id
                                            WHERE s.Id = @id
                                         ;
                ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));



                    SqlDataReader reader = cmd.ExecuteReader();



                    Student student = null;
                    while (reader.Read())
                    {

                        
                            student = new Student
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    CohortName = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                        
                    }
                        reader.Close();

                        return View(student);
                    }
                }
            }
        
        

            // GET: Students/Create
            public ActionResult Create()
            {
                return View();
            } 

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Edit/5
        // don't fully understand what's going on here.  Why 171?
        public ActionResult Edit(int id)
        {
            Student student = GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            StudentEditViewModel viewModel = new StudentEditViewModel
            {
                Cohorts = GetAllCohorts(),
                Student = student
            };

            return View(viewModel);
        }


        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(int id, StudentEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Student 
                                           SET FirstName = @firstname, 
                                               LastName = @lastname,
                                               SlackHandle = @slackhandle, 
                                               CohortId = @cohortid
                                         WHERE id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@firstname", viewModel.Student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastname", viewModel.Student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@slackhandle", viewModel.Student.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@cohortid", viewModel.Student.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }
        }


        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            // establish new connection
            using (SqlConnection conn = Connection)
            //open connection
            {
                conn.Open();
                //using SQLCMD ; create command
                using(SqlCommand cmd = conn.CreateCommand())
                //SQL statement
                {
                    cmd.CommandText = @"SELECT s.Id AS studentId,
                                               s.FirstName,
                                               s.LastName,
                                               s.SlackHandle,
                                               s.CohortId,
                                               c.Name as CohortName
                                                FROM Student s LEFT JOIN Cohort c on s.cohortId = c.id
                                                WHERE s.id = @id";
                    //build in an alias/command parameters for it to recognize to capture @id
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    //cmd.execute reader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // instantiate new Student object, declare as null.
                    Student student = null;

                    //conditional-IF reader.read (meaning, did the SQL query return data?), take data from SQL query
                    if (reader.Read())
                    {
                    //grab student object from DB, rewrite/reassign the 'Student student' by using the SQLreader to capture object values 
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                        // close the reader
                        reader.Close();
                        // pass the student object's updated values in as argument into the return statement below.
                    }
                    //why return View(student) why not return student? 
                    return View(student);
                }





                 
                
            }
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            //try/catch:  try the first block; if exception is thrown , CATCH it
           // try
            //{
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                       
                         cmd.CommandText = @"DELETE FROM Student
                                            WHERE Id = @id";
                    
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            
                        {
                        return RedirectToAction(nameof(Index));
                    }
                        
                        
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                   

                }

                   
            //}
           /* catch (Exception)
            {
                

                return RedirectToAction(nameof(Index));

            }*/
        }





        // below from StudentExercisesAPI

        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //try/catch:  try the first block; if exception is thrown, CATCH it
            try
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                
            }
            catch (Exception)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }*/
        



        //declare boolean property T/F - is data returned from the query?
        private bool StudentExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                //open the connection
                conn.Open();
                //create new command using  method built into System.Data.SqlClient
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //@ character designates multi-line SQL query
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName
                        FROM Student
                        WHERE Id = @id";
                    //define a new parameter called 'id' to capture any student by their id
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }










        private Student GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id AS StudentId,
                                               s.FirstName, s.LastName, 
                                               s.SlackHandle, 
                                                s.CohortId,
                                               c.Name  as CohortName
                                          FROM Student s LEFT JOIN Cohort c on s.cohortid = c.id
                                         WHERE  s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                    }

                    reader.Close();

                    return student;
                }
            }

        }

        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, name from Cohort;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("name"))
                        });
                    }
                    reader.Close();

                    return cohorts;
                }
            }

        }





    }
}