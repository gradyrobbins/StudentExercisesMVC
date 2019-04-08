namespace StudentExercisesMVC.Models

// You must define a type for representing an exercise in code. An exercise can be assigned to many students.

// Name of exercise
// Language of exercise (JavaScript, Python, CSharp, etc.)

{
   public class Exercise
    {
        public Exercise(string exerciseName, string exerciseLang) {
            ExerciseName = exerciseName;
            ExerciseLang = exerciseLang;

        }
        public string ExerciseName {get;set;}
        public string ExerciseLang {get;set;}

    }

}