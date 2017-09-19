using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AnboDatabaseSoapProviderWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string ConnectionString =
           //"Server=tcp:anboserver.database.windows.net,1433;Initial Catalog=anboSchoolDatabase;Persist Security Info=False;User ID=anbo;Password=Secret12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
           "Server=tcp:anbo-databaseserver.database.windows.net,1433;Initial Catalog=anbobase;Persist Security Info=False;User ID=anbo;Password=Secret12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IList<Student> GetAllStudents()
        {
            const string selectAllStudents = "select * from student order by name";
            IList<Student> result = new List<Student>();
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllStudents, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Student student = ReadStudent(reader);
                                result.Add(student);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static Student ReadStudent(IDataRecord reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int semester = reader.GetInt32(2);
            DateTime timeStamp = reader.GetDateTime(3);
            Student student = new Student
            {
                Id = id,
                Name = name,
                Semester = semester,
                TimeStamp = timeStamp
            };
            return student;
        }

        public Student GetStudentById(int id)
        {
            const string selectStudent = "select * from student where id=@id";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStudent, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        reader.Read(); // Advance cursor to first row
                        Student student = ReadStudent(reader);
                        return student;
                    }
                }
            }
        }

        public IList<Student> GetStudentsByName(string name)
        {
            string selectStr = "select * from student where name = @name";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStr, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@name", name);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        IList<Student> students = new List<Student>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Student st = ReadStudent(reader);
                                students.Add(st);
                            }
                        }
                        return students;
                    }
                }
            }
        }

        public int AddStudent(string name, byte semester)
            {
                const string insertStudent = "insert into student (name, semester) values (@name, @semester)";
                using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
                {
                    databaseConnection.Open();
                    using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
                    {
                        insertCommand.Parameters.AddWithValue("@name", name);
                        insertCommand.Parameters.AddWithValue("@semester", semester);
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        return rowsAffected;
                    }
                }
            }
        }
    }
