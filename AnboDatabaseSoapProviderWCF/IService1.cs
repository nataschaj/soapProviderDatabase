using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace AnboDatabaseSoapProviderWCF
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Student> GetAllStudents();

        [OperationContract]
        Student GetStudentById(int id);

        [OperationContract]
        IList<Student> GetStudentsByName(string name);

        [OperationContract]
        int AddStudent(string name, byte semester);
    }

    [DataContract]
    public class Student
    {
        [DataMember]
        public int Id;

        [DataMember]
        public DateTime TimeStamp;

        [DataMember]
        public string Name;

        [DataMember]
        public int Semester; // SQL Server tinyint = C# byte
    }
}