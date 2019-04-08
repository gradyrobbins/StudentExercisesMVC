
using System;
using System.Collections.Generic;
using System.Text;


// You must define a type for representing an instructor in code.

// First name
// Last name
// Slack handle
// The instructor's cohort
// A method to assign an exercise to a student


namespace StudentExercisesMVC.Models
{
    public class Instructor {
        public int Id { get; set; }
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string SlackHandle {get;set;}
        public int CohortId {get;set;}
        public Cohort Cohort { get; set; }

        // A method to assign an exercise to a student
        public void AssignAnExercise(Student student, Exercise exercise) {
             {
                student.ExerciseList.Add(exercise);
                Console.WriteLine($"{FirstName} assigned {exercise.ExerciseName} to {student.FirstName} ");
            };
        }
       /*public Instructor(string firstName, string lastName, string slackHandle, int cohortId )
        {
            FirstName = firstName;
            LastName = lastName;
            SlackHandle = slackHandle;
            CohortId = cohortId;
        }*/

        // updated instructions from Andy 3/26
        // 
        /*{
            "id": 1,
            "firstName": "Jisie",
            "lastName": "David",
            "slackHandle": "@jisie",
            "cohortId": 2,
            "cohort": {
                "id": 2,
                "name": "Cohort 29",
                "students": [],
                "instructors": []
            }
            
        },*/    


    }

}