
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


// You must define a type for representing a cohort in code.

// The cohort's name (Evening Cohort 6, Day Cohort 25, etc.)
// The collection of students in the cohort.
// The collection of instructors in the cohort.

namespace StudentExercisesMVC.Models
{
    public class Cohort {
        public int Id { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 5)]
        //Message "Cohort name should be in fofmat of[Day|Evening [number]"
        [RegularExpression(@"(\bday\b|\bDay\b|\bevening\b|\bEvening\b)\s(\b\d{1,2})")]

        public string Name {get; set;}
        public List<Student> rosterOfStudents { get; set; } = new List<Student>();
        public List<Instructor> rosterOfInstructors { get; set; } = new List<Instructor>();

       

    }

}