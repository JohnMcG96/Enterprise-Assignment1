using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program menu = new Program();

            using (var db = new AssignmentModelContext())
            {
                Console.WriteLine("\nUNIVERSITY ATTEDNANCE REGISTER");
                Console.WriteLine("______________________________");
                
                while (true)
                {
                    Console.WriteLine("Please Select from the following options.");
                    Console.WriteLine("1. Display All Modules");
                    Console.WriteLine("2. Find Instructor By Module");
                    Console.WriteLine("3. Display Module Learning Events");
                    Console.WriteLine("4. Display Attendance By Student");
                    Console.WriteLine("5. Display Students Missing Events");
                    Console.WriteLine("6. Display Attendance Report");
                    Console.WriteLine("0. Exit");
                    int choice = 0;
                    Console.Write("ENTER Your Selection: ");
                    while (!(int.TryParse(Console.ReadLine(), out choice) && (choice >= 0 && choice <= 6)))
                    {
                        Console.WriteLine("You need to enter a value 0..7\n");
                        Console.Write("ENTER Your Selection: ");
                    }
                    Console.WriteLine("____________________");
                    switch (choice)
                    {
                        case 1:
                            menu.ModuleList(db);
                            break;
                        case 2:
                            menu.StaffByModule(db);
                            break;
                        case 3:
                            menu.LearningEventByModule(db);
                            break;
                        case 4:
                            menu.StudentAttendanceByModule(db);
                            break;
                        case 5:
                            menu.StudentsMissing(db);
                            break;
                        case 6:
                            menu.ModuleAttendance(db);
                            break;
                        case 0:
                            Console.WriteLine("Thank you for using the system. Press any key to exit");
                            Console.ReadKey();
                            return;
                    }
                }
            }
        }

        public void ModuleList(AssignmentModelContext dbC)
        {
            var modules = dbC.Modules.OrderBy(m => m.ModuleID);
            if (modules != null)
            {
                Console.WriteLine("MODULE LIST");
                Console.WriteLine("___________");
                var sb = new StringBuilder();
                sb.Append(String.Format("{0,-15} {1,-10}\n", "MODULE CODE", "MODULE NAME"));
                foreach (var item in modules)
                {
                    sb.Append(String.Format("{0,-15} {1,-10}\n", item.ModuleCode, item.ModuleName));
                }
                Console.WriteLine(sb);
                Console.WriteLine("____________________________");
            }
            else
            {
                Console.WriteLine("No modules found");
                Console.WriteLine("____________________________");
            }
        }

        public void StaffByModule(AssignmentModelContext dbC)
        {
            Console.WriteLine("STAFF BY MODULE LIST");
            Console.WriteLine("____________________\n");
            Console.Write("Please enter Module code: ");
            string moduleCode = Console.ReadLine();

            var staffModule = from mod in dbC.Modules
                              join sta in dbC.Instructors on mod.InsturctorID equals sta.StaffID into staffGroup
                              from item in staffGroup.DefaultIfEmpty()
                              where mod.ModuleCode.Equals(moduleCode)
                              select new { item.StaffNum, item.StaffName };

            if (staffModule != null)
            {
                Console.WriteLine();
                var sb = new StringBuilder();
                sb.Append(String.Format("{0,-15} {1,-30}\n", "STAFF NUMBER", "STAFF NAME"));
                foreach (var item in staffModule)
                {
                    sb.Append(String.Format("{0,-15} {1,-30}\n", item.StaffNum, item.StaffName));
                }
                Console.WriteLine(sb);
                Console.WriteLine("____________________________");
            }
            else
            {
                Console.WriteLine("NO MODULES FOUND...");
                Console.WriteLine("____________________________");
            }
        }

        public void LearningEventByModule(AssignmentModelContext dbC)
        {
            Console.WriteLine("EVENTS BY MODULE LIST");
            Console.WriteLine("______________________\n");
            Console.Write("Please enter Module code: ");
            string moduleCode = Console.ReadLine();

            var eventModule = from mod in dbC.Modules
                              join eve in dbC.LearningEvents on mod.ModuleID equals eve.ModuleID into eventGroup
                              from item in eventGroup.DefaultIfEmpty()
                              where mod.ModuleCode.Equals(moduleCode)
                              select new { item.EventDateTime, item.EventType };

            if (eventModule != null)
            {
                Console.WriteLine();
                var sb = new StringBuilder();
                sb.Append(String.Format("{0,-15} {1,-30}\n", "EVENT TYPE", "EVENT DATE & TIME"));
                foreach (var item in eventModule)
                {
                    sb.Append(String.Format("{0,-15} {1,-30}\n", item.EventType, item.EventDateTime));
                }
                Console.WriteLine(sb);
                Console.WriteLine("____________________________");
            }
            else
            {
                Console.WriteLine("MODULE NOT FOUND...");
                Console.WriteLine("____________________________");
            }
        }

        public void StudentAttendanceByModule(AssignmentModelContext dbC)
        {
            Console.WriteLine("ATTENDANCE BY MODULE LIST");
            Console.WriteLine("_________________________\n");
            Console.Write("Please enter Module code: ");
            string moduleCode = Console.ReadLine();


            var eventModule = dbC.Modules.FirstOrDefault(m => m.ModuleCode == moduleCode);

            if (eventModule != null)
            {
                Console.Write("Please enter Student ID: ");
                string studentID = Console.ReadLine();
                var student = dbC.Students.FirstOrDefault(s => s.StuNum == studentID);

                if (student != null) // module and student exists
                {
                    Console.WriteLine();
                    Console.WriteLine($"ATTENDANCE RECORD FOR { student.StuFName} { student.StuSName}");
                    var sb = new StringBuilder();
                    sb.Append(String.Format("{0,-25} {1,-10} {2,-10}\n", "DATE/TIME", "TYPE", "ATTENDED"));

                    var learningEvents = dbC.LearningEvents.Where(m => m.Module.ModuleCode == moduleCode)
                                            .OrderBy(lEvent => lEvent.EventDateTime);

                    foreach (var item in learningEvents)
                    {
                        var attendances = item.Attendances.Where(s => s.StudentID == student.StuID);
                        foreach (var att in attendances)
                        {
                            sb.Append(String.Format("{0,-25} {1,-10} {2,-10}", item.EventDateTime, item.EventType, att.AttendStatus));
                        }
                    }
                    Console.WriteLine(sb);
                    Console.WriteLine();
                    Console.WriteLine("____________________________");
                }
                else
                {
                    Console.WriteLine("STUDENT NOT FOUND...");
                    Console.WriteLine("____________________________");
                }

            }
            else
            {
                Console.WriteLine("MODULE NOT FOUND...");
                Console.WriteLine("____________________________");
            }
        }

        public void StudentsMissing(AssignmentModelContext dbC)
        {
            Console.WriteLine("STUDENTS OVER 2 ABSENSES");
            Console.WriteLine("________________________\n");
            var missing = dbC.Attendances.SqlQuery("SELECT COUNT(Stu.StuID) AS Absenses, Stu.StuID FROM " +
                "Student AS Stu " +
                "JOIN Attendance AS Att ON Stu.StuID = Att.StudentID" +
                "WHERE Att.AttendStatus = 0" +
                "GROUP BY Stu.StuID" +
                "HAVING COUNT(Stu.StuID) >= 2");
            if (missing != null)
            {
                var sb = new StringBuilder();
                sb.Append(String.Format("{0,-15} {1,-30} {2,-30}\n", "STUDENT ID", "STUDENT FORENAME", "STUDENT SURNAME"));

                foreach (var item in missing)
                {
                    var student = dbC.Students.Where(m => m.StuID == item.StudentID);
                    foreach (var missingStu in student)
                    {
                        sb.Append(String.Format("{0,-15} {1,-30} {2,-30}", missingStu.StuID, missingStu.StuFName, missingStu.StuSName));
                    }
                }
                Console.WriteLine(sb);
                Console.WriteLine();
                Console.WriteLine("____________________________");
            }
            else
            {
                Console.WriteLine("ALL STUDENTS ACCEPTABLY PRESENT...");
                Console.WriteLine("____________________________");
            }
        }

        public void ModuleAttendance(AssignmentModelContext dbC)
        {
            Console.WriteLine("MODULE ATTENDANCE REPORT");
            Console.WriteLine("________________________\n");

            var attendReport = dbC.Modules.SqlQuery("SELECT M.ModuleName, LE.EventID, " +
                "COUNT(Att.AttendStatus) AS Students_Attended FROM Module AS M" +
                "JOIN LearningEvent AS LE ON M.ModuleID = LE.ModuleID" +
                "JOIN Attendance AS Att ON LE.EventID = Att.EventID" +
                "WHERE Att.AttendStatus = 1 AND M.ModuleCode = @code" +
                "GROUP BY M.ModuleName, LE.EventID" +
                "ORDER BY LE.EventID");
            if (attendReport != null)
            {
                Console.WriteLine();
                var sb = new StringBuilder();
                sb.Append(String.Format("{0,-30} {1,-10} {2,-25}\n", "MODULE NAME", "EVENT ID", "NO. STUDENTS ATTENDED"));
                foreach (var item in attendReport)
                {
                    //sb.Append(String.Format("{0,-30} {1,-10} {2,-25}\n", item.ModuleName, item.EventID, item.Students_Attended));
                }
                Console.WriteLine(sb);
                Console.WriteLine("____________________________");
            }
            else
            {
                Console.WriteLine("Module not found...");
            }
        }
    }
}

   

