using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models

// You must define a type for representing an exercise in code. An exercise can be assigned to many students.

// Name of exercise
// Language of exercise (JavaScript, Python, CSharp, etc.)

{
   public class Exercise
    {

       
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exercise Name")]
        public string ExerciseName { get; set; }

        [Required]
        [Display(Name = " Language")]
        public string ExerciseLang { get; set; }

    }

}