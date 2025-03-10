# Student-Management-and-Section-Management-System-project

A simple Student Section Management System built using ASP.NET Core MVC (C#) with Entity Framework Core.
This system allows:

- Managing Students (Add, Edit, Delete)
- Managing Subjects (Add, Edit, Delete)
- Managing Schedules (Add, Edit, Delete)
- Enroll Students to Schedules (Multiple Enrollment)
- Prevent Duplicate Schedules (Same time for different subjects)
- View Enrolled Students in a Schedule

Features
- Schedule Management - Allows adding schedules. Prevents duplicate time slots.
- Student Management - Add, update, and delete students.
- Subject Management - Add, update, and delete subjects.
- Student Enrollment - Allows enrolling students to multiple schedules. Prevents time conflict.
- Data Validation - Prevents same time schedules for different subjects. Ensures end time > start time.
- TempData Messaging - Shows success/error messages after every action.

Step-by-Step Setup Guide

Step 1: Clone the Repository
Open your terminal (or Command Prompt) and run:
git clone https://github.com/your-github-username/Student_Section_ManagementSystemProject.git

Step 2: Navigate to Project Folder
cd Student_Section_ManagementSystemProject

Step 3: Run the Application

Run the application using:
dotnet run

Step 4: Test the Application
You can now:

- Add Students
- Add Subjects
- Create Schedules
- Enroll Students to Multiple Schedules
- Prevent Time Conflict between Subjects

How to Use the Application

ADDING STUDENTS
- To add a student, go to the Students page and click on the Add Student button. Enter the Student Name and Email in the provided fields, then click Save to successfully add the student.

EDITING/UPDATING STUDENTS
- To edit or update a student, go to the Students page, select the student you want to edit, and click the Edit link. Update the Student Name or Email as needed, then click Save to apply the changes.

DELETING STUDENTS
- To delete a student, go to the Students page, select the student you want to remove, and click the Delete link to permanently remove the student.

ADDING SUBJECTS
- To add a subject, go to the Subjects page and click the Add Subject button. Enter the Subject Name and Description, then click Save to add the subject to the list.

EDITING/UPDATING SUBJECTS
- To edit or update a subject, go to the Subjects page, select the subject you want to modify, and click the Edit link. Make the necessary changes to the Subject Name or Description, then click Save to update the subject.

DELETING SUBJECTS
- To delete a subject, go to the Subjects page, select the subject you want to delete, and click the Delete link to permanently remove the subject.

ADDING SCHEDULES
- To add a schedule, go to the Schedules page and click the Add Schedule button. Select the Subject, set the Start Time and End Time, then click Save to create the schedule.

ENROLL STUDENT TO SCHEDULE
- To enroll a student in a schedule, go to the Schedules page and click the Details link of your desired schedule. From the dropdown, select a student and click Enroll Student to successfully enroll the student in the schedule.

How In-Memory Storage Works

In this project:

- No Database like SQL Server or MySQL is used.
- Instead, all data (students, subjects, schedules, enrollments) is stored in-memory using C# List<T>.
- Data will reset every time you restart the application.
- The data is managed using ApplicationDbContext (in-memory context).
