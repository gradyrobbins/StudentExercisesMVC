
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


// You must define a type for representing a student in code. A student can only be in one cohort at a time. A student can be working on many exercises at a time.

// Properties
// First name
// Last name
// Slack handle
// The student's cohort
// The collection of exercises that the student is currently working on

namespace StudentExercisesMVC.Models
{
    public class Student{
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName {get ; set; }

        [Display(Name = "Last Name")]
        public string LastName {get; set; }

        [Display(Name = "Slack Handle")]
        public string SlackHandle {get; set;}

        [Display(Name = "Cohort Id")]
        public int CohortId {get; set;}


        public Cohort Cohort { get; set; }

        public List<Exercise> ExerciseList { get; set; } = new List<Exercise>();

        [Display(Name = "Name")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

       
    }

}