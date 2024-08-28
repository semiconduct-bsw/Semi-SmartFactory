using _20240828_ModelDataMVC.Models;

namespace _20240828_ModelDataMVC.Repository
{
    public interface IStudent
    {
        List<StudentModel> getAllStudents();
        StudentModel getStudentById(int id);
    }
}
